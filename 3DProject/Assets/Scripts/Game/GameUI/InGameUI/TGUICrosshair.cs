using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    MWeaponStats        characterWeaponStats;
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
        characterWeaponStats = playerCharacter.characterStat.weaponStats;
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

        characterWeaponStats = weaponPtr.weaponStats;
        Debug.Log($"(TGUICrosshair:OnChangeInHandItem) Set weapon stats, now default accuracy: {characterWeaponStats.currentAccuracy}");
    }

    void OnCrosshairUpdate()
    {
        if (characterWeaponStats == null) return;

        // ���߷��� ���� ũ�� ���� (��: ���߷��� �������� �� Ŀ��)
        float size = Mathf.Lerp(0, 100, characterWeaponStats.currentAccuracy);
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
