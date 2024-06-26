using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

// 플레이어가 직접 조종하는 캐릭터의 스탯 및 상호작용을 위한 클래스입니다
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

    void OnTriggerEnter(Collider other) // 트리거와 충돌할 때 실행되는 메소드
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //해당 무기가 떨어져 있는지 확인
            {
                //lootableItems.Add(itemObject);
                eventManager.TriggerEvent(EEventType.UIEnterInteractiveItem, itemObject);
                Debug.Log("(TGPlayerCharacter:OnTriggerEnter) enter to " + itemObject.objectName);
            }
        }
    }

    private void OnTriggerExit(Collider other) //트리거에서 빠져나올 때 실행되는 메소드
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //해당 무기가 떨어져 있는지 확인
            {
                //lootableItems.Remove(itemObject.GetComponent<TGItem>());
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

    //item 관련 method
    // "TGPlayerCharacterController"에서 Item 드랍을 호출했을 때 실행
    public void CommandDropItem(object parameter) 
    {
        TGItem itemPtr = (TGItem)parameter;

        if (itemPtr.equipmentType == inHandItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        DropItem(itemPtr.itemType);
    }

    // "TGPlayerCharacterController"에서 특정 아이템을 손에 드는 명령을 내릴때 수행
    public void CommandChangeInHandItem(EEquipmentType itemType) 
    {
        if (equipItems[itemType] == null) return;

        if(inHandItem != itemType)
        {
            ChangeInHandItem(equipItems[inHandItem], equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.ChangeHandItem, equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.UpdateItemInfo, equipItems[itemType]);
        } 
    }

    // UI를 통해 아이템을 주웠을 때 
    public void CommandTakeItem(object parameter) 
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
            weaponPtr.CommandReload();
            eventManager.TriggerEvent(EEventType.StartCircleTimerUI, weaponPtr.weaponStats.reloadTime);

            Debug.Log("(TGPlayerCharacter:CommandReloadInHandItem) Command reload");
        }
    }

}