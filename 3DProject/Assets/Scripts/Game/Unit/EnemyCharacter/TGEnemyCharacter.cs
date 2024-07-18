using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//적이 되는 캐릭터를 정의합니다.
public class TGEnemyCharacter : TGCharacter
{
    // Inspector
    public GameObject damageText;
    public GameObject primaryWeapon;

    public float nextFireIntervalTime = 2f;
    public float burstCount = 5f;

    // private
    WaitForSeconds autoWeaponFireWaitForSeconds;
    WaitForSeconds nextFireIntervalWaitForSeconds;

    // Unity lifecycle
    private void Start()
    {
        nextFireIntervalWaitForSeconds = new WaitForSeconds(nextFireIntervalTime);

        if(primaryWeapon != null )
        {
            TGItemWeapon itemPtr = Instantiate(primaryWeapon, HandPosition.transform).GetComponent<TGItemWeapon>();

            TakeItem(itemPtr);
            ChangeInHandItem(null, itemPtr);

            Invoke("InitEquip", 1);
        }
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
        for(int i = 0; i < burstCount; i++)
        {
            equipItems[HandInItem].UseItem();
            yield return autoWeaponFireWaitForSeconds;
        }

        yield return nextFireIntervalWaitForSeconds;
        StartCoroutine(Fireintermittently());
    }

    public override void ReceiveDamage(float damageValue)
    {
        base.ReceiveDamage(damageValue);
        TMP_Text text = Instantiate(damageText, transform.position + new Vector3(0, 2, 0), Quaternion.identity).GetComponent<TMP_Text>();
        text.text = damageValue.ToString();
    }

}
