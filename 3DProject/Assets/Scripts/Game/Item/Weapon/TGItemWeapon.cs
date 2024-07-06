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
    public GameObject projectilePrefab;    //�߻� �� ������ �Ѿ� ������
    public GameObject muzzle;              //�Ѿ��� �߻�� ��ġ
    
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
        weaponStats             = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // ���� ���� �ҷ�����
        objectPoolManager       = TGObjectPoolManager.Instance;
        equipmentType           = weaponStats.weaponType;
        reloadWaitForSeconds    = new WaitForSeconds(weaponStats.reloadTime); // ������ �ð� �ڷ�ƾ��

        Debug.Log($"(TGItemWeapon:Start) Weapon stat loaded! {weaponStats.weaponName}, {weaponStats.defaultAccuracy}");
    }

    void InitObjectPool()
    {
        objectPoolManager.CreateTGObjectPool(ETGObjectType.Projectile, projectilePrefab, 10, 100);
    }

    // ���� ��Ŀ���� ���� �޼ҵ�
    public override void UseItem() // ������ ���
    {
        if (weaponStats.currentAmmo <= 0) return; // ��ź ���� 0�̸� ���� ����

        weaponStats.currentAmmo--;
        FireWeapon();
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    protected virtual void FireWeapon()
    {
        Vector3     muzzlePosition  = muzzle.transform.position;
        Quaternion  direction       = CalculateAccuracy(weaponStats.currentAccuracy); // currentAccuracy�� �ݿ��Ͽ� �߻�ü�� ������ ����
        float       mass            = CalculateMass(weaponStats.bulletVelocity, weaponStats.range); 

        TGProjectile projectilePtr = objectPoolManager.GetTGObject(ETGObjectType.Projectile).GetComponent<TGProjectile>(); // ������Ʈ Ǯ���� �߻�ü Ȱ��ȭ

        projectilePtr.CommandFire(muzzlePosition, direction, weaponStats.bulletVelocity, mass);

        Debug.Log($"(TGItemWeapon:FireWeapon) {objectName} Fires {projectilePtr}");
        // �߻� �� �Ѿ��� ����� ���� ���� ���� �ʿ� 
    }

    public virtual bool CommandReload()     // ���� �޼ҵ带 �ܺη� ���� ȣ��
    {
        if (isReloading) return false;                                   // ������ ���ϰ� ���� ���� ����
        if (weaponStats.currentAmmo >= weaponStats.maxAmmo) return false;  // ��ź ���� �ִ� ��ź �� ���� ���� ���� ����

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

        int ammoCount = Mathf.Clamp(ammoItem.itemCount, 0, weaponStats.maxAmmo - weaponStats.currentAmmo); // ������ ������ ���ҵ� �� ����
        ammoItem.ReduceItemCount(ammoCount);        // �ִ� ��ź �� ������ ������ ����
        weaponStats.currentAmmo += ammoCount;   // �ִ� ��ź �� ��ŭ�� ����, ���� ��ź �� ����

        // UI ������Ʈ
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        ammoItem.itemButton.SetItemName();

        isReloading = false;
        Debug.Log($"(TGItemWeapon:Reload) End reload, current Ammo = {weaponStats.currentAmmo}");
    }

    private Quaternion CalculateAccuracy(float accuracy)    // ���߷� ��� �޼ҵ�
    {
        float xRotation = Random.Range(-accuracy, accuracy);
        float yRotation = Random.Range(-accuracy, accuracy);

        Quaternion deltaRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        return transform.rotation * deltaRotation;
    }

    float CalculateMass(float velocity, float range) // ���� ������ ���� �߻�ü ���� ��� �޼ҵ�
    {
        // ���� �߻� ��, ���� �ð� ���
        float timeOfFlight = range / velocity;

        // ���� ���
        // ���� �߻��̹Ƿ� ���� �ӵ��� �߷¸� ����ϸ� ��
        // ���ӵ� ���: F = ma ���� a = F/m �̹Ƿ� a = gravity
        // m = F / a = (�߷� ���ӵ� * ���� �ð�^2) / (2 * �߷� ���ӵ�)
        // ���⼭ F�� ���� ���⿡���� �߷°� �����ϰ� ���˴ϴ�.
        float mass = (9.81f * timeOfFlight * timeOfFlight) / 2.0f;

        return mass;
    }
}
