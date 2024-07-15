using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȭ�� �߾ӿ� ������ ���߷��� ǥ���ϱ� ���� UI Ŭ�����Դϴ�.
public class TGUICrosshair : MonoBehaviour
{
    //Inspector
    public RectTransform centerSquare;
    public RectTransform topPart;
    public RectTransform bottomPart;
    public RectTransform leftPart;
    public RectTransform rightPart;

    public float accuracy; // ���߷� (0.0f ~ 1.0f ����)

    // private
    // references
    TGPlayerCharacter   playerCharacter;
    TGItemWeapon        characterWeapon;
    TGEventManager      eventManager;

    //Unity lifecycle
    private void Start()
    {
        InitReferences();
        InitEvent();
    }

    void Update()
    {
        OnCrosshairUpdate();
    }

    //Init
    void InitReferences()
    {
        playerCharacter = GameObject.Find("PlayerCharacter").GetComponent<TGPlayerCharacter>();
        //characterWeapon = playerCharacter.characterStat.weaponStats;
        eventManager = TGEventManager.Instance;
    }

    void InitEvent()
    {
        eventManager.StartListening(EEventType.ToggleInventoryUI, OnToggleUI);      //�κ��丮 ų �� ���
        eventManager.StartListening(EEventType.ChangeHandItem, OnChangeInHandItem); //InHandItem ���� �� ������ ���� �ݿ�
    }

    // ���߷� ������Ʈ �Լ�
    public void SetAccuracy(float newAccuracy)
    {
        accuracy = Mathf.Clamp01(newAccuracy);
    }

    void OnChangeInHandItem(object parameter)
    {
        TGItemWeapon weaponPtr = (TGItemWeapon)parameter;

        characterWeapon = weaponPtr;
    }

    void OnCrosshairUpdate()
    {
        if (characterWeapon == null) return;

        // ���߷��� ���� ũ�� ���� (��: ���߷��� �������� �� Ŀ��)
        float size = Mathf.Lerp(0, 100, characterWeapon.currentAccuracy);
        //accuracy = characterWeaponStats.currentAccuracy;

        //centerSquare.sizeDelta = new Vector2(size, size);

        // ���ڼ� ���� ��ġ ������Ʈ
        topPart.anchoredPosition    = new Vector2(0, size / 2);
        bottomPart.anchoredPosition = new Vector2(0, -size / 2);
        leftPart.anchoredPosition   = new Vector2(-size / 2, 0);
        rightPart.anchoredPosition  = new Vector2(size / 2, 0);
    }

    //UI On/Off ���, �̺�Ʈ Ʈ���ſ� ���� �����
    void OnToggleUI(object parameters)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
