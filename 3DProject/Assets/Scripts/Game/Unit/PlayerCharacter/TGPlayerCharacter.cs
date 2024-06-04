using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    None = 0,

    Forward,
    Backward,
    Left,
    Right,

    End = 999
}

// �÷��̾ ���� �����ϴ� ĳ������ ���� �� ��ȣ�ۿ��� ���� Ŭ�����Դϴ�
public class TGPlayerCharacter : TGCharacter
{
    // public
    public MCharacterStats playerStat { get; set; } // �÷��̾� ĳ���� ����

    // private
    // Capsule Colider ref
    private CapsuleCollider col;
    private Rigidbody       rb;

    //private
    TGItem handInItem = null;   //�÷��̾� ĳ���Ͱ� ��� �ִ� ������ ref

    //Unity lifetime
    protected override void ChildAwake()
    {
        playerStat = new MCharacterStats();
    }

    void OnCollisionStay(Collision collision) // collision�� �浹�� �� ����Ǵ� �޼ҵ�
    {
        if (collision.gameObject.tag == "Item")
        {
            if (collision.gameObject.GetComponent<TGItem>().isDropped) //�ش� ���Ⱑ ������ �ִ��� Ȯ��
            {
                TakeItem(collision.gameObject.GetComponent<TGItem>());
            }
        }
    }

    //item ���� method
    public void CommandDropItem(ItemType itemType) // "TGPlayerCharacterController"���� Item ����� ȣ������ �� ����
    {
        equipItems[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    public void CommandHandInItem(ItemType itemType) // "TGPlayerCharacterController"���� Ư�� �������� �տ� ��� ����� ������ ����
    {
        if(handInItem != null)
        {
            ChangeHandInItem(handInItem, equipItems[itemType]);
        } 
        else
        {

        }
    }
    private void ChangeHandInItem(TGItem previousItem, TGItem nextItem)
    {
        if (previousItem != null)
        {
            previousItem.enabled = false;
        }
        nextItem.enabled = true;
    }
}
