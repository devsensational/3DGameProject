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
    //public List<TGItem> lootableItems = new List<TGItem>();

    // private
    // Capsule Colider ref
     CapsuleCollider col;
     Rigidbody       rb;

    //References
    TGEventManager  eventManager;
    MWeaponStats    weaponStats;

    //Unity lifecycle
    protected override void ChildAwake()
    {
        InitReferences();
        InitEvent();
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
                //lootableItems.Add(itemObject);
                eventManager.TriggerEvent(EEventType.EnterInteractiveItem, itemObject);
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
                //lootableItems.Remove(itemObject.GetComponent<TGItem>());
                eventManager.TriggerEvent(EEventType.ExitInteractiveItem, itemObject);
                Debug.Log("exit from " + itemObject.objectName);
            }
        }
    }

    //Init
    void InitReferences()
    {
        characterStat   = new MCharacterStats();
        eventManager    = TGEventManager.Instance;
    }

    void InitEvent()
    {
       
    }

    //item ���� method
    // "TGPlayerCharacterController"���� Item ����� ȣ������ �� ����
    public void CommandDropItem(EItemType itemType) 
    {
        equipItems[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    // "TGPlayerCharacterController"���� Ư�� �������� �տ� ��� ����� ������ ����
    public void CommandHandInItem(EItemType itemType) 
    {
        if(handInItem != null)
        {
            ChangeHandInItem(handInItem, equipItems[itemType]);
        } 
        else
        {

        }
    }

    //�տ� �� �������� ��ü�� �� ����ϴ� �޼ҵ�
    private void ChangeHandInItem(TGItem previousItem, TGItem nextItem)
    {
        if (previousItem != null)
        {
            previousItem.enabled = false;
        }
        nextItem.enabled = true;
    }

    //���� 
}