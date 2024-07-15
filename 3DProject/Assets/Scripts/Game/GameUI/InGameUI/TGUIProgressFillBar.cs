using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TGUIProgressFillBar : MonoBehaviour
{
    // Insepector
    public EEventType   eventType;
    public Image        fillImage;

    //private
    TGEventManager eventManager;

    private void Start()
    {
        eventManager = TGEventManager.Instance;

        eventManager.StartListening(eventType, OnUpdateBar);
    }

    public void OnUpdateBar(object parameter)
    {
        fillImage.fillAmount = (float)parameter;
    }
}
