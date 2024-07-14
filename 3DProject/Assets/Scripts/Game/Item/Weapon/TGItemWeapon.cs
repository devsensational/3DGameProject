using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

// TGItem �� ��� ������ ���� �������� �����մϴ�.
public class TGItemWeapon : TGItem
{
    // Insepector
    public GameObject projectilePrefab;    //�߻� �� ������ �Ѿ� ������
    public GameObject muzzle;              //�Ѿ��� �߻�� ��ġ

    // public
    public MWeaponStats weaponStats { get; protected set; }

    public int currentAmmo = 0;

    // protected
    protected TGObjectPoolManager objectPoolManager;


    // private
    WaitForSeconds reloadWaitForSeconds;
    WaitForSeconds fireRateWaitForSeconds;
    WaitForSeconds recoilRecoveryForSeconds;

    bool isReloading = false;
    bool isWeaponReady = true;

    //Unity lifetime
    protected override void ChildStart()
    {
        InitReferences();
        InitObjectPool();
    }

    //Init
    void InitReferences()
    {
        weaponStats                 = TGGameManager.Instance.loadedWeaponStatDict[objectName]; // ���� ���� �ҷ�����
        objectPoolManager           = TGObjectPoolManager.Instance;
        equipmentType               = weaponStats.weaponType;
        reloadWaitForSeconds        = new WaitForSeconds(weaponStats.reloadTime); // ������ �ð� �ڷ�ƾ��
        fireRateWaitForSeconds      = new WaitForSeconds(60 / weaponStats.fireRate); // ���� �ð� �ڷ�ƾ��
        recoilRecoveryForSeconds    = new WaitForSeconds(0.05f); // �ݵ� ȸ�� �ð� �ڷ�ƾ

        Debug.Log($"(TGItemWeapon:Start) Weapon stat loaded! {weaponStats.weaponName}, {this.GetHashCode()}, {weaponStats.defaultAccuracy}");
    }

    void InitObjectPool()
    {
        objectPoolManager.CreateTGObjectPool(ETGObjectType.Projectile, projectilePrefab, 10, 100);
    }

    // ���� ��Ŀ���� ���� �޼ҵ�
    public override void UseItem() // ������ ���
    {
        StartCoroutine(FireWeapon());
        Debug.Log("(TGItemWeapon:UseItem) Used weapon");
    }

    protected virtual IEnumerator FireWeapon()
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
        weaponStats.currentAccuracy = Mathf.Clamp(weaponStats.currentAccuracy * weaponStats.recoilMultiplier, weaponStats.minAccuracy, weaponStats.maxAccuracy);

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
        Quaternion direction = CalculateAccuracy(weaponStats.currentAccuracy); // currentAccuracy�� �ݿ��Ͽ� �߻�ü�� ������ ����
        float mass = CalculateMass(weaponStats.bulletVelocity, weaponStats.range);

        TGProjectile projectilePtr = objectPoolManager.GetTGObject(ETGObjectType.Projectile).GetComponent<TGProjectile>(); // ������Ʈ Ǯ���� �߻�ü Ȱ��ȭ

        projectilePtr.CommandFire(muzzlePosition, direction, weaponStats.bulletVelocity, mass, weaponStats.damage);
        Debug.Log($"(TGItemWeapon:FireWeapon) {objectName} Fires {projectilePtr}");
    }

    protected virtual IEnumerator RecoilRecovery() // �ݵ��� ���� ���ҵ� ���߷� ȸ�� �޼ҵ�
    {
        if(!isWeaponReady) yield break; // ��ź�� �߻�Ǹ� �ݵ� ȸ���� �ߴ���

        weaponStats.currentAccuracy = Mathf.Clamp(weaponStats.currentAccuracy * weaponStats.recoilRecoveryMultiplier, weaponStats.minAccuracy, weaponStats.maxAccuracy);
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
        ammoItem.itemButton.SetItemName();

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
}
