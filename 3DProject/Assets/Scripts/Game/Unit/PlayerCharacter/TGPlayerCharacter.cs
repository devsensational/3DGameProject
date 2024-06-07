using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
    public List<TGItem> lootableItems = new List<TGItem>();

    // private
    // Capsule Colider ref
    private CapsuleCollider col;
    private Rigidbody       rb;

    //private
    TGEventManager  eventManager;
    MWeaponStats    weaponStats;
    

    //Unity lifecycle
    protected override void ChildAwake()
    {
        characterStat = new MCharacterStats();
    }

    private void Start()
    {
        eventManager = TGEventManager.Instance;
    }

    void OnTriggerEnter(Collider other) // Ʈ���ſ� �浹�� �� ����Ǵ� �޼ҵ�
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //�ش� ���Ⱑ ������ �ִ��� Ȯ��
            {
                lootableItems.Add(itemObject);
                Debug.Log("enter to " + itemObject.objectName);
            }
        }
    }

    private void OnTriggerExit(Collider other) //Ʈ���ſ��� �������� �� ����Ǵ� �޼ҵ�
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //�ش� ���Ⱑ ������ �ִ��� Ȯ��
            {
                lootableItems.Remove(itemObject.GetComponent<TGItem>());
                Debug.Log("exit from " + itemObject.objectName);
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