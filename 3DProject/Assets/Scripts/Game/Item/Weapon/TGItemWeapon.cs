using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeapon : TGItem
{
    // Insepector
    public GameObject ProjectilePrefab;    //�߻� �� ������ �Ѿ� ������
    public GameObject Muzzle;              //�Ѿ��� �߻�� ��ġ
    
    // public
    public MWeaponStats weaponStats { get; protected set; }

    // private
    WaitForSeconds reloadWaitForSeconds;

    //Unity lifetime
    protected override void ChildStart()
    {
        weaponStats = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // ���� ���� �ҷ�����

        equipmentType = weaponStats.weaponType;
        reloadWaitForSeconds = new WaitForSeconds(weaponStats.reloadTime); // ������ �ð� �ڷ�ƾ��
    }

    // ���� ��Ŀ���� ���� �޼ҵ�
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
