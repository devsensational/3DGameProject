using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector
    public GameObject Content;  // Content ��ü�� �Ҵ�
    public GameObject UIPrefab; // ������ UI �������� �Ҵ�

    //private
    //References
    Dictionary<EKeyValues, KeyCode> keyValuePairs;      
    TGObjectPoolManager             poolManager;       
    TGEventManager                  eventManager;

    Dictionary<TGItem, TGUILootableItemInterective> lootableItemUIDictionary = new Dictionary<TGItem, TGUILootableItemInterective>();

    bool isEnable = false;  // UI Gameobject�� ���� �������� �ʱ� ���� ������ bool���� 

    // Unity lifecycle
    void Start()
    {
        InitReferences();
        InitEvent();
        InitObjectPool();
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
        eventManager.StartListening(EEventType.EnterInteractiveItem, CreateUILootableButton);
        eventManager.StartListening(EEventType.ExitInteractiveItem, RemoveUILootableButton);
    }

    void InitObjectPool()
    {
        poolManager.CreateTGObjectPool(ETGObjectType.UILootableItemButton, UIPrefab);
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
        TGUILootableItemInterective ptrUI = poolManager.GetTGObject(ETGObjectType.UILootableItemButton).GetComponent<TGUILootableItemInterective>();
        lootableItemUIDictionary.Add(ptrItem, ptrUI);
        ptrUI.transform.parent = Content.transform;
        ptrUI.SetButton(ptrItem);

    }

    void RemoveUILootableButton(object parameters)
    {
        TGItem ptrItem = (TGItem)parameters;
        TGUILootableItemInterective ptrUI = lootableItemUIDictionary[ptrItem];
        lootableItemUIDictionary.Remove(ptrItem);
        poolManager.ReleaseTGObject(ETGObjectType.UILootableItemButton, ptrUI.gameObject);        

    }
}
