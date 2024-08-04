using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TGAIActionNode : MonoBehaviour
{
    //Inspector
    public List<TGAIConditionNode> conditionNodes;
    public bool isNextStatus = false; // 이 노드를 다음 상태로 유지한 채 변경하고 싶으면 true로 선택할 것

    [Header("")]
    public TGAIActionNode parentNode;
    public TGAIController controller; //root

    public virtual void Start()
    {
        controller = GetComponent<TGAIController>();
    }

    public abstract IEnumerator Run();
}
