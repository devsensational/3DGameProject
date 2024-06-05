using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUICrosshair : MonoBehaviour
{
    public RectTransform centerSquare;
    public RectTransform topPart;
    public RectTransform bottomPart;
    public RectTransform leftPart;
    public RectTransform rightPart;

    [Range(0, 1)]
    public float accuracy = 1.0f; // 명중률 (0.0f ~ 1.0f 사이)

    void Update()
    {
        // 명중률에 따른 크기 조정 (예: 명중률이 낮을수록 더 커짐)
        float size = Mathf.Lerp(0, 200, 1.0f - accuracy);
        centerSquare.sizeDelta = new Vector2(size, size);

        // 십자선 파츠 위치 업데이트
        topPart.anchoredPosition    = new Vector2(0, size / 2);
        bottomPart.anchoredPosition = new Vector2(0, -size / 2);
        leftPart.anchoredPosition   = new Vector2(-size / 2, 0);
        rightPart.anchoredPosition  = new Vector2(size / 2, 0);
    }

    // 명중률 업데이트 함수
    public void SetAccuracy(float newAccuracy)
    {
        accuracy = Mathf.Clamp01(newAccuracy);
    }
}
