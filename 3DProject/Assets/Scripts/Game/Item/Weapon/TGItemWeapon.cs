using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeapon : TGItem
{
    //public
    public GameObject ProjectilePrefab;    //�߻� �� ������ �Ѿ� ������
    public GameObject Muzzle;              //�Ѿ��� �߻�� ��ġ

    //private
    MWeaponStats weaponStats     { get; set; }

    //Unity lifetime
    protected override void ChildStart()
    {
        weaponStats = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // ���� ���� �ҷ�����
        itemType = weaponStats.weaponType;
    }

    private void TemporarySetWeaponStats()
    {

    }

    // ���� ��Ŀ���� ���� �޼ҵ�
    public override void UseItem()
    {

    }

    public virtual void Reload()
    {

    }

    //
    private void AccurateCalc()
    {
        float accurate = Random.Range(-characterStats.currentAccuracy, characterStats.currentAccuracy);
    }
}
