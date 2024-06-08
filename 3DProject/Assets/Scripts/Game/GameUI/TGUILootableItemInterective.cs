using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TGUILootableItemInterective : MonoBehaviour
{
    //Inspector
    public TMP_Text buttonText;         // ��ư�� ���̴� �ؽ�Ʈ

    //private
    TGEventManager      eventManager;
    TGPlayerCharacter   playerCharacter;    // �������� �ֿ� ĳ����
    TGItem              interectedItem;     // ��ư ���� �� ��ȣ�ۿ� �� ������

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
        playerCharacter.TakeItem(interectedItem);
        eventManager.TriggerEvent(EEventType.ExitInteractiveItem, interectedItem);
    }
}
