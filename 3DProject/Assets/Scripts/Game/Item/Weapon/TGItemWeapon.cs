using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityChan;
using UnityEngine;

// TGItem �� ��� ������ ���� �������� �����մϴ�.
public class TGItemWeapon : TGItem
{
    // Insepector
    public GameObject projectilePrefab;    //�߻� �� ������ �Ѿ� ������
    public GameObject muzzle;              //�Ѿ��� �߻�� ��ġ

    // public
    public MWeaponStats weaponStats { get; protected set; }

    public float currentAccuracy = 0.3f;    //���� ���߷�
    public int currentAmmo = 0;             //���� ��ź��

    // protected
    protected TGObjectPoolManager objectPoolManager;


    // private
    WaitForSeconds reloadWaitForSeconds;
    WaitForSeconds fireRateWaitForSeconds;
    WaitForSeconds recoilRecoveryForSeconds;

    bool isReloading = false;
    bool isWeaponReady = true;

    // Recoil Pattern ���� ����
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
        if (isWeaponReady) //���Ⱑ �߻� ������ ���� ��
        {
            RecoverRecoil();
        }
    }

    //Init
    void InitReferences()
    {
        weaponStats                 = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // ���� ���� �ҷ�����
        objectPoolManager           = TGObjectPoolManager.Instance;
        equipmentType               = weaponStats.weaponType;
        reloadWaitForSeconds        = new WaitForSeconds(weaponStats.reloadTime); // ������ �ð� �ڷ�ƾ��
        fireRateWaitForSeconds      = new WaitForSeconds(60 / weaponStats.fireRate); // ���� �ð� �ڷ�ƾ��
        recoilRecoveryForSeconds    = new WaitForSeconds(0.01f); // �ݵ� ȸ�� �ð� �ڷ�ƾ
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
    // ���� ��Ŀ���� ���� �޼ҵ�
    public override void UseItem() // ������ ���
    {
        StartCoroutine(FireWeapon());
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    protected virtual IEnumerator FireWeapon() // ���Ⱑ �߻�Ǵ� ����
    {
        if (currentAmmo <= 0) yield break; // ��ź ���� 0�̸� ���� ����
        if (isReloading) yield break; // ���� ���̸� ���� ����
        if (!isWeaponReady) yield break;

        currentAmmo--;
        
        if(itemHolder.tag == "Player")
        {
            TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        }

        isWeaponReady = false;

        // �ݵ��� ���� ���߷� ���� ����
        currentAccuracy = Mathf.Clamp(currentAccuracy * weaponStats.recoilMultiplier, weaponStats.minAccuracy, weaponStats.maxAccuracy);
        ApplyRecoil();

        projectileFire();

        yield return fireRateWaitForSeconds;

        isWeaponReady = true;

        // �ڵ� �߻� ���⸦ ��� �߻��ؾ� �ϴ� ���
        if (weaponStats.fireMode == EGunFireMode.Auto && Input.GetKey(TGPlayerKeyManager.Instance.KeyValuePairs[EKeyValues.Fire]))
        {
            StartCoroutine(FireWeapon());
        }
        StartCoroutine(RecoilRecovery());
    }

    protected virtual void projectileFire()    // �߻�ü ���� ���� �� �߻�
    {
        // �߻�ü ����
        Vector3 muzzlePosition = muzzle.transform.position;
        Quaternion direction = CalculateAccuracy(currentAccuracy); // currentAccuracy�� �ݿ��Ͽ� �߻�ü�� ������ ����
        float mass = CalculateMass(weaponStats.bulletVelocity, weaponStats.range);

        TGProjectile projectilePtr = objectPoolManager.GetTGObject(ETGObjectType.Projectile).GetComponent<TGProjectile>(); // ������Ʈ Ǯ���� �߻�ü Ȱ��ȭ

        projectilePtr.CommandFire(muzzlePosition, direction, weaponStats.bulletVelocity, mass, weaponStats.damage);
        Debug.Log($"(TGItemWeapon:FireWeapon) {objectName} Fires {projectilePtr}");
    }

    protected virtual IEnumerator RecoilRecovery() // �ݵ��� ���� ���ҵ� ���߷� ȸ�� �޼ҵ�
    {
        if(!isWeaponReady) yield break; // ��ź�� �߻�Ǹ� �ݵ� ȸ���� �ߴ���

        currentAccuracy = Mathf.Clamp(currentAccuracy * weaponStats.recoilRecoveryMultiplier, weaponStats.minAccuracy, weaponStats.maxAccuracy);
        yield return recoilRecoveryForSeconds;

        StartCoroutine(RecoilRecovery());
    }

    public virtual bool CommandReload()     // ���� �޼ҵ带 �ܺη� ���� ȣ��
    {
        if (isReloading) return false;                                   // ������ ���ϰ� ���� ���� ����
        if (currentAmmo >= weaponStats.maxAmmo) return false;  // ��ź ���� �ִ� ��ź �� ���� ���� ���� ����

        TGCharacter itemHolderCharacter = itemHolder.GetComponent<TGCharacter>();

        if (!itemHolderCharacter.inventory.TryGetValue(weaponStats.ammoType, out TGItem item)) return false; // Ű�� ������ �ߴ�
        if (item == null || item.itemCount < 0) return false; // �������� 1�� �̻� �����ϸ� ����

        StartCoroutine(Reload());
        return true;
    }

    protected override IEnumerator Reload() // ���� ���� �޼ҵ�
    {
        //���� ��ư�� ������ ���� ����Ǿ� �ϴ� ��ɾ�
        Debug.Log("(TGItemWeapon:Reload) Start reload");
        isReloading = true;

        yield return reloadWaitForSeconds; // ���� �ð� ��ŭ ����
        
        //������ ����� �� ����Ǿ� �ϴ� ��ɾ�
        TGCharacter itemHolderCharacter = itemHolder.GetComponent<TGCharacter>();
        TGItem ammoItem = itemHolderCharacter.inventory[weaponStats.ammoType];

        int ammoCount = Mathf.Clamp(ammoItem.itemCount, 0, weaponStats.maxAmmo - currentAmmo); // ������ ������ ���ҵ� �� ����
        ammoItem.ReduceItemCount(ammoCount);        // �ִ� ��ź �� ������ ������ ����
        currentAmmo += ammoCount;   // �ִ� ��ź �� ��ŭ�� ����, ���� ��ź �� ����

        // UI ������Ʈ
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);

        if(itemButton != null)
        {
            ammoItem.itemButton.SetItemName();
        }

        isReloading = false;
        Debug.Log($"(TGItemWeapon:Reload) End reload, current Ammo = {currentAmmo}");
    }

    private Quaternion CalculateAccuracy(float accuracy)    // ���߷� ��� �޼ҵ�
    {
        float xRotation = Random.Range(-accuracy, accuracy);
        float yRotation = Random.Range(-accuracy, accuracy);

        Quaternion deltaRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        return transform.rotation * deltaRotation;
    }

    float CalculateMass(float velocity, float range, float gravity = 9.81f) // ���� ������ ���� �߻�ü ���� ��� �޼ҵ�
    {
        // ���� �ð��� ����մϴ�.
        float time = range / velocity;

        // �߻�ü�� �߷� �Ͽ��� �������� �Ÿ��κ��� ������ �����մϴ�.
        // �߷� ���ӵ� �Ͽ����� �Ÿ� = 0.5 * g * t^2
        // ���⼭ t�� ���� �ð��Դϴ�.
        float fallDistance = 0.5f * gravity * Mathf.Pow(time, 2);

        // ������ �߻�ü�� ������ �� ���� �Ÿ��� �����ϵ��� �߷°� ������ ���ߴ� ���Դϴ�.
        // ���⼭�� �ܼ��� ���� �Ÿ��� ���� �ð�, �߷����κ��� ������ �����մϴ�.
        float mass = fallDistance / (0.5f * gravity * Mathf.Pow(time, 2));

        return mass;
    }

    // �ݵ� ���� �޼ҵ�
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
