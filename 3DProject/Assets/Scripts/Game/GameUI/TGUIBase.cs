using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUIBase : MonoBehaviour
{
    //public
    public TGEventManager eventManager;

    //Unity lifecycle
    void Start()
    {
        eventManager = TGEventManager.Instance;
    }

    void Update()
    {
        
    }

    
}
