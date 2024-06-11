using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TGItem : TGObject
{
    //Inspector
    public GameObject ItemModel;

    //public
    public EItemType    itemType;
    public GameObject   itemHolder { get; set; }    //�������� �ֿ� ������Ʈ
    public bool         isDropped = false;          //�������� �ֿ� �� �ִ� �������� Ȯ��

    public MCharacterStats characterStats { get; set; }

    //protected
    protected bool isHandIn = false;

    //private

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
        transform.SetParent(null);  //�θ� ����
        ItemModel.SetActive(true);

        //cl.isTrigger = false;
        //rb.isKinematic = false;
    }


    public void OnPickedUpThisItem(GameObject pickedUpCharacterObject)    // �������� �ֿ����� �� ȣ��
    {
        itemHolder = pickedUpCharacterObject;
        isDropped = false;

        //�������� �տ� ����� �� ���� ������ ���� �ʵ��� ����
        //cl.isTrigger = true;
        //rb.isKinematic = true;

        // �θ� ����
        transform.SetParent(pickedUpCharacterObject.GetComponent<TGCharacter>().HandPosition.transform);
        characterStats = pickedUpCharacterObject.GetComponent<TGCharacter>().characterStat; //�ֿ� ĳ������ ���� ������ ��������

        // ��ġ �� ȸ�� �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        ItemModel.SetActive(false);
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

    public virtual void UseItem() { }

    //�θ��� �������� �޼ҵ带 �������� �ʰ� �޼ҵ� �ۼ��� �����ϰ� �ϴ� �޼ҵ��
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
