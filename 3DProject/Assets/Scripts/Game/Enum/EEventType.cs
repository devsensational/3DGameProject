using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eventmanager에서 호출되는 Event 타입을 위한 enum클래스입니다.
public enum EEventType
{
    None = 0,

    //여기서 부터 작성
    ToggleInventoryUI,
    UIEnterInteractiveItem,
    UIExitInteractiveItem,
    UIPickedupItemToInventory,
    UIDropItemFromInventory,
    PickedupItemToInventory,
    DropItemFromInventory,
    UICreateItemButton,
    ChangeHandItem,
    UpdateItemInfo,
    RemoveItemInfoUIText,
    StartCircleTimerUI,
    HideCircleTimerUI,
}

