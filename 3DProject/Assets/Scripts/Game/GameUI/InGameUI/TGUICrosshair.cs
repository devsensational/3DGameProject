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

    public float accuracy; // 명중률 (0.0f ~ 1.0f 사이)

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
        eventManager.StartListening(EEventType.ToggleInventoryUI, OnToggleUI);      //인벤토리 킬 때 토글
        eventManager.StartListening(EEventType.ChangeHandItem, OnChangeInHandItem); //InHandItem 변경 시 아이템 스탯 반영
    }

    // 명중률 업데이트 함수
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

        // 명중률에 따른 크기 조정 (예: 명중률이 낮을수록 더 커짐)
        float size = Mathf.Lerp(0, 100, characterWeaponStats.currentAccuracy);
        //accuracy = characterWeaponStats.currentAccuracy;

        //centerSquare.sizeDelta = new Vector2(size, size);

        // 십자선 파츠 위치 업데이트
        topPart.anchoredPosition    = new Vector2(0, size / 2);
        bottomPart.anchoredPosition = new Vector2(0, -size / 2);
        leftPart.anchoredPosition   = new Vector2(-size / 2, 0);
        rightPart.anchoredPosition  = new Vector2(size / 2, 0);
    }

    //UI On/Off 토글, 이벤트 트리거에 의해 실행됨
    void OnToggleUI(object parameters)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
