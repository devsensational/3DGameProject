using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적이 되는 캐릭터를 정의합니다.
public class TGEnemyCharacter : TGCharacter
{
    // Inspector
    public GameObject damageText;

    public GameObject primaryWeapon;

    // private
    WaitForSeconds autoWeaponFireWaitForSeconds;

    // Unity lifecycle
    private void Start()
    {
        TGItemWeapon itemPtr = Instantiate(primaryWeapon, HandPosition.transform).GetComponent<TGItemWeapon>();

        TakeItem(itemPtr);
        ChangeInHandItem(null, itemPtr);

        Invoke("InitEquip", 1);
    }

    private void InitEquip()
    {
        TGItemWeapon itemPtr = (TGItemWeapon)equipItems[HandInItem];
        itemPtr.currentAmmo = 200;
        autoWeaponFireWaitForSeconds = new WaitForSeconds(60 / itemPtr.weaponStats.fireRate );

        StartCoroutine(Fireintermittently());
    }

    private IEnumerator Fireintermittently()
    {
        equipItems[HandInItem].UseItem();

        yield return autoWeaponFireWaitForSeconds;
        StartCoroutine(Fireintermittently());
    }

    public override void ReceiveDamage(float damageValue)
    {
        base.ReceiveDamage(damageValue);
        Instantiate(damageText, transform.position, Quaternion.identity);
    }

}
