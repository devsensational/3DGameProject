using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUIInventory : MonoBehaviour
{
    // Inspector
    public GameObject content;  // Content ��ü�� �Ҵ�
    public GameObject uiPrefab; // ������ UI �������� �Ҵ�

    //private
    //References
    Dictionary<EKeyValues, KeyCode>  keyValuePairs;      
    TGObjectPoolManager             poolManager;       
    TGEventManager                  eventManager;      

    bool isEnable = false;  // UI Gameobject�� ���� �������� �ʱ� ���� ������ bool���� 

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
    void CreateUIElement(string itemName)
    {
        GameObject newUIElement = Instantiate(uiPrefab, content.transform);
    }
}
