using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGAIConditionPlayerOutEnemyRecognizeRange : TGAIConditionNodeBase
{
    //private
    List<GameObject> playerCharacterList;

    // Unity Lifecycle
    public override void Start()
    {
        base.Start();
        playerCharacterList = controller.playerCharacterList;
    }

    public override bool TriggerAction(out TGAIActionNodeBase ptrNode)
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
