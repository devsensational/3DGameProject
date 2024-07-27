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

    // Unity lifecycle
    protected override void Start()
    {
        base.Start();

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
    }

    // Init
    private void InitEquip()
    {
        TGItemWeapon itemPtr = (TGItemWeapon)equipItems[HandInItem];
        itemPtr.currentAmmo = 0;
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
