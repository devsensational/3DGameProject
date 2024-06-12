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
    TGObjectPoolManager poolManager;
    TGPlayerCharacter   playerCharacter;    // 아이템을 주울 캐릭터
    TGItem              interectedItem;     // 버튼 누를 시 상호작용 될 아이템
    RectTransform       rectTransform;

    float originalWidth;
    float originalHeight;

    bool isPicked = false;
    //Unity lifecycle

    void Start()
    {
        InitReferences();
        InitEvent();

        // 오리지널 사이즈 저장
        originalWidth = rectTransform.rect.width;
        originalHeight = rectTransform.rect.height;
    }

    //Init
    void InitReferences()
    {
        playerCharacter = GameObject.Find("PlayerCharacter").GetComponent<TGPlayerCharacter>();
        eventManager    = TGEventManager.Instance;
        poolManager     = TGObjectPoolManager.Instance;
        rectTransform   = GetComponent<RectTransform>();
    }

    void InitEvent()
    {
        //eventManager.StartListening(EEventType.UIDropItemFromInventory, OnReleaseThisItem);
    }

    //UI 관련 메소드
    public void SetButton(TGItem interectedItem) // button 생성시 셋팅
    {
        this.interectedItem = interectedItem;
        buttonText.text = interectedItem.objectName;
    }

    //버튼이 눌렸을 때 호출
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
            eventManager.TriggerEvent(EEventType.UIPickedupItemToInventory, interectedItem);
            eventManager.TriggerEvent(EEventType.PickedupItemToInventory, interectedItem);
            isPicked = true;
        }
    }

    // 버튼 사이즈 초기화
    public void ResetButton()
    {
        rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight);
        isPicked = false;
        
    }

    // getter/setter
    public TGItem IntertectedItem
    {
        get => interectedItem;
    }
}
