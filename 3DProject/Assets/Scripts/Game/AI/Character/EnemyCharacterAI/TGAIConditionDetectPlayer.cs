using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGAIConditionDetectPlayer : TGAIConditionNode
{
    List<GameObject> playerCharacterList;
    RaycastHit hit;
    int layerMask;

    // Unity lifecycle
    public override void Start()
    {
        base.Start();
        playerCharacterList = controller.playerCharacterList;
        layerMask = 1 << LayerMask.NameToLayer("Terriain"); // AI와 플레이어 간 장애물이 있는지 확인하기 위한 레이어마스크
    }

    //
    public override bool TriggerAction(out TGAIActionNode ptrNode)
    {
        ptrNode = null;
        if (playerCharacterList.Count <= 0) { return false; }
        if (RaycastObstacleCheck())
        {
            ptrNode = action;
            return true;
        }

        return false;
    }

    bool RaycastObstacleCheck() // Player와 AI간 장애물 유무 체크 메소드, 존재하지 않으면 true 반환
    {
        if (playerCharacterList.Count < 1) return false;

        Vector3 direction = playerCharacterList[0].GetComponent<Collider>().bounds.center - transform.position;
        Debug.DrawRay(transform.position, direction);

        if (Physics.Raycast(transform.position, direction, out hit, direction.magnitude, layerMask))
        {
            if (hit.transform.gameObject.layer == 6)
            {
                return false;
            }
        }

        return true;
    }
}
