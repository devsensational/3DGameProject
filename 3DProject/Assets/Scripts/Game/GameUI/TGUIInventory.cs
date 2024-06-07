using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TGUIInventory : MonoBehaviour
{
    Dictionary<KeyValues, KeyCode>  keyValuePairs;      // KeyValuePair dictionary ref
    TGEventManager                  eventManager;       // Event manager ref

    bool isEnable = false;

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
        if (Input.GetKeyDown(keyValuePairs[KeyValues.ToggleInventoryUI]))
        {
            OnToggleUI(null);
            eventManager.TriggerEvent(EventType.ToggleInventoryUI);
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
}
