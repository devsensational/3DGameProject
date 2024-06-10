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
    TGPlayerCharacter   playerCharacter;    // �������� �ֿ� ĳ����
    TGItem              interectedItem;     // ��ư ���� �� ��ȣ�ۿ� �� ������

    bool isPicked = false;
    //Unity lifecycle
    private void Start()
    {
        InitReferences();
    }

    //Init
    void InitReferences()
    {
        playerCharacter = GameObject.Find("PlayerCharacter").GetComponent<TGPlayerCharacter>();
        eventManager    = TGEventManager.Instance;
    }

    //UI ���� �޼ҵ�
    public void SetButton(TGItem interectedItem) // button ������ ����
    {
        this.interectedItem = interectedItem;
        buttonText.text = interectedItem.name;
    }

    public void OnClickButton()
    {
        if(isPicked)
        {
            isPicked = false;
            interectedItem.OnDropThisItem();
            eventManager.TriggerEvent(EEventType.DropItemFromInventory, this);
        }
        else
        {
            playerCharacter.TakeItem(interectedItem);
            eventManager.TriggerEvent(EEventType.PickedupItemToInventory, this);
            isPicked = true;
        }
    }

    //
    public TGItem IntertectedItem
    {
        get => interectedItem;
    }
}
