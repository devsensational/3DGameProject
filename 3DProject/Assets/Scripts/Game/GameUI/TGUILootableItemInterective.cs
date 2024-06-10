using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TGUILootableItemInterective : MonoBehaviour
{
    //Inspector
    public TMP_Text buttonText;         // 버튼에 쓰이는 텍스트

    //public
    

    //private
    TGEventManager      eventManager;
    TGPlayerCharacter   playerCharacter;    // 아이템을 주울 캐릭터
    TGItem              interectedItem;     // 버튼 누를 시 상호작용 될 아이템

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

    //UI 관련 메소드
    public void SetButton(TGItem interectedItem) // button 생성시 셋팅
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
