using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TGItem : TGObject
{
    //Inspector
    public GameObject ItemModel;
    public string groundTag = "Terrain"; // �ٴڿ� ����� �±�

    //public
    public EItemType    itemType;
    public GameObject   itemHolder { get; set; }    //�������� �ֿ� ������Ʈ
    public bool         isDropped = false;          //�������� �ֿ� �� �ִ� �������� Ȯ��

    public MCharacterStats characterStats { get; set; }

    //protected
    protected bool isHandIn = false;

    //private
    float maxRayDistance = 100f; // Raycast�� �ִ� �Ÿ�

    Rigidbody rb;
    Collider cl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();

        ChildAwake();
    }

    void Start()
    {
        ChildStart();
    }

    void Update()
    {
        ChildUpdate();
    }

    // ������ ��ȣ�ۿ� ���� �޼ҵ�
    public void OnDropThisItem()    // �������� �������� �� ȣ��
    {
        itemHolder = null;          //�ش� ������ ������ �ʱ�ȭ
        isDropped = true;           //������ �ִ� ���·� ����
        isHandIn = false;   
        transform.SetParent(null);  //�θ� ����
        ItemModel.SetActive(true);

        DropToGround();
        //cl.isTrigger = false;
        //rb.isKinematic = false;
    }


    public void OnPickedUpThisItem(GameObject pickedUpCharacterObject)    // �������� �ֿ����� �� ȣ��
    {
        itemHolder = pickedUpCharacterObject;
        isDropped = false;

        TGCharacter chracterComponent = itemHolder.GetComponent<TGCharacter>();

        // �θ� ����
        transform.SetParent(chracterComponent.HandPosition.transform);
        characterStats = chracterComponent.characterStat; //�ֿ� ĳ������ ���� ������ ��������

        // ��ġ �� ȸ�� �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if(chracterComponent.GetHandInItem() != itemType)
        {
            ItemModel.SetActive(false);
        }
    }

    public void OnHandInThisItem()
    {
        if(isHandIn)
        {
            isHandIn = false;
            ItemModel.SetActive(false);
            Debug.Log(objectName + " hand off from " + itemHolder.name);
        }
        else
        {
            isHandIn = true;
            ItemModel.SetActive(true);
            Debug.Log(objectName + " on hand to " + itemHolder.name);
        }
    }

    // �������� �� ���� 
    void DropToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxRayDistance))
        {
            // Raycast�� �ٴڿ� ��Ҵ��� Ȯ��
            if (hit.collider.CompareTag(groundTag))
            {
                // �ٴڿ� ����� ��� ������Ʈ�� ��ġ�� �ٴ��� ��ġ�� ����
                Vector3 newPosition = hit.point;
                transform.position = newPosition;


                // ������Ʈ�� X������ 90�� ȸ��
                transform.rotation = Quaternion.Euler(0, 0, 90);

                Debug.Log("Object dropped to ground at position: " + newPosition);
            }
            else
            {
                Debug.Log("Raycast did not hit an object with the tag: " + groundTag);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    public virtual void UseItem() { }

    //�θ��� �������� �޼ҵ带 �������� �ʰ� �޼ҵ� �ۼ��� �����ϰ� �ϴ� �޼ҵ��
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
