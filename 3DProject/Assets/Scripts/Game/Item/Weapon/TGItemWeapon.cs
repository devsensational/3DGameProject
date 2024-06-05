using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeapon : TGItem
{
    //public
    GameObject ProjectilePrefab;    //�߻� �� ������ �Ѿ� ������
    GameObject Muzzle;              //�Ѿ��� �߻�� ��ġ

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

    // ���� ��Ŀ���� ���� �޼ҵ�
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
