using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ �� �̺�Ʈ�� �����ϱ� ���� Ŭ�����Դϴ�.
public class TGEventManager : UMonoSingleton<TGEventManager>
{
    // ��ųʸ��� ����Ͽ� �̺�Ʈ Ÿ�԰� �ش� ��������Ʈ�� ����
    private Dictionary<EEventType, Action<object>> eventDictionary = new Dictionary<EEventType, Action<object>>();

    // �̺�Ʈ������ ����
    public void StartListening(EEventType eventType, Action<object> listener)
    {
        Action<object> thisEvent;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventType] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(eventType, thisEvent);
        }
    }

    // �̺�Ʈ������ ����
    public void StopListening(EEventType eventType, Action<object> listener)
    {
        Action<object> thisEvent;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventType] = thisEvent;
        }
    }

    // �̺�Ʈ Ʈ����
    public void TriggerEvent(EEventType eventType, object parameter = null)
    {
        Action<object> thisEvent = null;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke(parameter);
        }
    }

    protected override void ChildAwake()
    {

    }

    protected override void ChildOnDestroy()
    {

    }
}
