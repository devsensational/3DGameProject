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
        inventory.Add(ItemType.PrimaryWeapon, null);
        inventory.Add(ItemType.SecondaryWeapon, null);

        ChildAwake();
    }


    protected void TakeItem(TGItem item)    // item ���� �õ� �޼ҵ�
    {
        if(item == null) return;                        // ���õ� item object�� null�� ��� �޼ҵ� ����

        inventory[item.itemType] = item;
        inventory[item.itemType].OnPickedUpThisItem(gameObject);        // Item�� ���� ���� �� item instance�� ����Ǿ� �� ��� ����
        Debug.Log(gameObject.name + " picked up " + item.name);
    }

    protected void DropItem(ItemType itemType)  // item ��� �õ� �޼ҵ�
    {
        inventory[itemType] = null;
    }

    public void CommandMove(Vector3 direction, float moveSpeed)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    protected virtual void ChildAwake() {}

}
