using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TGUICircleTimer : MonoBehaviour
{
    // Insepector
    public TMP_Text TimerText;
    public Image    ProgressCircle;

    // private
    float timeValue;    // ���� Ÿ�̸� �ð�
    float currentTime; // ���� �ð�

    //Unity lifecycle
    private void Start()
    {
        TGEventManager.Instance.StartListening(EEventType.StartCircleTimerUI, StartCircleTimer);
        TGEventManager.Instance.StartListening(EEventType.HideCircleTimerUI, HideCircleTimer);
    }

    void StartCircleTimer(object parameter) // Circle Tiemr ����
    {
        transform.localPosition = Vector3.zero; // ��ġ�� �߾����� �̵�
        
        timeValue = (float)parameter;
        currentTime = timeValue;
        TimerText.text = $"{currentTime}";

        StartCoroutine(TimerCoroutine()); // Ÿ�̸� �ڷ�ƾ ����
    }

    void HideCircleTimer(object parameter) // Circle Tiemr�� �����ϰ� ����
    {
        transform.localPosition = new Vector3(0, 1080, 0);
    }

    IEnumerator TimerCoroutine() // 0.1�ʸ��� �ð��� ���ҽ�Ű�� Ÿ�̸� �ڷ�ƾ
    {
        Debug.Log("(TGUICircleTimer:TimerCoroutine) Start circle timer");
        while(currentTime >= 0)
        {
            yield return null;
            currentTime -= Time.deltaTime;

            // Ÿ�̸� �ؽ�Ʈ ������Ʈ
            TimerText.text = $"{currentTime:F1}";

            // Progress circle ���� ������Ʈ
            float fillRatio = Mathf.Clamp01(currentTime / timeValue);
            ProgressCircle.fillAmount = fillRatio;
        }
        Debug.Log("(TGUICircleTimer:TimerCoroutine) End circle timer");
        HideCircleTimer(null);
    }

}
