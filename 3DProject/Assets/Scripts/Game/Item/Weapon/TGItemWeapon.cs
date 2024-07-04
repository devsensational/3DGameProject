using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

enum EWeaponStateFlags
{
    None        = 0x00,
    Reloading   = 0x01,
    Empty       = 0x02, 
}

public class TGItemWeapon : TGItem
{
    // Insepector
    public GameObject projectilePrefab;    //발사 시 생성될 총알 프리팹
    public GameObject muzzle;              //총알이 발사될 위치
    
    // public
    public MWeaponStats weaponStats { get; protected set; }

    // protected
    protected TGObjectPoolManager objectPoolManager;

    // private
    WaitForSeconds reloadWaitForSeconds;

    bool    isReloading = false;
    int     weaponState = 0;

    //Unity lifetime
    protected override void ChildStart()
    {
        InitReferences();
        InitObjectPool();
    }

    //Init
    void InitReferences()
    {
        weaponStats             = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // 무기 스탯 불러오기
        objectPoolManager       = TGObjectPoolManager.Instance;
        equipmentType           = weaponStats.weaponType;
        reloadWaitForSeconds    = new WaitForSeconds(weaponStats.reloadTime); // 재장전 시간 코루틴용

        Debug.Log($"(TGItemWeapon:Start) Weapon stat loaded! {weaponStats.weaponName}, {weaponStats.defaultAccuracy}");
    }

    void InitObjectPool()
    {
        objectPoolManager.CreateTGObjectPool(ETGObjectType.Projectile, projectilePrefab, 10, 100);
    }

    // 공격 메커니즘 관련 메소드
    public override void UseItem() // 아이템 사용
    {
        if (weaponStats.currentAmmo <= 0) return; // 장탄 수가 0이면 실행 안함

        weaponStats.currentAmmo--;
        FireWeapon();
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    protected virtual void FireWeapon()
    {
        Vector3     muzzlePosition  = muzzle.transform.position;
        Quaternion  direction       = AccurateCalc(weaponStats.currentAccuracy);

        //TGProjectile projectilePtr = objectPoolManager.GetTGObject(ETGObjectType.Projectile).GetComponent<TGProjectile>(); // 오브젝트 풀에서 발사체 활성화
        TGProjectile projectilePtr = Instantiate(projectilePrefab, muzzlePosition, direction).GetComponent<TGProjectile>();

        // currentAccuracy를 반영하여 발사체의 방향을 결정

        //projectilePrefab.transform.position = muzzlePosition;
        projectilePtr.CommandFire(muzzlePosition, direction, weaponStats.bulletVelocity);

        Debug.Log($"(TGItemWeapon:FireWeapon) {objectName} Fires {projectilePtr}");
        // 발사 시 총알이 가운데로 가는 문제 수정 필요 
    }

    public virtual bool CommandReload()     // 장전 메소드를 외부로 부터 호출
    {
        if (isReloading) return false;                                   // 장전을 안하고 있을 때만 장전
        if (weaponStats.currentAmmo >= weaponStats.maxAmmo) return false;  // 장탄 수가 최대 장탄 수 보다 적을 때만 장전

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

        int ammoCount = Mathf.Clamp(ammoItem.itemCount, 0, weaponStats.maxAmmo - weaponStats.currentAmmo); // 아이템 갯수가 감소될 수 결정
        ammoItem.ReduceItemCount(ammoCount);        // 최대 장탄 수 까지만 아이템 감소
        weaponStats.currentAmmo += ammoCount;   // 최대 장탄 수 만큼만 장전, 현재 장탄 수 보존

        // UI 업데이트
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        ammoItem.itemButton.SetItemName();

        isReloading = false;
        Debug.Log($"(TGItemWeapon:Reload) End reload, current Ammo = {weaponStats.currentAmmo}");
    }

    private Quaternion AccurateCalc(float accuracy)    // 명중률 계산 메소드
    {
        float xRotation = Random.Range(-accuracy, accuracy);
        float yRotation = Random.Range(-accuracy, accuracy);

        Quaternion deltaRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        return transform.rotation * deltaRotation;
    }
}
