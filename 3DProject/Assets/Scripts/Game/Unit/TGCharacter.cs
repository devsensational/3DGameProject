using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// �� ������Ʈ�� ������ �����ϱ� ���� Ŭ�����Դϴ�.
public class TGCharacter : TGObject
{
    //Inspector
    public GameObject HandPosition;   //�������� �ֿ��� �� ��ġ�� ������ GameObject

    //public
    public MCharacterStats characterStat { get; protected set; } // �÷��̾� ĳ���� ����
    public Dictionary<EEquipmentType, TGItem> equipItems = new Dictionary<EEquipmentType, TGItem>();
    public Dictionary<EItemType, TGItem> inventory = new Dictionary<EItemType, TGItem>(); // �ֿ� ������ ����Ʈ
    public EEquipmentType HandInItem = EEquipmentType.Default;   //�÷��̾� ĳ���Ͱ� ��� �ִ� ������ type

    public float currentHP = 0f;
    public float currnetSpeed = 0f;

    //protected
    protected TGEventManager          eventManager;  //�̺�Ʈ�Ŵ���
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

    // item ��ȣ�ۿ� ���� �޼ҵ�
    protected void TakeItem(TGItem item)    // item ���� �õ� �޼ҵ�
    {
        if (item == null) return;                        // ���õ� item object�� null�� ��� �޼ҵ� ����

        if (item.equipmentType != EEquipmentType.None)  // ��� Ÿ���� ���
        {
            if (equipItems[item.equipmentType] != null)
            {
                eventManager.TriggerEvent(EEventType.UIDropItemFromInventory, equipItems[item.equipmentType]);
                DropItem(equipItems[item.equipmentType].itemType);
            }
            equipItems[item.equipmentType] = item;
            //equipItems[item.equipmentType].OnPickedUpThisItem(gameObject);        // Item�� ���� ���� �� item instance�� ����Ǿ� �� ��� ����
        }

        if (inventory.ContainsKey(item.itemType) && inventory[item.itemType] != null)
        {
            inventory[item.itemType].itemCount += item.itemCount; //���� ������ �������� �̹� �����ϸ� ���� ����
            inventory[item.itemType].UpdateButtonUI();
            Destroy(item.gameObject);
            return;
        }

        if (!inventory.ContainsKey(item.itemType)) // �κ��丮 ��ųʸ��� Ű�� ���� �� ����
        {
            inventory.Add(item.itemType, item);
            Debug.Log("(TGCharacter:TakeItem) Inventroy dictionary added: " + item.itemType);
        }

        inventory[item.itemType] = item;
        item.OnPickedUpThisItem(gameObject);        // Item�� ���� ���� �� item instance�� ����Ǿ� �� ��� ����

        Debug.Log("(TGCharacter:TakeItem) " + gameObject.name + " picked up " + item.name);
    }

    protected void DropItem(EItemType itemType)  // item ��� �õ� �޼ҵ�
    {
        TGItem ptrItem = inventory[itemType];

        if (ptrItem == null) return;
        if (ptrItem.equipmentType != EEquipmentType.None) //��� �������� ��� �������� ����
        {
            equipItems[ptrItem.equipmentType] = null;
            if (HandInItem == ptrItem.equipmentType)
            {
                HandInItem = EEquipmentType.Default; // �տ� ��� �ִ� ����� ��� handInItem ����
                anim.DisableUpperBody();
            }
        }

        inventory[itemType] = null;

        ptrItem.OnDropThisItem();
    }
    
    public void CommandMove(Vector3 direction, float moveSpeed) // �̵� ���� �޼ҵ�
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    
    protected void ChangeInHandItem(TGItem previousItem, TGItem nextItem) //�տ� �� �������� ��ü�� �� ����ϴ� �޼ҵ�
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

    public virtual void CommandReloadInHandItem() // �տ� ��� �ִ� ���� ������ ����
    {
        if (equipItems[HandInItem] == null) return;

        if (equipItems[HandInItem].equipmentType != EEquipmentType.None)
        {
            TGItemWeapon weaponPtr = (TGItemWeapon)equipItems[HandInItem];
            if (weaponPtr.CommandReload()) // �������� ���������� ������� �� ����
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

    public virtual void ReceiveDamage(float damageValue) // ������ ���ú�
    {
        currentHP -= damageValue;
        Debug.Log($"(TGCharacter:ReceiveDamage) {objectName} received {damageValue} damage!");

        if( currentHP <= 0 )
        {
            OnDeadCharacter(); //ü���� 0 ���ϸ� ĳ���� ��� ó��
        }
    }

    protected virtual void OnDeadCharacter() // ĳ���Ͱ� ��� ó�� �ɶ� ����Ǵ� �޼ҵ�
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
