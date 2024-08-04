using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGAIConditionPlayerOutEnemyRecognizeRange : TGAIConditionNode
{
    List<GameObject> playerCharacterList;

    public override void Start()
    {
        base.Start();
        playerCharacterList = controller.playerCharacterList;
    }

    public override bool TriggerAction(out TGAIActionNode ptrNode)
    {
        if(controller.playerCharacterList.Count <= 0)
        {
            ptrNode = action;
            return true;
        }
        ptrNode = null;
        return false;
    }
}
