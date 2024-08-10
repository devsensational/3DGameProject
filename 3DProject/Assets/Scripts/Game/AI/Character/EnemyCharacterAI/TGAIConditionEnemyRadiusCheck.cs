using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TGAIConditionEnemyRadiusCheck : TGAIConditionNodeBase
{
    public override bool TriggerAction(out TGAIActionNodeBase ptrNode)
    {
        if (controller.playerCharacterList.Count > 0)
        {
            ptrNode = action;
            return true;
        }
        ptrNode = null;
        return false;
    }
}
