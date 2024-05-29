using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyValues
{
    NONE = 0,
    //�Ʒ��� Enum�� �ۼ�
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT,
    JUMP,
    FIRE,
    AIM,
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
    MOUSECURSORSWITCH,
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

    // �ʱⰪ ����
    public void Init()
    {
        KeyValuePairs = new Dictionary<KeyValues, KeyCode>();

        KeyValuePairs.Add(KeyValues.FORWARD,            KeyCode.W);
        KeyValuePairs.Add(KeyValues.BACKWARD,           KeyCode.S);
        KeyValuePairs.Add(KeyValues.LEFT,               KeyCode.A);
        KeyValuePairs.Add(KeyValues.RIGHT,              KeyCode.D);
        KeyValuePairs.Add(KeyValues.FIRE,               KeyCode.Mouse0);
        KeyValuePairs.Add(KeyValues.MOUSECURSORSWITCH,  KeyCode.LeftControl);
    }

    // Ű �Ҵ� ���� �� ���Ǵ� �޼ҵ�
    public void KeyChange(KeyValues keyValue, KeyCode keyCode)
    {
        KeyValuePairs[keyValue] = keyCode;
    }
}
