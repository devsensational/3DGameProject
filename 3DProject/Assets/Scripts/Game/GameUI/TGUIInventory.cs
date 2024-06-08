using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector
    public GameObject content;  // Content 객체를 할당
    public GameObject uiPrefab; // 생성할 UI 프리팹을 할당

    //private
    //References
    Dictionary<EKeyValues, KeyCode>  keyValuePairs;      
    TGObjectPoolManager             poolManager;       
    TGEventManager                  eventManager;      

    bool isEnable = false;  // UI Gameobject를 직접 제어하지 않기 위해 생성한 bool변수 

    // Unity lifecycle
    void Start()
    {
        InitReferences();
        InitEvent();

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
    }

    void InitEvent()
    {

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
    void CreateUIElement(string itemName)
    {
        GameObject newUIElement = Instantiate(uiPrefab, content.transform);
    }
}
