using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUIMouseCursorController : MonoBehaviour
{
    private Dictionary<KeyValues, KeyCode> keyValuePairs;           // KeyValuePair map ref

    private bool isMouseCursorLock = false;
    void Start()
    {
        keyValuePairs = TGPlayerKeyManager.Instance.KeyValuePairs;
        MouseCursorLockSwitch();
    }

    void Update()
    {
        if (Input.GetKeyDown(keyValuePairs[KeyValues.MOUSECURSORSWITCH])) //MOUSECURSORSWITCH에 할당 된 키가 입력되면 호출
        {
            MouseCursorLockSwitch();
        }
    }

    // 마우스 커서 On/Off
    void MouseCursorLockSwitch()
    {
        if(!isMouseCursorLock)
        {
            Cursor.visible = false;                         //마우스 커서가 보이지 않게 함
            Cursor.lockState = CursorLockMode.Locked;       //마우스 커서를 고정시킴
            isMouseCursorLock = true;
            Debug.Log("Mouse cursor lock");
        }
        else
        { 
            Cursor.visible = true;                          //마우스 커서가 보이게 않게 함
            Cursor.lockState = CursorLockMode.Confined;     //마우스 커서 고정 해제시킴
            isMouseCursorLock = false;
            Debug.Log("Mouse cursor unlock");
        }
    }
}
