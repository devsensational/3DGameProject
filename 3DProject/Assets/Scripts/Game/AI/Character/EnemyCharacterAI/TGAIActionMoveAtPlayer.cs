using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TGAIActionMoveAtPlayer : TGAIActionNode
{
    NavMeshAgent agent;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    public override IEnumerator Run()
    {
        agent.SetDestination(controller.playerCharacterList[0].transform.position);
        yield return null;
    }
}
