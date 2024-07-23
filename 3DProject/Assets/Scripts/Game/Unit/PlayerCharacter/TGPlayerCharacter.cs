using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Start()
    {
        base.Start();
        eventManager = TGEventManager.Instance;
    }

    private void Update()
    {

    }

    void OnTriggerEnter(Collider other) // 트리거와 충돌할 때 실행되는 메소드
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //해당 무기가 떨어져 있는지 확인
            {
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
    public void CommandDropItem(object parameter) // "TGPlayerCharacterController"에서 Item 드랍을 호출했을 때 실행
    {
        TGItem itemPtr = (TGItem)parameter;

        if (itemPtr.equipmentType == HandInItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        DropItem(itemPtr.itemType);
    }

    public void CommandChangeInHandItem(EEquipmentType itemType) // "TGPlayerCharacterController"에서 특정 아이템을 손에 드는 명령을 내릴때 수행
    {
        if (equipItems[itemType] == null) return;

        if(HandInItem != itemType)
        {
            ChangeInHandItem(equipItems[HandInItem], equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.ChangeHandItem, equipItems[itemType]);
            eventManager.TriggerEvent(EEventType.UpdateItemInfo, equipItems[itemType]);
        } 
    }

    public void CommandTakeItem(object parameter)  // UI를 통해 아이템을 주웠을 때 
    { 
        TGItem itemPtr = (TGItem)parameter;

        if (itemPtr.equipmentType == HandInItem)
        {
            eventManager.TriggerEvent(EEventType.RemoveItemInfoUIText, null);
        }

        TakeItem(itemPtr);

    }
    public override void CommandReloadInHandItem() // 손에 들고 있는 무기 재장전 수행
    {
        if (equipItems[HandInItem] == null) return;

        if (equipItems[HandInItem].equipmentType != EEquipmentType.None)
        {
            TGItemWeapon weaponPtr = (TGItemWeapon)equipItems[HandInItem];
            if (weaponPtr.CommandReload()) // 재장전이 성공적으로 실행됐을 때 수행
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