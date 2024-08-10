using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TGAIController : TGAIActionNodeBase
{
    //Inspector
    public TGAIActionNodeBase nodePtr = null;

    [Header("Checking parameters (Don't add object here)")]
    public List<GameObject> coverableObjectList = new List<GameObject>();
    public List<GameObject> playerCharacterList = new List<GameObject>();

    //public
    public TGCharacter character { get; private set; }
    public bool isConditionCheckPaused { get; set; } // true�� ���ǹ� üũ�� �������� �ʽ��ϴ�. ����� Run�� �����ϴ� ���� ���¸� �����ϰ� ���� ���� �� ���

    // Unity Lifecycle
    void Awake()
    {
        if (nodePtr == null) { nodePtr = this; }
        character = GetComponent<TGCharacter>();  
    }

    void Update()
    {
        
        CheckConditions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCharacterList.Add(other.gameObject);
        }
        if (other.gameObject.tag == "CoverableObject")
        {
            coverableObjectList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCharacterList.Remove(other.gameObject);
        }
        if (other.gameObject.tag == "CoverableObject")
        {
            coverableObjectList.Remove(other.gameObject);
        }
    }

    public override void Start()
    {

    }

    void CheckConditions() // List�� condition���� ��ȸ�ϴ� �޼ҵ�, condition ���� �� ���� �ൿ�� �����ϰ� �����͸� �����մϴ�.
    {
        if(nodePtr == null) { return; }
        if(isConditionCheckPaused) { return; }

        foreach (TGAIConditionNodeBase condition in nodePtr.conditionNodes)
        {
            if (condition.TriggerAction(out TGAIActionNodeBase node))
            {
                if(node.isNextStatus)
                {
                    condition.action.parentNode = nodePtr;
                    nodePtr = node;
                    Debug.Log($"(TGAIController:CheckConditions) Changed node pointer");
                }

                StartCoroutine(condition.action.Run());
            }
        }
    }

    public void MoveToParentNode()
    {
        nodePtr = nodePtr.parentNode;
    }

    public override IEnumerator Run()
    {
        //Controller�� Root�̱� ������ ���๮�� �����ϴ�.
        yield return null;
    }
}
