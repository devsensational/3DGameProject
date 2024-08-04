using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TGAIConditionEnemyFoundPlayer : TGAIConditionNode
{
    public override bool TriggerAction(out TGAIActionNode ptrNode)
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
