using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TGAIConditionNode : MonoBehaviour
{
    public TGAIActionNode action;
    public TGAIController controller;

    public virtual void Start()
    {
        controller = GetComponent<TGAIController>();
    }

    public abstract bool TriggerAction(out TGAIActionNode ptrNode);
}
