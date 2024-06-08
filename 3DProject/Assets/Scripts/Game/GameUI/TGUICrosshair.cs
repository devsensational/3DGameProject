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

    [Range(0, 1)]
    public float accuracy; // 명중률 (0.0f ~ 1.0f 사이)

    //private
    // references
    TGPlayerCharacter   playerCharacter;
    MCharacterStats     characterStats;
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
        characterStats = playerCharacter.characterStat;
        eventManager = TGEventManager.Instance;
    }

    void InitEvent()
    {
        eventManager.StartListening(EEventType.ToggleInventoryUI, OnToggleUI); //인벤토리 킬 때 토글
    }

    // 명중률 업데이트 함수
    public void SetAccuracy(float newAccuracy)
    {
        accuracy = Mathf.Clamp01(newAccuracy);
    }

    void OnCrosshairUpdate()
    {
        // 명중률에 따른 크기 조정 (예: 명중률이 낮을수록 더 커짐)
        float size = Mathf.Lerp(0, 200, 1.0f - characterStats.currentAccuracy);
        centerSquare.sizeDelta = new Vector2(size, size);

        // 십자선 파츠 위치 업데이트
        topPart.anchoredPosition    = new Vector2(0, size / 2);
        bottomPart.anchoredPosition = new Vector2(0, -size / 2);
        leftPart.anchoredPosition   = new Vector2(-size / 2, 0);
        rightPart.anchoredPosition  = new Vector2(size / 2, 0);
    }

    //UI On/Off 토글
    void OnToggleUI(object parameters)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
