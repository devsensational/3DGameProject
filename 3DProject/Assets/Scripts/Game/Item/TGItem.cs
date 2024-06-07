using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default = 0,

    //���⼭ ���� enum �ۼ�
    PrimaryWeapon,
    SecondaryWeapon,
    //

    End = 999
}

public class TGItem : TGObject
{
    //public
    public ItemType     itemType;
    public GameObject   itemHolder { get; set; } //�������� �ֿ� ������Ʈ
    public bool         isDropped = false; //�������� �ֿ� �� �ִ� �������� Ȯ��

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
        itemHolder = null;
        isDropped = true;
    }


    public void OnPickedUpThisItem(GameObject pickedUpCharacterObject)    // �������� �ֿ����� �� ȣ��
    {
        itemHolder = pickedUpCharacterObject;
        isDropped = false;

        //�������� �տ� ����� �� ���� ������ ���� �ʵ��� ����
        cl.isTrigger = true;
        rb.isKinematic = true;

        // �θ� ����
        transform.SetParent(pickedUpCharacterObject.GetComponent<TGCharacter>().HandObject.transform);
        characterStats = pickedUpCharacterObject.GetComponent<TGCharacter>().characterStat; //�ֿ� ĳ������ ���� ������ ��������

        // ��ġ �� ȸ�� �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public virtual void UseItem() { }

    //�θ��� �������� �޼ҵ带 �������� �ʰ� �޼ҵ� �ۼ��� �����ϰ� �ϴ� �޼ҵ��
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
