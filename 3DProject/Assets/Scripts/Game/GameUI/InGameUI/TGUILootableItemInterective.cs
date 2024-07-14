using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// �κ��丮 UI���� ��ȣ�ۿ��ϴ� ��ư�� ���� Ŭ�����Դϴ�.
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
    RectTransform       rectTransform;

    float originalWidth;
    float originalHeight;

    bool isPicked = false;

    //Unity lifecycle
    void Start()
    {
        InitReferences();
        InitEvent();

        // �������� ������ ����
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

    //UI ���� �޼ҵ�
    public void SetButton(TGItem interectedItem) // button ������ ����
    {
        this.interectedItem = interectedItem;
        interectedItem.itemButton = this;       // ��ư�� ���� ������ ���۷��� ����

        SetItemName();
    }

    public void OnClickButton()    //��ư�� ������ �� ȣ��
    {
        if(isPicked)
        {
            eventManager.TriggerEvent(EEventType.UIDropItemFromInventory, interectedItem);
            eventManager.TriggerEvent(EEventType.DropItemFromInventory, interectedItem);
            SetItemName();
            isPicked = false;
        }
        else
        {
            eventManager.TriggerEvent(EEventType.UIPickedupItemToInventory, interectedItem);
            eventManager.TriggerEvent(EEventType.PickedupItemToInventory, interectedItem);
            SetItemName();
            isPicked = true;
        }
    }

    public void ResetButton()    // ��ư ������ �ʱ�ȭ
    {
        rectTransform.sizeDelta = new Vector2(originalWidth, originalHeight);
        isPicked = false;
        
    }

    public void SetItemName() // �������� ���� �ݿ�
    {
        if(interectedItem.equipmentType == EEquipmentType.None)
        {
            buttonText.text = $"{interectedItem.objectName} ({interectedItem.itemCount})";
            return;
        }
        buttonText.text = $"{interectedItem.objectName}";
    }


    public TGItem IntertectedItem     // getter/setter
    {
        get => interectedItem;
    }
}
