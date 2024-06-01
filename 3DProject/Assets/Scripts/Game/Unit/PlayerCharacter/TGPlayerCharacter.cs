using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGPlayerCharacter : TGUnit
{
    //private
    TGItem handInItem = null;

    private void OnCollisionStay(Collision collision) // collision과 충돌할 때 실행되는 메소드
    {
        Debug.Log("collisionEnter " + collision.gameObject.name);
        if(collision.gameObject.tag == "Item")
        {
            if(collision.gameObject.GetComponent<TGItem>().isDropped) //해당 무기가 떨어져 있는지 확인
            {
                TakeItem(collision.gameObject.GetComponent<TGItem>());
            }
        }
    }

    public void CommandDropItem(ItemType itemType) // "TGPlayerCharacterController"에서 Item 드랍을 호출했을 때 실행
    {
        inventory[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    public void CommandHandInItem(ItemType itemType) // "TGPlayerCharacterController"에서 특정 아이템을 손에 드는 명령을 내릴때 수행
    {
        if(handInItem != null)
        {
            ChangeHandInItem(handInItem, inventory[itemType]);
        } 
        else
        {

        }
    }
    
    private void ChangeHandInItem(TGItem previousItem, TGItem nextItem)
    {
        if(previousItem != null)
        {
            previousItem.enabled = false;
        }
        nextItem.enabled     = true;
    }
}
