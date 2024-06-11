using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector    
    public GameObject LootableContent;          // Lootable list content 객체를 할당
    public GameObject InventoryContent;         // inventory Content 객체를 할당
    public GameObject PrimaryWeaponContent;     //
    public GameObject SecondaryWeaponContent;   //

    public GameObject UIPrefab; // 생성할 UI 프리팹을 할당
    public Dictionary<EItemType, GameObject> ContentDictionary = new Dictionary<EItemType, GameObject>(); // 콘텐츠UI에 아이템 타입을 할당하여 자동으로 부모가 설정되도록 하는 딕셔너리

    //private
    //References
    Dictionary<EKeyValues, KeyCode> keyValuePairs;      
    TGObjectPoolManager             poolManager;       
    TGEventManager                  eventManager;

    Dictionary<TGItem, TGUILootableItemInterective> lootableItemUIDictionary    = new Dictionary<TGItem, TGUILootableItemInterective>(); //땅에 드랍되어있는 아이템 목록을 위한 딕셔너리
    Dictionary<TGItem, TGUILootableItemInterective> inventoryUIDictionary       = new Dictionary<TGItem, TGUILootableItemInterective>(); //인벤토리에 들어있는 아이템 목록을 위한 딕셔너리

    bool isEnable = false;  // UI Gameobject를 직접 제어하지 않기 위해 생성한 bool변수 

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

    //키 컨트롤
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

    // 루팅가능한 아이템 버튼 생성 메소드
    void CreateUILootableButton(object parameters)
    {
        TGItem ptrItem = (TGItem)parameters;
        TGUILootableItemInterective ptrUI = poolManager.GetTGObject(ETGObjectType.UILootableItemButton).GetComponent<TGUILootableItemInterective>(); //오브젝트 풀로 부터 꺼내옴
        
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

    // 루팅가능한 아이템 버튼 제거 메소드
    void RemoveUILootableButton(object parameters)
    {
        TGItem ptrItem = (TGItem)parameters;
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];

        lootableItemUIDictionary.Remove(ptrItem);
        poolManager.ReleaseTGObject(ETGObjectType.UILootableItemButton, ptrUI.gameObject); //오브젝트 풀로 반환

    }

    // 아이템 루팅 시 상호작용
    void CreateUIInventoryItemButton(object parameters)
    {
        TGUILootableItemInterective ptrUI = (TGUILootableItemInterective)parameters;

        //lootableItemUIDictionary.Remove(ptrUI.IntertectedItem);
        ptrUI.transform.SetParent(ContentDictionary[ptrUI.IntertectedItem.itemType].transform); //딕셔너리에 저장된 콘텐츠UI가 자동으로 부모가 됨
    }

    // 아이템 드랍 시 상호작용
    void RemoveUIInventoryItemButton(object parameters)
    {
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[(TGItem)parameters];

        poolManager.ReleaseTGObject(ETGObjectType.UILootableItemButton, ptrUI.gameObject); //오브젝트 풀로 반환

        //lootableItemUIDictionary.Add(ptrUI.IntertectedItem, ptrUI);
        //ptrUI.transform.SetParent(LootableContent.transform);
    }
}
