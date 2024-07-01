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

    void Start()
    {
        eventManager = TGEventManager.Instance;
    }

    private void Update()
    {

    }

    void OnTriggerEnter(Collider other) // Ʈ���ſ� �浹�� �� ����Ǵ� �޼ҵ�
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //�ش� ���Ⱑ ������ �ִ��� Ȯ��
            {
                eventManager.TriggerEvent(EEventType.UIEnterInteractiveItem, itemObject);
                Debug.Log("(TGPlayerCharacter:OnTriggerEnter) enter to " + itemObject.objectName);
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
                eventManager.TriggerEvent(EEventType.UIExitInteractiveItem, itemObject);
                Debug.Log("(TGPlayerCharacter:OnTriggerExit) exit from " + itemObject.objectName);
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
    public void CommandDropItem(object parameter) // "TGPlayerCharacterController"���� Item ����� ȣ������ �� ����
    {
        TGItem itemPtr = (TGItem)parameter;

        if (itemPtr.equipmentType == inHandItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        DropItem(itemPtr.itemType);
    }

    public void CommandChangeInHandItem(EEquipmentType itemType) // "TGPlayerCharacterController"���� Ư�� �������� �տ� ��� ����� ������ ����
    {
        if (equipItems[itemType] == null) return;

        if(inHandItem != itemType)
        {
            ChangeInHandItem(equipItems[inHandItem], equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.ChangeHandItem, equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.UpdateItemInfo, equipItems[itemType]);
        } 
    }

    public void CommandTakeItem(object parameter)  // UI�� ���� �������� �ֿ��� �� 
    { 
        TGItem itemPtr = (TGItem)parameter;

        if (itemPtr.equipmentType == inHandItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        TakeItem(itemPtr);

    }

    public void CommandUseInHandItem()
    {
        if (equipItems[inHandItem] == null) return;

        equipItems[inHandItem].UseItem();
    }

    public void CommandReloadInHandItem()
    {
        if (equipItems[inHandItem] == null) return;

        if (equipItems[inHandItem].equipmentType != EEquipmentType.None)
        {
            TGItemWeapon weaponPtr = (TGItemWeapon)equipItems[inHandItem];
            if (weaponPtr.CommandReload()) // �������� ���������� ������� �� ����
            {
                eventManager.TriggerEvent(EEventType.StartCircleTimerUI, weaponPtr.weaponStats.reloadTime);
            }

            Debug.Log("(TGPlayerCharacter:CommandReloadInHandItem) Command reload");
        }
    }

}