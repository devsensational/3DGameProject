using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TGAIActionNodeBase : MonoBehaviour
{
    //Inspector
    public List<TGAIConditionNodeBase> conditionNodes;
    public bool isNextStatus = false; // 이 노드를 다음 상태로 유지한 채 변경하고 싶으면 true로 선택할 것

    [Header("")]
    public TGAIActionNodeBase parentNode;
    public TGAIController controller; //root

    //protected
    protected float cooldown = 0f;
    protected bool  isRunning = false;

    // Unity Lifecycle
    public virtual void Start()
    {
        controller = GetComponent<TGAIController>();
    }

    public abstract IEnumerator Run();
}
