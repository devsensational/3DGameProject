using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector    
    public GameObject UIHideSpace;              // UI�� ������ ������ ����Ʈ�� �Ҵ�
    public GameObject LootableContent;          // ���� ������ ��Ͽ� ���� Content�� �Ҵ�
    public GameObject InventoryContent;         // ������ �� �κ��丮 ��Ͽ� ���� Content�� �Ҵ�
    public GameObject PrimaryWeaponContent;     // �ֹ��� Content�� �Ҵ�
    public GameObject SecondaryWeaponContent;   // �������� Content�� �Ҵ� 
    //�߰����� ������ ��� �� Inspector�� �߰��ϰ� "InitUIType" �޼ҵ忡 Key�� �߰��� ��

    public GameObject UIPrefab; // ������ UI �������� �Ҵ�(��ư)
    public Dictionary<EEquipmentType, GameObject> ContentDictionary = new Dictionary<EEquipmentType, GameObject>(); // ������UI�� ������ Ÿ���� �Ҵ��Ͽ� �ڵ����� �θ� �����ǵ��� �ϴ� ��ųʸ�

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

    private void OnDestroy()
    {
        
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
        //eventManager.StartListening(EEventType.UICreateItemButton, CreateUILootableButton);
        eventManager.StartListening(EEventType.UIEnterInteractiveItem, OnInterectLootableItem);
        eventManager.StartListening(EEventType.UIExitInteractiveItem, OnUIMoveToHideSpce);
        eventManager.StartListening(EEventType.UIPickedupItemToInventory, OnUIPickedUpItem);
        eventManager.StartListening(EEventType.UIDropItemFromInventory, OnUIMoveToHideSpce);
    }

    void InitObjectPool()
    {
        poolManager.CreateTGObjectPool(ETGObjectType.UILootableItemButton, UIPrefab);
    }

    void InitUIType()
    {
        ContentDictionary.Add(EEquipmentType.PrimaryWeapon,      PrimaryWeaponContent);
        ContentDictionary.Add(EEquipmentType.SecondaryWeapon,    SecondaryWeaponContent);
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
    void CreateUILootableButton(TGItem ptrItem)
    {
        TGUILootableItemInterective ptrUI = poolManager.GetTGObject(ETGObjectType.UILootableItemButton).GetComponent<TGUILootableItemInterective>(); //������Ʈ Ǯ�� ���� ������

        if (!lootableItemUIDictionary.ContainsKey(ptrItem))
        {
            lootableItemUIDictionary.Add(ptrItem, ptrUI);
        }
        else
        {
            lootableItemUIDictionary[ptrItem] = ptrUI;
        }
        ptrUI.transform.SetParent(UIHideSpace.transform);
        ptrUI.SetButton(ptrItem);
    }

    // ������ �ִ� �����۰� ������ �������� �� ����Ǵ� �޼ҵ�, ��ư�� ���ð����� ������� �̵���Ŵ
    void OnInterectLootableItem(object parameter)
    {
        TGItem ptrItem = (TGItem)parameter;
        if (!lootableItemUIDictionary.ContainsKey(ptrItem))
        {
            CreateUILootableButton(ptrItem);
        }
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];
        ptrUI.transform.SetParent(LootableContent.transform);
    }

    // ��ư�� Ÿ�Կ� �´� ����Ʈ�� �̵���Ű�� �޼ҵ�
    void OnUIPickedUpItem(object parameter)
    {
        TGItem ptrItem = (TGItem)parameter;
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];

        if(ptrItem.equipmentType != EEquipmentType.None)
        {
            ptrUI.transform.SetParent(ContentDictionary[ptrItem.equipmentType].transform);
        }
        else
        {
            ptrUI.transform.SetParent(InventoryContent.transform);
        }
    }

    // ��ư�� Hide Space�� �̵���Ű�� �޼ҵ�, �����۰� �־����ų� �������� ���� �� ȣ��
    void OnUIMoveToHideSpce(object parameter)
    {
        TGItem ptrItem = (TGItem)parameter;
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];
        
        ptrUI.ResetButton();
        ptrUI.transform.SetParent(UIHideSpace.transform);
    }
}
