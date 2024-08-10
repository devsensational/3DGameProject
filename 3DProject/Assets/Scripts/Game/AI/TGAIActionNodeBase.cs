using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TGAIActionNodeBase : MonoBehaviour
{
    //Inspector
    public List<TGAIConditionNodeBase> conditionNodes;
    public bool isNextStatus = false; // �� ��带 ���� ���·� ������ ä �����ϰ� ������ true�� ������ ��

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
