using System.Collections;
using System.Collections.Generic;
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

    }

    public virtual void CommandReload()
    {

    }

    private IEnumerator Reload()
    {
        yield return reloadWaitForSeconds;
        
    }

    //
    private void AccurateCalc()
    {
        float accurate = Random.Range(-characterStats.currentAccuracy, characterStats.currentAccuracy);
    }
}
