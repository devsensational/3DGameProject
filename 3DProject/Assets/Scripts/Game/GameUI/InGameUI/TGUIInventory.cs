using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector    
    public GameObject UIHideSpace;              // UI가 숨겨질 공간의 리스트를 할당
    public GameObject LootableContent;          // 루팅 가능한 목록에 대한 Content를 할당
    public GameObject InventoryContent;         // 루팅이 된 인벤토리 목록에 대한 Content를 할당
    public GameObject PrimaryWeaponContent;     // 주무기 Content를 할당
    public GameObject SecondaryWeaponContent;   // 보조무기 Content를 할당 
    //추가적인 슬롯의 경우 위 Inspector에 추가하고 "InitUIType" 메소드에 Key를 추가할 것

    public GameObject UIPrefab; // 생성할 UI 프리팹을 할당(버튼)
    public Dictionary<EEquipmentType, GameObject> ContentDictionary = new Dictionary<EEquipmentType, GameObject>(); // 콘텐츠UI에 아이템 타입을 할당하여 자동으로 부모가 설정되도록 하는 딕셔너리

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
    void CreateUILootableButton(TGItem ptrItem)
    {
        TGUILootableItemInterective ptrUI = poolManager.GetTGObject(ETGObjectType.UILootableItemButton).GetComponent<TGUILootableItemInterective>(); //오브젝트 풀로 부터 꺼내옴

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

    // 떨어져 있는 아이템과 가까이 접근했을 때 실행되는 메소드, 버튼을 루팅가능한 목록으로 이동시킴
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

    // 버튼을 타입에 맞는 리스트로 이동시키는 메소드
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

    // 버튼을 Hide Space로 이동시키는 메소드, 아이템과 멀어지거나 아이템을 버릴 때 호출
    void OnUIMoveToHideSpce(object parameter)
    {
        TGItem ptrItem = (TGItem)parameter;
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];
        
        ptrUI.ResetButton();
        ptrUI.transform.SetParent(UIHideSpace.transform);
    }
}
