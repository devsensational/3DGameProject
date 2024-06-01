using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyValues
{
    NONE = 0,
    //아래에 Enum값 작성
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT,
    JUMP,
    FIRE,
    AIM,
    ITEM1,
    ITEM2,
    ITEM3,
    ITEM4,
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
    MOUSECURSORSWITCH,
    INTERACTION,
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

        KeyValuePairs.Add(KeyValues.FORWARD,            KeyCode.W);
        KeyValuePairs.Add(KeyValues.BACKWARD,           KeyCode.S);
        KeyValuePairs.Add(KeyValues.LEFT,               KeyCode.A);
        KeyValuePairs.Add(KeyValues.RIGHT,              KeyCode.D);
        KeyValuePairs.Add(KeyValues.FIRE,               KeyCode.Mouse0);
        KeyValuePairs.Add(KeyValues.MOUSECURSORSWITCH,  KeyCode.LeftControl);
        KeyValuePairs.Add(KeyValues.ITEM1,              KeyCode.Alpha1);
        KeyValuePairs.Add(KeyValues.ITEM2,              KeyCode.Alpha2);
        KeyValuePairs.Add(KeyValues.ITEM3,              KeyCode.Alpha3);
        KeyValuePairs.Add(KeyValues.ITEM4,              KeyCode.Alpha4);
        KeyValuePairs.Add(KeyValues.INTERACTION,        KeyCode.F);

    }

    // 키 할당 변경 시 사용되는 메소드
    public void KeyChange(KeyValues keyValue, KeyCode keyCode)
    {
        KeyValuePairs[keyValue] = keyCode;
    }
}
