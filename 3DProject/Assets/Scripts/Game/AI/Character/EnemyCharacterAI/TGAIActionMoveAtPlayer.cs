using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TGAIActionMoveAtPlayer : TGAIActionNodeBase
{
    // Inspector
    public float moveSpeed = 4f;
    public float stopDistance = 0f;

    // private
    NavMeshAgent agent;
    TGCharacter character;

    // Unity Lifecycle
    public override void Start()
    {
        base.Start();
        agent       = GetComponent<NavMeshAgent>();
        character   = GetComponent<TGCharacter>();
    }

    public override IEnumerator Run()
    {
        if (isRunning) { yield break; }

        isRunning = true;
        agent.isStopped = false;
        agent.SetDestination(controller.playerCharacterList[0].transform.position);
        character.currnetSpeed = moveSpeed;

        while(agent.remainingDistance > stopDistance && !agent.isStopped)
        {
            yield return null;
        }

        character.currnetSpeed = 0f;
        isRunning = false;
    }
}
