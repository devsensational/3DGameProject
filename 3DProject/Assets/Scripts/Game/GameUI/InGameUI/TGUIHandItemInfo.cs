using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 현재 플레이어가 들고 있는 무기의 정보를 표시하기 위한 UI 클래스입니다.
public class TGUIHandItemInfo : MonoBehaviour
{
    //inspector
    public TMP_Text itemName;
    public TMP_Text itemCount;

    //public


    //private
    TGEventManager  eventManager;
    TGItem          item;

    //Unity lifecycle
    void Start()
    {
        InitReferences();
        InitEvent();
        RemoveItemInfo(null);
    }

    void Update()
    {

    }

    //Init
    void InitReferences()
    {
        eventManager = TGEventManager.Instance;
    }

    void InitEvent()
    {
        eventManager.StartListening(EEventType.ChangeHandItem, OnChangeHandItem);
        eventManager.StartListening(EEventType.UpdateItemInfo, OnUpdateWeaponCountStat);
        eventManager.StartListening(EEventType.RemoveItemInfoUIText, RemoveItemInfo);
    }

    //
    void OnChangeHandItem(object parameter)
    {
        item = (TGItem)parameter;
        itemName.text = item.objectName;

        if(item.equipmentType == EEquipmentType.None)
        {
            itemCount.text = "";
        }
    }

    void OnUpdateWeaponCountStat(object parameter)
    {
        item = (TGItem)parameter;
        int currentCount = item.itemCount;

        if(item.equipmentType != EEquipmentType.None)
        {
            TGItemWeapon weaponPtr = (TGItemWeapon)item;
            int maxCount = weaponPtr.weaponStats.maxAmmo;
            itemCount.text = $"{weaponPtr.weaponStats.currentAmmo} / {maxCount}";
        }
        Debug.Log("(TGUIHandItemInfo:OnUpdateWeaponCountStat) Updated weapon stat");
    }

    void RemoveItemInfo(object parameter)
    {
        itemName.text = "";
        itemCount.text = "";
    }

}
