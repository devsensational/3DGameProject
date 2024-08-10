using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TGAIConditionNodeBase : MonoBehaviour
{
    //public
    public TGAIActionNodeBase action;
    public TGAIController controller;

    // Unity Lifecycle
    public virtual void Start()
    {
        controller = GetComponent<TGAIController>();
    }

    public abstract bool TriggerAction(out TGAIActionNodeBase ptrNode);
}
