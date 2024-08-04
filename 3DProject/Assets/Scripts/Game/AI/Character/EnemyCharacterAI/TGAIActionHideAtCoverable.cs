using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TGAIActionHideAtCoverable : TGAIActionNode
{
    NavMeshAgent agent;
    List<GameObject> coverableObjectList;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        coverableObjectList = controller.coverableObjectList;
    }

    public override IEnumerator Run()
    {
        controller.isConditionCheckPaused = true;
        controller.character.CommandReloadInHandItem();

        if(coverableObjectList.Count > 0 )
        {
            agent.SetDestination(coverableObjectList[0].transform.position);
        }

        while(agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        Debug.Log($"(TGAIActionHideAtCoverable:Run) isConditionCheckPaused = false");
        controller.isConditionCheckPaused = false;
        yield return null;
    }
}
