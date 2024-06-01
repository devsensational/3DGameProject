using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGPlayerCharacter : TGUnit
{
    //private
    TGItem handInItem = null;

    private void OnCollisionStay(Collision collision) // collision�� �浹�� �� ����Ǵ� �޼ҵ�
    {
        Debug.Log("collisionEnter " + collision.gameObject.name);
        if(collision.gameObject.tag == "Item")
        {
            if(collision.gameObject.GetComponent<TGItem>().isDropped) //�ش� ���Ⱑ ������ �ִ��� Ȯ��
            {
                TakeItem(collision.gameObject.GetComponent<TGItem>());
            }
        }
    }

    public void CommandDropItem(ItemType itemType) // "TGPlayerCharacterController"���� Item ����� ȣ������ �� ����
    {
        inventory[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    public void CommandHandInItem(ItemType itemType) // "TGPlayerCharacterController"���� Ư�� �������� �տ� ��� ����� ������ ����
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
