using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGAIActionMoveToParentNode : TGAIActionNode
{
    public override IEnumerator Run()
    {
        controller.MoveToParentNode();
        yield return null;
    }
}
