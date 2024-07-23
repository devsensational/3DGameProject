using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Start()
    {
        base.Start();
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

        if (itemPtr.equipmentType == HandInItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        DropItem(itemPtr.itemType);
    }

    public void CommandChangeInHandItem(EEquipmentType itemType) // "TGPlayerCharacterController"���� Ư�� �������� �տ� ��� ����� ������ ����
    {
        if (equipItems[itemType] == null) return;

        if(HandInItem != itemType)
        {
            ChangeInHandItem(equipItems[HandInItem], equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.ChangeHandItem, equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.UpdateItemInfo, equipItems[itemType]);
        } 
    }

    public void CommandTakeItem(object parameter)  // UI�� ���� �������� �ֿ��� �� 
    { 
        TGItem itemPtr = (TGItem)parameter;

        if (itemPtr.equipmentType == HandInItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        TakeItem(itemPtr);

    }
    public override void CommandReloadInHandItem() // �տ� ��� �ִ� ���� ������ ����
    {
        if (equipItems[HandInItem] == null) return;

        if (equipItems[HandInItem].equipmentType != EEquipmentType.None)
        {
            TGItemWeapon weaponPtr = (TGItemWeapon)equipItems[HandInItem];
            if (weaponPtr.CommandReload()) // �������� ���������� ������� �� ����
            {
                eventManager.TriggerEvent(EEventType.StartCircleTimerUI, weaponPtr.weaponStats.reloadTime);
                anim.OnReloadAnimation(null);
            }

            Debug.Log("(TGPlayerCharacter:CommandReloadInHandItem) Command reload");
        }
    }

    public void CommandUseInHandItem()
    {
        if (equipItems[HandInItem] == null) return;

        equipItems[HandInItem].UseItem();
        
    }

    public void CommandAimWeaponItem()
    {
        if (equipItems[HandInItem] == null) return;

        equipItems[HandInItem].EnableAim();
    }

    public void CommandDisableAimWeaponItem()
    {
        if (equipItems[HandInItem] == null) return;

        equipItems[HandInItem].DisableAim();
    }

    public override void ReceiveDamage(float damageValue)
    {
        base.ReceiveDamage(damageValue);
        eventManager.TriggerEvent(EEventType.UIUpdateHPBar, Mathf.Clamp01(currentHP / characterStat.maxHp));
    }

    public override void OnReloadComplete()
    {
        TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, equipItems[HandInItem]);
    }
}