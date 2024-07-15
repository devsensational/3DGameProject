using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면 중앙에 무기의 명중률을 표시하기 위한 UI 클래스입니다.
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

        characterWeapon = weaponPtr;
    }

    void OnCrosshairUpdate()
    {
        if (characterWeapon == null) return;

        // 명중률에 따른 크기 조정 (예: 명중률이 낮을수록 더 커짐)
        float size = Mathf.Lerp(0, 100, characterWeapon.currentAccuracy);
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
