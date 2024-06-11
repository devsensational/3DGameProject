using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
    MWeaponStats    weaponStats;

    //Unity lifecycle
    protected override void ChildAwake()
    {
        InitReferences();
    }

    protected override void ChildOnDestroy()
    {
        eventManager.StopListening(EEventType.DropItemFromInventory, CommandDropItem);
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
                eventManager.TriggerEvent(EEventType.UIEnterInteractiveItem, itemObject);
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
                eventManager.TriggerEvent(EEventType.UIExitInteractiveItem, itemObject);
                Debug.Log("exit from " + itemObject.objectName);
            }
        }
    }

    //Init
    protected override void InitReferences()
    {
        base.InitReferences();

        characterStat   = new MCharacterStats();
        eventManager    = TGEventManager.Instance;
    }

    protected override void InitEvent()
    {
        base.InitEvent();

        eventManager.StartListening(EEventType.DropItemFromInventory, CommandDropItem);
        eventManager.StartListening(EEventType.PickedupItemToInventory, CommandTakeItem);
    }

    //item ���� method
    // "TGPlayerCharacterController"���� Item ����� ȣ������ �� ����
    public void CommandDropItem(object parameter) 
    {
        TGItem parameterObj = (TGItem)parameter;

        DropItem(parameterObj.itemType);
    }

    // "TGPlayerCharacterController"���� Ư�� �������� �տ� ��� ����� ������ ����
    public void CommandHandInItem(EItemType itemType) 
    {
        if (equipItems[itemType] == null) return;

        if(handInItem != itemType)
        {
            ChangeHandInItem(equipItems[handInItem], equipItems[itemType]);
        } 
    }


    public void CommandTakeItem(object parameter) 
    { 
        TGItem item = (TGItem)parameter;
        TakeItem(item);
    }
    //���� 
}