using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
