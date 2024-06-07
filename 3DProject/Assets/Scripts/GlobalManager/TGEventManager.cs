using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None = 0,
    ToggleInventoryUI,

}

public class TGEventManager : UMonoSingleton<TGEventManager>
{
    // ��ųʸ��� ����Ͽ� �̺�Ʈ Ÿ�԰� �ش� ��������Ʈ�� ����
    private Dictionary<EventType, Action<object>> eventDictionary = new Dictionary<EventType, Action<object>>();

    // �̺�Ʈ������ ����
    public void StartListening(EventType eventType, Action<object> listener)
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
    public void StopListening(EventType eventType, Action<object> listener)
    {
        Action<object> thisEvent;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventType] = thisEvent;
        }
    }

    // �̺�Ʈ Ʈ����
    public void TriggerEvent(EventType eventType, object parameter = null)
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
