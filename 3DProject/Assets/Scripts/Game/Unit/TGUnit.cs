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


    protected void TakeItem(TGItem item)    // item 습득 시도 메소드
    {
        if(item == null) return;                        // 선택된 item object가 null일 경우 메소드 종료

        inventory[item.itemType] = item;
        inventory[item.itemType].OnPickedUpThisItem(gameObject);        // Item이 습득 됐을 때 item instance가 실행되야 할 명령 수행
        Debug.Log(gameObject.name + "이 " + item.name + "을 줏었습니다.");
    }

    protected void DropItem(ItemType itemType)  // item 드랍 시도 메소드
    {
        inventory[itemType] = null;
    }
}
