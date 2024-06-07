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
    // 딕셔너리를 사용하여 이벤트 타입과 해당 델리게이트를 저장
    private Dictionary<EventType, Action<object>> eventDictionary = new Dictionary<EventType, Action<object>>();

    // 이벤트리스닝 시작
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

    // 이벤트리스닝 중지
    public void StopListening(EventType eventType, Action<object> listener)
    {
        Action<object> thisEvent;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventType] = thisEvent;
        }
    }

    // 이벤트 트리거
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
