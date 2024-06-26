using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    bool isReloading = false;

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
        if (weaponStats.currentAmmo <= 0) return; // ��ź ���� 0�̸� ���� ����
        

        weaponStats.currentAmmo--;
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    public virtual void CommandReload()     // ���� �޼ҵ带 �ܺη� ���� ȣ��
    {
        if (isReloading) return;                                   // ������ ���ϰ� ���� ���� ����
        if (weaponStats.currentAmmo >= weaponStats.maxAmmo) return;  // ��ź ���� �ִ� ��ź �� ���� ���� ���� ����

        TGCharacter itemHolderCharacter = itemHolder.GetComponent<TGCharacter>();

        if (!itemHolderCharacter.inventory.TryGetValue(weaponStats.ammoType, out TGItem item)) return; // Ű�� ������ �ߴ�
        if (item != null && item.itemCount > 0) // �������� 1�� �̻� �����ϸ� ����
        {
            StartCoroutine(Reload());
        }
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

        int ammoCount = Mathf.Clamp(ammoItem.itemCount, 0, weaponStats.maxAmmo - weaponStats.currentAmmo); // ������ ������ ���ҵ� �� ����
        ammoItem.itemCount -= ammoCount;        // �ִ� ��ź �� ������ ������ ����
        weaponStats.currentAmmo += ammoCount;   // �ִ� ��ź �� ��ŭ�� ����, ���� ��ź �� ����

        // UI ������Ʈ
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
