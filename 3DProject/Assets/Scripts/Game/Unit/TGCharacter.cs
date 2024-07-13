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

    //protected
    protected TGEventManager eventManager;  //�̺�Ʈ�Ŵ���
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
    //Init
    protected virtual void InitReferences()
    {
        eventManager = TGEventManager.Instance;
        characterStat = new MCharacterStats();
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

        if (ptrItem.equipmentType != EEquipmentType.None) //��� �������� ��� �������� ����
        {
            equipItems[ptrItem.equipmentType] = null;
            if (HandInItem == ptrItem.equipmentType)
            {
                HandInItem = EEquipmentType.Default; // �տ� ��� �ִ� ����� ��� handInItem ����
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
                Debug.Log($"(TGCharacter:ChangeInHandItem) Weapon switch");
            }
        }
    }

    public void ReceiveDamage(float damageValue)
    {
        characterStat.currentHp -= damageValue;

        Debug.Log($"(TGCharacter:ReceiveDamage) {objectName} received {damageValue} damage!");
    }

    // getter/setter
    public EEquipmentType GetInHandItem()
    {
        return HandInItem;
    }

    // child
    protected virtual void ChildAwake() {}
    protected virtual void ChildOnDestroy() { }
}
