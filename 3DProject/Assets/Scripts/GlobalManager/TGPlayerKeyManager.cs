using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyValues
{
    None = 0,
    //아래에 Enum값 작성
    Forward,
    Backward,
    Left,
    Right,
    Jump,
    Fire,
    Aim,
    Item1,
    Item2,
    Item3,
    Item4,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    MouseCursorSwitch,
    Interaction,
    ToggleCameraView,
    ToggleInventoryUI,
    //Enum end

    End = 300
}

public class TGPlayerKeyManager : UMonoSingleton<TGPlayerKeyManager>
{
    //public
    public Dictionary<KeyValues, KeyCode> KeyValuePairs { get; private set; }

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
        KeyValuePairs = new Dictionary<KeyValues, KeyCode>();

        KeyValuePairs.Add(KeyValues.Forward,            KeyCode.W);
        KeyValuePairs.Add(KeyValues.Backward,           KeyCode.S);
        KeyValuePairs.Add(KeyValues.Left,               KeyCode.A);
        KeyValuePairs.Add(KeyValues.Right,              KeyCode.D);
        KeyValuePairs.Add(KeyValues.Fire,               KeyCode.Mouse0);
        KeyValuePairs.Add(KeyValues.MouseCursorSwitch,  KeyCode.LeftControl);
        KeyValuePairs.Add(KeyValues.Item1,              KeyCode.Alpha1);
        KeyValuePairs.Add(KeyValues.Item2,              KeyCode.Alpha2);
        KeyValuePairs.Add(KeyValues.Item3,              KeyCode.Alpha3);
        KeyValuePairs.Add(KeyValues.Item4,              KeyCode.Alpha4);
        KeyValuePairs.Add(KeyValues.Interaction,        KeyCode.F);
        KeyValuePairs.Add(KeyValues.ToggleCameraView,   KeyCode.V);
        KeyValuePairs.Add(KeyValues.ToggleInventoryUI,  KeyCode.Tab);

    }

    // 키 할당 변경 시 사용되는 메소드
    public void KeyChange(KeyValues keyValue, KeyCode keyCode)
    {
        KeyValuePairs[keyValue] = keyCode;
    }
}
