using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

//적이 되는 캐릭터를 정의합니다.
public class TGEnemyCharacter : TGCharacter
{
    // Inspector
    public GameObject damageText;
    public GameObject primaryWeapon;
    public List<GameObject> inventoryInitList = new List<GameObject>();

    public float nextFireIntervalTime = 2f;
    public float burstCount = 5f;

    // private
    WaitForSeconds autoWeaponFireWaitForSeconds;
    WaitForSeconds nextFireIntervalWaitForSeconds;

    float timer = 5f;

    // Unity lifecycle
    void Start()
    {
        nextFireIntervalWaitForSeconds = new WaitForSeconds(nextFireIntervalTime);

        for(int i = 0; i < inventoryInitList.Count; i++) 
        {
            TGItem itemPtr = Instantiate(inventoryInitList[i].GetComponent<TGItem>());
            TakeItem(itemPtr);
        }

        if(primaryWeapon != null )
        {
            TGItemWeapon itemPtr = Instantiate(primaryWeapon, HandPosition.transform).GetComponent<TGItemWeapon>();

            TakeItem(itemPtr);
            ChangeInHandItem(null, itemPtr);

            Invoke("InitEquip", 1);
        }
    }

    void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
        else if (timer < 0f)
        {
            DetermineFire();
        }
    }

    // Init
    private void InitEquip()
    {
        TGItemWeapon itemPtr = (TGItemWeapon)equipItems[HandInItem];
        itemPtr.currentAmmo = 0;
        autoWeaponFireWaitForSeconds = new WaitForSeconds(60 / itemPtr.weaponStats.fireRate );
    }

    // 유닛 AI 관련
    private void DetermineFire()
    {
        if (HandInItem == EEquipmentType.None || HandInItem == EEquipmentType.Default) return;

        TGItemWeapon itemPtr = (TGItemWeapon)equipItems[HandInItem];

        if (itemPtr.currentAmmo <= 0) // 탄이 없을 경우 재장전
        {
            CommandReloadInHandItem();
            timer = itemPtr.weaponStats.reloadTime + 1;
            return;
        }

        if (itemPtr.currentAmmo >= 0)
        {
            StartCoroutine(Fireintermittently());
        }

        timer = nextFireIntervalTime;
    }

    private IEnumerator Fireintermittently()
    {
        for (int i = 0; i < burstCount; i++)
        {
            equipItems[HandInItem].UseItem();
            yield return autoWeaponFireWaitForSeconds;
        }
    }

    public override void ReceiveDamage(float damageValue)
    {
        base.ReceiveDamage(damageValue);
        TMP_Text text = Instantiate(damageText, transform.position + new Vector3(0, 2, 0), Quaternion.identity).GetComponent<TMP_Text>();
        text.text = damageValue.ToString();
    }

    protected override void OnDeadCharacter()
    {
        base.OnDeadCharacter();


    }
}
