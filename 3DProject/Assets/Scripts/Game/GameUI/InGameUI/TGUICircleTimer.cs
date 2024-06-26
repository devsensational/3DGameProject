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
    float timeValue;    // 받은 타이머 시간
    float currentTime; // 현재 시간

    //Unity lifecycle
    private void Start()
    {
        TGEventManager.Instance.StartListening(EEventType.StartCircleTimerUI, StartCircleTimer);
        TGEventManager.Instance.StartListening(EEventType.HideCircleTimerUI, HideCircleTimer);
    }

    void StartCircleTimer(object parameter) // Circle Tiemr 실행
    {
        transform.localPosition = Vector3.zero; // 위치를 중앙으로 이동
        
        timeValue = (float)parameter;
        currentTime = timeValue;
        TimerText.text = $"{currentTime}";

        StartCoroutine(TimerCoroutine()); // 타이머 코루틴 실행
    }

    void HideCircleTimer(object parameter) // Circle Tiemr를 종료하고 숨김
    {
        transform.localPosition = new Vector3(0, 1080, 0);
    }

    IEnumerator TimerCoroutine() // 0.1초마다 시간을 감소시키는 타이머 코루틴
    {
        Debug.Log("(TGUICircleTimer:TimerCoroutine) Start circle timer");
        while(currentTime >= 0)
        {
            yield return null;
            currentTime -= Time.deltaTime;

            // 타이머 텍스트 업데이트
            TimerText.text = $"{currentTime:F1}";

            // Progress circle 비율 업데이트
            float fillRatio = Mathf.Clamp01(currentTime / timeValue);
            ProgressCircle.fillAmount = fillRatio;
        }
        Debug.Log("(TGUICircleTimer:TimerCoroutine) End circle timer");
        HideCircleTimer(null);
    }

}
