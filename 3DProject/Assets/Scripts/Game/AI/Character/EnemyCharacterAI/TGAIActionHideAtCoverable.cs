using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TGAIActionHideAtCoverable : TGAIActionNodeBase
{
    //private
    NavMeshAgent agent;
    List<GameObject> coverableObjectList;

    // Unity Lifecycle
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

        agent.isStopped = false;


        if (coverableObjectList.Count > 0 )
        {
            float distance = Vector3.Distance(transform.position, coverableObjectList[0].transform.position);
            Vector3 position = coverableObjectList[0].transform.position;

            foreach ( GameObject obj in coverableObjectList )
            {
                float compDistance = Vector3.Distance(transform.position, obj.transform.position);
                if(distance > compDistance) 
                {
                    distance = compDistance;
                    position = obj.transform.position;
                }
            }
            agent.SetDestination(position);
        }

        float reloadTime = controller.character.GetInHandWeapon().weaponStats.reloadTime;
        while(reloadTime > 0.1f)
        {
            reloadTime -= Time.deltaTime;
            yield return null;
        }

        Debug.Log($"(TGAIActionHideAtCoverable:Run) isConditionCheckPaused = false");
        controller.isConditionCheckPaused = false;
        yield return null;
    }
}
