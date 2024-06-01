using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUnit : TGObject
{
    //public

    //protected
    protected Dictionary<ItemType, TGItem> inventory = new Dictionary<ItemType, TGItem>();

    //private

    private void Awake()
    {
        inventory.Add(ItemType.PRIMARYWEAPON, null);
        inventory.Add(ItemType.SECONDARYWEAPON, null);

        
    }


    protected void TakeItem(TGItem item)    // item ���� �õ� �޼ҵ�
    {
        if(item == null) return;                        // ���õ� item object�� null�� ��� �޼ҵ� ����

        inventory[item.itemType] = item;
        inventory[item.itemType].OnPickedUpThisItem(gameObject);        // Item�� ���� ���� �� item instance�� ����Ǿ� �� ��� ����
        Debug.Log(gameObject.name + "�� " + item.name + "�� �޾����ϴ�.");
    }

    protected void DropItem(ItemType itemType)  // item ��� �õ� �޼ҵ�
    {
        inventory[itemType] = null;
    }
}
