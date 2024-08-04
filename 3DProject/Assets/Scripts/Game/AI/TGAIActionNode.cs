using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TGAIActionNode : MonoBehaviour
{
    //Inspector
    public List<TGAIConditionNode> conditionNodes;
    public bool isNextStatus = false; // �� ��带 ���� ���·� ������ ä �����ϰ� ������ true�� ������ ��

    [Header("")]
    public TGAIActionNode parentNode;
    public TGAIController controller; //root

    public virtual void Start()
    {
        controller = GetComponent<TGAIController>();
    }

    public abstract IEnumerator Run();
}
