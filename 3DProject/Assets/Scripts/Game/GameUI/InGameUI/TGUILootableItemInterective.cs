using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TGUILootableItemInterective : MonoBehaviour
{
    //Inspector
    public TMP_Text buttonText;         // ��ư�� ���̴� �ؽ�Ʈ

    //public
    

    //private
    TGEventManager      eventManager;
    TGObjectPoolManager poolManager;
    TGPlayerCharacter   playerCharacter;    // �������� �ֿ� ĳ����
    TGItem              interectedItem;     // ��ư ���� �� ��ȣ�ۿ� �� ������

    bool isPicked = false;
    //Unity lifecycle
    private void Start()
    {
        InitReferences();
        InitEvent();
    }

    //Init
    void InitReferences()
    {
        playerCharacter = GameObject.Find("PlayerCharacter").GetComponent<TGPlayerCharacter>();
        eventManager    = TGEventManager.Instance;
        poolManager     = TGObjectPoolManager.Instance;
    }

    void InitEvent()
    {
        //eventManager.StartListening(EEventType.UIDropItemFromInventory, OnReleaseThisItem);
    }

    //UI ���� �޼ҵ�
    public void SetButton(TGItem interectedItem) // button ������ ����
    {
        this.interectedItem = interectedItem;
        buttonText.text = interectedItem.objectName;
    }

    //��ư�� ������ �� ȣ��
    public void OnClickButton()
    {
        if(isPicked)
        {
            eventManager.TriggerEvent(EEventType.UIDropItemFromInventory, interectedItem);
            eventManager.TriggerEvent(EEventType.DropItemFromInventory, interectedItem);
            isPicked = false;
        }
        else
        {
            eventManager.TriggerEvent(EEventType.UIPickedupItemToInventory, this);
            eventManager.TriggerEvent(EEventType.PickedupItemToInventory, interectedItem);
            isPicked = true;
        }
    }

    private void OnReleaseThisItem(object parameter)
    {
        TGItem ptrItem = (TGItem)parameter;   
        if(isPicked && ptrItem == interectedItem)
        {
            isPicked = false;
            poolManager.ReleaseTGObject(ETGObjectType.UILootableItemButton, gameObject);

        }
    }

    // getter/setter
    public TGItem IntertectedItem
    {
        get => interectedItem;
    }
}
