using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEventType
{
    None = 0,

    //���⼭ ���� �ۼ�
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

