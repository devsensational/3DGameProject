using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityChan;
using UnityEngine;

// TGItem 중 장비 가능한 무기 아이템을 정의합니다.
public class TGItemWeapon : TGItem
{
    // Insepector
    public GameObject projectilePrefab;    //발사 시 생성될 총알 프리팹
    public GameObject muzzle;              //총알이 발사될 위치

    // public
    public MWeaponStats weaponStats { get; protected set; }

    public float currentAccuracy = 0.3f;    //현재 명중률
    public int currentAmmo = 0;             //현재 장탄량

    // protected
    protected TGObjectPoolManager objectPoolManager;


    // private
    WaitForSeconds reloadWaitForSeconds;
    WaitForSeconds fireRateWaitForSeconds;
    WaitForSeconds recoilRecoveryForSeconds;

    bool isReloading = false;
    bool isWeaponReady = true;

    // Recoil Pattern 관련 변수
    // Inspector
    [Header("Recoil pattern insepector")]
    public TextAsset    recoilPatternFile;
    public Camera       playerCamera        = null;

    //public float recoilRecoverSpeed = 5.0f;
    public float indexRecoverSpeed  = 1.0f;
    
    [SerializeField]private float currentRecoilIndex = 0f;

    //private
    private List<MRecoilPatternData> recoilDataList;

    private Vector3 targetRecoil    = Vector3.zero;

    //Unity lifetime
    protected override void ChildStart()
    {
        InitReferences();
        InitObjectPool();
        InitRecoilPattern();
    }

    private void Update()
    {
        if (isWeaponReady) //무기가 발사 중이지 않을 때
        {
            RecoverRecoil();
        }
    }

    //Init
    void InitReferences()
    {
        weaponStats                 = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // 무기 스탯 불러오기
        objectPoolManager           = TGObjectPoolManager.Instance;
        equipmentType               = weaponStats.weaponType;
        reloadWaitForSeconds        = new WaitForSeconds(weaponStats.reloadTime); // 재장전 시간 코루틴용
        fireRateWaitForSeconds      = new WaitForSeconds(60 / weaponStats.fireRate); // 연사 시간 코루틴용
        recoilRecoveryForSeconds    = new WaitForSeconds(0.01f); // 반동 회복 시간 코루틴
        currentAccuracy             = weaponStats.minAccuracy;

        Debug.Log($"(TGItemWeapon:Start) Weapon stat loaded! {weaponStats.weaponName}, {this.GetHashCode()}, {weaponStats.defaultAccuracy}");
    }

    void InitObjectPool()
    {
        objectPoolManager.CreateTGObjectPool(ETGObjectType.Projectile, projectilePrefab, 10, 100);
    }

    void InitRecoilPattern()
    {
        if (recoilPatternFile == null) return;

        recoilDataList = TGGameManager.Instance.jsonUtility.LoadJsonFile<List<MRecoilPatternData>>(recoilPatternFile);
        Debug.Log($"(TGItemWeapon:InitRecoilPattern) Recoil pattern data loaded! {recoilDataList[0].x}, {recoilDataList[1].x}");
    }
    // 공격 메커니즘 관련 메소드
    public override void UseItem() // 아이템 사용
    {
        StartCoroutine(FireWeapon());
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    protected virtual IEnumerator FireWeapon() // 무기가 발사되는 과정
    {
        if (currentAmmo <= 0) yield break; // 장탄 수가 0이면 실행 안함
        if (isReloading) yield break; // 장전 중이면 실행 안함
        if (!isWeaponReady) yield break;

        currentAmmo--;
        
        if(itemHolder.tag == "Player")
        {
            TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        }

        isWeaponReady = false;

        // 반동에 의한 명중률 저하 구현
        currentAccuracy = Mathf.Clamp(currentAccuracy * weaponStats.recoilMultiplier, weaponStats.minAccuracy, weaponStats.maxAccuracy);
        ApplyRecoil();

        projectileFire();

        yield return fireRateWaitForSeconds;

        isWeaponReady = true;

        // 자동 발사 무기를 계속 발사해야 하는 경우
        if (weaponStats.fireMode == EGunFireMode.Auto && Input.GetKey(TGPlayerKeyManager.Instance.KeyValuePairs[EKeyValues.Fire]))
        {
            StartCoroutine(FireWeapon());
        }
        StartCoroutine(RecoilRecovery());
    }

    protected virtual void projectileFire()    // 발사체 상태 리셋 후 발사
    {
        // 발사체 리셋
        Vector3 muzzlePosition = muzzle.transform.position;
        Quaternion direction = CalculateAccuracy(currentAccuracy); // currentAccuracy를 반영하여 발사체의 방향을 결정
        float mass = CalculateMass(weaponStats.bulletVelocity, weaponStats.range);

        TGProjectile projectilePtr = objectPoolManager.GetTGObject(ETGObjectType.Projectile).GetComponent<TGProjectile>(); // 오브젝트 풀에서 발사체 활성화

        projectilePtr.CommandFire(muzzlePosition, direction, weaponStats.bulletVelocity, mass, weaponStats.damage);
        Debug.Log($"(TGItemWeapon:FireWeapon) {objectName} Fires {projectilePtr}");
    }

    protected virtual IEnumerator RecoilRecovery() // 반동에 의해 감소된 명중률 회복 메소드
    {
        if(!isWeaponReady) yield break; // 차탄이 발사되면 반동 회복을 중단함

        currentAccuracy = Mathf.Clamp(currentAccuracy * weaponStats.recoilRecoveryMultiplier, weaponStats.minAccuracy, weaponStats.maxAccuracy);
        yield return recoilRecoveryForSeconds;

        StartCoroutine(RecoilRecovery());
    }

    public virtual bool CommandReload()     // 장전 메소드를 외부로 부터 호출
    {
        if (isReloading) return false;                                   // 장전을 안하고 있을 때만 장전
        if (currentAmmo >= weaponStats.maxAmmo) return false;  // 장탄 수가 최대 장탄 수 보다 적을 때만 장전

        TGCharacter itemHolderCharacter = itemHolder.GetComponent<TGCharacter>();

        if (!itemHolderCharacter.inventory.TryGetValue(weaponStats.ammoType, out TGItem item)) return false; // 키가 없으면 중단
        if (item == null || item.itemCount < 0) return false; // 아이템이 1개 이상 존재하면 실행

        StartCoroutine(Reload());
        return true;
    }

    protected override IEnumerator Reload() // 무기 장전 메소드
    {
        //장전 버튼이 눌리자 마자 실행되야 하는 명령어
        Debug.Log("(TGItemWeapon:Reload) Start reload");
        isReloading = true;

        yield return reloadWaitForSeconds; // 장전 시간 만큼 멈춤
        
        //장전이 종료된 후 실행되야 하는 명령어
        TGCharacter itemHolderCharacter = itemHolder.GetComponent<TGCharacter>();
        TGItem ammoItem = itemHolderCharacter.inventory[weaponStats.ammoType];

        int ammoCount = Mathf.Clamp(ammoItem.itemCount, 0, weaponStats.maxAmmo - currentAmmo); // 아이템 갯수가 감소될 수 결정
        ammoItem.ReduceItemCount(ammoCount);        // 최대 장탄 수 까지만 아이템 감소
        currentAmmo += ammoCount;   // 최대 장탄 수 만큼만 장전, 현재 장탄 수 보존

        // UI 업데이트
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);

        if(itemButton != null)
        {
            ammoItem.itemButton.SetItemName();
        }

        isReloading = false;
        Debug.Log($"(TGItemWeapon:Reload) End reload, current Ammo = {currentAmmo}");
    }

    private Quaternion CalculateAccuracy(float accuracy)    // 명중률 계산 메소드
    {
        float xRotation = Random.Range(-accuracy, accuracy);
        float yRotation = Random.Range(-accuracy, accuracy);

        Quaternion deltaRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        return transform.rotation * deltaRotation;
    }

    float CalculateMass(float velocity, float range, float gravity = 9.81f) // 낙차 구현을 위한 발사체 질량 계산 메소드
    {
        // 비행 시간을 계산합니다.
        float time = range / velocity;

        // 발사체가 중력 하에서 떨어지는 거리로부터 질량을 결정합니다.
        // 중력 가속도 하에서의 거리 = 0.5 * g * t^2
        // 여기서 t는 비행 시간입니다.
        float fallDistance = 0.5f * gravity * Mathf.Pow(time, 2);

        // 질량은 발사체가 낙하할 때 일정 거리를 유지하도록 중력과 균형을 맞추는 값입니다.
        // 여기서는 단순히 비행 거리와 비행 시간, 중력으로부터 질량을 역산합니다.
        float mass = fallDistance / (0.5f * gravity * Mathf.Pow(time, 2));

        return mass;
    }

    // 반동 관련 메소드
    public void ApplyRecoil()
    {
        if (itemHolder == null || itemHolder.tag != "Player") return;

        if (recoilDataList.Count == 0)
        {
            Debug.Log("(TGItemWeapon:ApplyRecoil)No recoil patterns available.");
            return;
        }
        Debug.Log("(TGItemWeapon:ApplyRecoil)Applied recoil.");

        int currentRecoilIndexInt = Mathf.FloorToInt(currentRecoilIndex);
        MRecoilPatternData recoilData = recoilDataList[currentRecoilIndexInt];
        currentRecoilIndex = (currentRecoilIndexInt + 1) % recoilDataList.Count;

        Vector3 recoilRotation = new Vector3(-recoilData.y, recoilData.x, 0);
        targetRecoil += recoilRotation;

        playerCamera.GetComponent<TGPlayerFollowMainCameraController>().ApplyRecoil(recoilRotation);
    }

    private void RecoverRecoil()
    {
        if (itemHolder == null || itemHolder.tag != "Player") return;
        if (targetRecoil == Vector3.zero && currentRecoilIndex == Mathf.Floor(currentRecoilIndex)) return;

        if (currentRecoilIndex > 0)
        {
            currentRecoilIndex = Mathf.Max(0, currentRecoilIndex - Time.deltaTime * indexRecoverSpeed);
        }

    }
}
