using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

