using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// 현 프로젝트의 유닛을 정의하기 위한 클래스입니다.
public class TGCharacter : TGObject
{
    //Inspector
    public GameObject HandPosition;   //아이템을 주웠을 때 위치를 결정할 GameObject

    //public
    public MCharacterStats characterStat { get; protected set; } // 플레이어 캐릭터 스탯
    public Dictionary<EEquipmentType, TGItem> equipItems = new Dictionary<EEquipmentType, TGItem>();
    public Dictionary<EItemType, TGItem> inventory = new Dictionary<EItemType, TGItem>(); // 주운 아이템 리스트
    public EEquipmentType HandInItem = EEquipmentType.Default;   //플레이어 캐릭터가 들고 있는 아이템 type

    public float currentHP = 0f;
    public float currnetSpeed = 0f;

    //protected
    protected TGEventManager          eventManager;  //이벤트매니저
    protected TGCharacterAnimation    anim;

    //private
    

    //Unity lifecycle
    private void Awake()
    {
        equipItems.Add(EEquipmentType.None, null);
        equipItems.Add(EEquipmentType.Default, null);
        equipItems.Add(EEquipmentType.PrimaryWeapon, null);
        equipItems.Add(EEquipmentType.SecondaryWeapon, null);

        ChildAwake();

        InitReferences();
        InitEvent();
    }

    private void OnDestroy()
    {
        ChildOnDestroy();   
    }

    protected virtual void Start()
    {

    }

    //Init
    protected virtual void InitReferences()
    {
        eventManager    = TGEventManager.Instance;
        characterStat   = new MCharacterStats();
        currentHP       = characterStat.maxHp;
        anim            = GetComponent<TGCharacterAnimation>();
    }

    protected virtual void InitEvent()
    {

    }

    // item 상호작용 관련 메소드
    protected void TakeItem(TGItem item)    // item 습득 시도 메소드
    {
        if (item == null) return;                        // 선택된 item object가 null일 경우 메소드 종료

        if (item.equipmentType != EEquipmentType.None)  // 장비 타입일 경우
        {
            if (equipItems[item.equipmentType] != null)
            {
                eventManager.TriggerEvent(EEventType.UIDropItemFromInventory, equipItems[item.equipmentType]);
                DropItem(equipItems[item.equipmentType].itemType);
            }
            equipItems[item.equipmentType] = item;
            //equipItems[item.equipmentType].OnPickedUpThisItem(gameObject);        // Item이 습득 됐을 때 item instance가 실행되야 할 명령 수행
        }

        if (inventory.ContainsKey(item.itemType) && inventory[item.itemType] != null)
        {
            inventory[item.itemType].itemCount += item.itemCount; //같은 종류의 아이템이 이미 존재하면 수량 증가
            inventory[item.itemType].UpdateButtonUI();
            Destroy(item.gameObject);
            return;
        }

        if (!inventory.ContainsKey(item.itemType)) // 인벤토리 딕셔너리에 키가 없을 시 생성
        {
            inventory.Add(item.itemType, item);
            Debug.Log("(TGCharacter:TakeItem) Inventroy dictionary added: " + item.itemType);
        }

        inventory[item.itemType] = item;
        item.OnPickedUpThisItem(gameObject);        // Item이 습득 됐을 때 item instance가 실행되야 할 명령 수행

        Debug.Log("(TGCharacter:TakeItem) " + gameObject.name + " picked up " + item.name);
    }

    protected void DropItem(EItemType itemType)  // item 드랍 시도 메소드
    {
        TGItem ptrItem = inventory[itemType];

        if (ptrItem == null) return;
        if (ptrItem.equipmentType != EEquipmentType.None) //장비 아이템일 경우 장비아이템 해제
        {
            equipItems[ptrItem.equipmentType] = null;
            if (HandInItem == ptrItem.equipmentType)
            {
                HandInItem = EEquipmentType.Default; // 손에 들고 있는 장비일 경우 handInItem 해제
                anim.DisableUpperBody();
            }
        }

        inventory[itemType] = null;

        ptrItem.OnDropThisItem();
    }
    
    public void CommandMove(Vector3 direction, float moveSpeed) // 이동 관련 메소드
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    
    protected void ChangeInHandItem(TGItem previousItem, TGItem nextItem) //손에 든 아이템을 교체할 때 사용하는 메소드
    {
        if (previousItem != null)
        {
            previousItem.OnHandInThisItem();
        }
        if (nextItem != null)
        {
            nextItem.OnHandInThisItem();
            HandInItem = nextItem.equipmentType;

            TGItemWeapon weaponPtr = (TGItemWeapon)equipItems[HandInItem];
            if (weaponPtr != null)
            {
                characterStat.weaponStats = weaponPtr.weaponStats; //
                anim.EnableUpperBody();
                Debug.Log($"(TGCharacter:ChangeInHandItem) Weapon switch");
            }
        }
    }

    public virtual void CommandReloadInHandItem() // 손에 들고 있는 무기 재장전 수행
    {
        if (equipItems[HandInItem] == null) return;

        if (equipItems[HandInItem].equipmentType != EEquipmentType.None)
        {
            TGItemWeapon weaponPtr = (TGItemWeapon)equipItems[HandInItem];
            if (weaponPtr.CommandReload()) // 재장전이 성공적으로 실행됐을 때 수행
            {
                anim.OnReloadAnimation(null);
            }

            Debug.Log("(TGPlayerCharacter:CommandReloadInHandItem) Command reload");
        }
    }

    public virtual void OnReloadComplete()
    {

    }

    public virtual void OnFire()
    {
        anim.OnFireAnimation(null);
    }

    public virtual void ReceiveDamage(float damageValue) // 데미지 리시브
    {
        currentHP -= damageValue;
        Debug.Log($"(TGCharacter:ReceiveDamage) {objectName} received {damageValue} damage!");

        if( currentHP <= 0 )
        {
            OnDeadCharacter(); //체력이 0 이하면 캐릭터 사망 처리
        }
    }

    protected virtual void OnDeadCharacter() // 캐릭터가 사망 처리 될때 실행되는 메소드
    {
        foreach (EItemType itemType in Enum.GetValues(typeof(EItemType)))
        {
            if (inventory.ContainsKey(itemType))
            {
                DropItem(itemType);
            }
        }

        if (anim != null)
        {
            anim.OnDeadAnimation(null);
        }

    }

    // getter/setter
    public EEquipmentType GetInHandItem()
    {
        return HandInItem;
    }

    public TGItemWeapon GetInHandWeapon()
    {
        return (TGItemWeapon)equipItems[HandInItem];
    }

    // child
    protected virtual void ChildAwake() {}
    protected virtual void ChildOnDestroy() { }
}
