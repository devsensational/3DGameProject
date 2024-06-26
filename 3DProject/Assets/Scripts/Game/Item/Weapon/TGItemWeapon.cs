using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TGItemWeapon : TGItem
{
    // Insepector
    public GameObject ProjectilePrefab;    //발사 시 생성될 총알 프리팹
    public GameObject Muzzle;              //총알이 발사될 위치
    
    // public
    public MWeaponStats weaponStats { get; protected set; }

    // private
    WaitForSeconds reloadWaitForSeconds;

    bool isReloading = false;

    //Unity lifetime
    protected override void ChildStart()
    {
        weaponStats = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // 무기 스탯 불러오기

        equipmentType = weaponStats.weaponType;
        reloadWaitForSeconds = new WaitForSeconds(weaponStats.reloadTime); // 재장전 시간 코루틴용
    }

    // 공격 메커니즘 관련 메소드
    public override void UseItem()
    {
        if (weaponStats.currentAmmo <= 0) return; // 장탄 수가 0이면 실행 안함
        

        weaponStats.currentAmmo--;
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    public virtual void CommandReload()     // 장전 메소드를 외부로 부터 호출
    {
        if (isReloading) return;                                   // 장전을 안하고 있을 때만 장전
        if (weaponStats.currentAmmo >= weaponStats.maxAmmo) return;  // 장탄 수가 최대 장탄 수 보다 적을 때만 장전

        TGCharacter itemHolderCharacter = itemHolder.GetComponent<TGCharacter>();

        if (!itemHolderCharacter.inventory.TryGetValue(weaponStats.ammoType, out TGItem item)) return; // 키가 없으면 중단
        if (item != null && item.itemCount > 0) // 아이템이 1개 이상 존재하면 실행
        {
            StartCoroutine(Reload());
        }
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
        ammoItem.itemCount -= ammoCount;        // 최대 장탄 수 까지만 아이템 감소
        weaponStats.currentAmmo += ammoCount;   // 최대 장탄 수 만큼만 장전, 현재 장탄 수 보존

        // UI 업데이트
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        ammoItem.itemButton.SetItemName();

        isReloading = false;
        Debug.Log($"(TGItemWeapon:Reload) End reload, current Ammo = {weaponStats.currentAmmo}");
    }

    //
    private void AccurateCalc()
    {
        float accurate = Random.Range(-characterStats.currentAccuracy, characterStats.currentAccuracy);
    }
}
