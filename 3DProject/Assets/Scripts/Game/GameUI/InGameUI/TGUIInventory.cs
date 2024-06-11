using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector    
    public GameObject LootableContent;          // Lootable list content ��ü�� �Ҵ�
    public GameObject InventoryContent;         // inventory Content ��ü�� �Ҵ�
    public GameObject PrimaryWeaponContent;     //
    public GameObject SecondaryWeaponContent;   //

    public GameObject UIPrefab; // ������ UI �������� �Ҵ�
    public Dictionary<EItemType, GameObject> ContentDictionary = new Dictionary<EItemType, GameObject>(); // ������UI�� ������ Ÿ���� �Ҵ��Ͽ� �ڵ����� �θ� �����ǵ��� �ϴ� ��ųʸ�

    //private
    //References
    Dictionary<EKeyValues, KeyCode> keyValuePairs;      
    TGObjectPoolManager             poolManager;       
    TGEventManager                  eventManager;

    Dictionary<TGItem, TGUILootableItemInterective> lootableItemUIDictionary    = new Dictionary<TGItem, TGUILootableItemInterective>(); //���� ����Ǿ��ִ� ������ ����� ���� ��ųʸ�
    Dictionary<TGItem, TGUILootableItemInterective> inventoryUIDictionary       = new Dictionary<TGItem, TGUILootableItemInterective>(); //�κ��丮�� ����ִ� ������ ����� ���� ��ųʸ�

    bool isEnable = false;  // UI Gameobject�� ���� �������� �ʱ� ���� ������ bool���� 

    // Unity lifecycle
    void Start()
    {
        InitReferences();
        InitEvent();
        InitObjectPool();
        InitUIType();
    }

    void Update()
    {
        InputKey();
    }

    // Init
    void InitReferences() 
    {
        eventManager    = TGEventManager.Instance;
        keyValuePairs   = TGPlayerKeyManager.Instance.KeyValuePairs;
        poolManager     = TGObjectPoolManager.Instance;
    }

    void InitEvent()
    {
        eventManager.StartListening(EEventType.UIEnterInteractiveItem, CreateUILootableButton);
        eventManager.StartListening(EEventType.UIExitInteractiveItem, RemoveUILootableButton);
        eventManager.StartListening(EEventType.UIPickedupItemToInventory, CreateUIInventoryItemButton);
        eventManager.StartListening(EEventType.UIDropItemFromInventory, RemoveUIInventoryItemButton);
    }

    void InitObjectPool()
    {
        poolManager.CreateTGObjectPool(ETGObjectType.UILootableItemButton, UIPrefab);
    }

    void InitUIType()
    {
        ContentDictionary.Add(EItemType.PrimaryWeapon,      PrimaryWeaponContent);
        ContentDictionary.Add(EItemType.SecondaryWeapon,    SecondaryWeaponContent);
        ContentDictionary.Add(EItemType.LootableItem,       LootableContent);
    }

    //Ű ��Ʈ��
    void InputKey()
    {
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.ToggleInventoryUI]))
        {
            OnToggleUI(null);
            eventManager.TriggerEvent(EEventType.ToggleInventoryUI);
        }
    }

    // UI Toggle
    void OnToggleUI(object parameter)
    {
        if(isEnable)
        {
            transform.localPosition = new Vector3(0, 1080, 0);
            isEnable = false;
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, 0);
            isEnable = true;
        }
    }

    // ���ð����� ������ ��ư ���� �޼ҵ�
    void CreateUILootableButton(object parameters)
    {
        TGItem ptrItem = (TGItem)parameters;
        TGUILootableItemInterective ptrUI = poolManager.GetTGObject(ETGObjectType.UILootableItemButton).GetComponent<TGUILootableItemInterective>(); //������Ʈ Ǯ�� ���� ������
        
        if (!lootableItemUIDictionary.ContainsKey(ptrItem))
        {
            lootableItemUIDictionary.Add(ptrItem, ptrUI);
        }
        else
        {
            lootableItemUIDictionary[ptrItem] = ptrUI;
        }
        ptrUI.transform.SetParent(LootableContent.transform);
        ptrUI.SetButton(ptrItem);

    }

    // ���ð����� ������ ��ư ���� �޼ҵ�
    void RemoveUILootableButton(object parameters)
    {
        TGItem ptrItem = (TGItem)parameters;
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];

        lootableItemUIDictionary.Remove(ptrItem);
        poolManager.ReleaseTGObject(ETGObjectType.UILootableItemButton, ptrUI.gameObject); //������Ʈ Ǯ�� ��ȯ

    }

    // ������ ���� �� ��ȣ�ۿ�
    void CreateUIInventoryItemButton(object parameters)
    {
        TGUILootableItemInterective ptrUI = (TGUILootableItemInterective)parameters;

        //lootableItemUIDictionary.Remove(ptrUI.IntertectedItem);
        ptrUI.transform.SetParent(ContentDictionary[ptrUI.IntertectedItem.itemType].transform); //��ųʸ��� ����� ������UI�� �ڵ����� �θ� ��
    }

    // ������ ��� �� ��ȣ�ۿ�
    void RemoveUIInventoryItemButton(object parameters)
    {
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[(TGItem)parameters];

        poolManager.ReleaseTGObject(ETGObjectType.UILootableItemButton, ptrUI.gameObject); //������Ʈ Ǯ�� ��ȯ

        //lootableItemUIDictionary.Add(ptrUI.IntertectedItem, ptrUI);
        //ptrUI.transform.SetParent(LootableContent.transform);
    }
}
