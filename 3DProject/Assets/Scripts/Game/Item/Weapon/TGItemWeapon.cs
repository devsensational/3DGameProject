using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeapon : TGItem
{
    //public
    GameObject ProjectilePrefab;    //발사 시 생성될 총알 프리팹
    GameObject Muzzle;              //총알이 발사될 위치

    //private
    MWeaponStats    weaponStats     { get; set; }
    MCharacterStats characterStats  { get; set; }
    //Unity lifetime
    protected override void ChildAwake()
    {
        weaponStats = new MWeaponStats();
    }

    private void TemporarySetWeaponStats()
    {

    }

    // 공격 메커니즘 관련 메소드
    protected virtual void Fire()
    {

    }

    protected virtual void Reload()
    {

    }

    //
    private void AccurateCalc()
    {
        float accurate = Random.Range(-characterStats.currentAccurate, characterStats.currentAccurate);
    }
}
