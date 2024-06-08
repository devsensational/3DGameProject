using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGPlayerKeyManager : UMonoSingleton<TGPlayerKeyManager>
{
    //public
    public Dictionary<EKeyValues, KeyCode> KeyValuePairs { get; private set; }

    protected override void ChildAwake()
    {
        DontDestroyOnLoad(this);
        Init();
    }

    protected override void ChildOnDestroy()
    {

    }

    // 초기값 셋팅
    public void Init()
    {
        KeyValuePairs = new Dictionary<EKeyValues, KeyCode>();

        KeyValuePairs.Add(EKeyValues.Forward,            KeyCode.W);
        KeyValuePairs.Add(EKeyValues.Backward,           KeyCode.S);
        KeyValuePairs.Add(EKeyValues.Left,               KeyCode.A);
        KeyValuePairs.Add(EKeyValues.Right,              KeyCode.D);
        KeyValuePairs.Add(EKeyValues.Fire,               KeyCode.Mouse0);
        KeyValuePairs.Add(EKeyValues.MouseCursorSwitch,  KeyCode.LeftControl);
        KeyValuePairs.Add(EKeyValues.Item1,              KeyCode.Alpha1);
        KeyValuePairs.Add(EKeyValues.Item2,              KeyCode.Alpha2);
        KeyValuePairs.Add(EKeyValues.Item3,              KeyCode.Alpha3);
        KeyValuePairs.Add(EKeyValues.Item4,              KeyCode.Alpha4);
        KeyValuePairs.Add(EKeyValues.Interaction,        KeyCode.F);
        KeyValuePairs.Add(EKeyValues.ToggleCameraView,   KeyCode.V);
        KeyValuePairs.Add(EKeyValues.ToggleInventoryUI,  KeyCode.Tab);

    }

    // 키 할당 변경 시 사용되는 메소드
    public void KeyChange(EKeyValues keyValue, KeyCode keyCode)
    {
        KeyValuePairs[keyValue] = keyCode;
    }
}
