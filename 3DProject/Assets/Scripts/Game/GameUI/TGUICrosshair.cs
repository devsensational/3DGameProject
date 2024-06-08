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
    public float accuracy; // ���߷� (0.0f ~ 1.0f ����)

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
        eventManager.StartListening(EEventType.ToggleInventoryUI, OnToggleUI); //�κ��丮 ų �� ���
    }

    // ���߷� ������Ʈ �Լ�
    public void SetAccuracy(float newAccuracy)
    {
        accuracy = Mathf.Clamp01(newAccuracy);
    }

    void OnCrosshairUpdate()
    {
        // ���߷��� ���� ũ�� ���� (��: ���߷��� �������� �� Ŀ��)
        float size = Mathf.Lerp(0, 200, 1.0f - characterStats.currentAccuracy);
        centerSquare.sizeDelta = new Vector2(size, size);

        // ���ڼ� ���� ��ġ ������Ʈ
        topPart.anchoredPosition    = new Vector2(0, size / 2);
        bottomPart.anchoredPosition = new Vector2(0, -size / 2);
        leftPart.anchoredPosition   = new Vector2(-size / 2, 0);
        rightPart.anchoredPosition  = new Vector2(size / 2, 0);
    }

    //UI On/Off ���
    void OnToggleUI(object parameters)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
