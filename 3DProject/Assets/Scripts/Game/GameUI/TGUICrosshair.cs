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
    public float accuracy = 1.0f; // ���߷� (0.0f ~ 1.0f ����)

    void Update()
    {
        // ���߷��� ���� ũ�� ���� (��: ���߷��� �������� �� Ŀ��)
        float size = Mathf.Lerp(0, 200, 1.0f - accuracy);
        centerSquare.sizeDelta = new Vector2(size, size);

        // ���ڼ� ���� ��ġ ������Ʈ
        topPart.anchoredPosition    = new Vector2(0, size / 2);
        bottomPart.anchoredPosition = new Vector2(0, -size / 2);
        leftPart.anchoredPosition   = new Vector2(-size / 2, 0);
        rightPart.anchoredPosition  = new Vector2(size / 2, 0);
    }

    // ���߷� ������Ʈ �Լ�
    public void SetAccuracy(float newAccuracy)
    {
        accuracy = Mathf.Clamp01(newAccuracy);
    }
}
