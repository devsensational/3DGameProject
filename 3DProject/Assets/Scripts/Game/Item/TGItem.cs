using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    DEFAULT = 0,

    //���⼭ ���� enum �ۼ�
    PRIMARYWEAPON,
    SECONDARYWEAPON,
    //

    END = 999
}

public class TGItem : TGObject
{
    //public
    public ItemType     itemType;
    public GameObject   itemHolder { get; set; } //�������� �ֿ� ������Ʈ
    public bool         isDropped = false; //�������� �ֿ� �� �ִ� �������� Ȯ��

    //protected
    protected bool isHandIn = false;

    //private

    private void Awake()
    {
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


    public void OnDropThisItem()    // �������� �������� �� ȣ��
    {
        itemHolder = null;
        isDropped = true;
    }


    public void OnPickedUpThisItem(GameObject gameObject)    // �������� �ֿ����� �� ȣ��
    {
        itemHolder = gameObject;
        isDropped = false;
        transform.SetParent(gameObject.transform.Find("HandInItemPos"));
        GetComponent<Rigidbody>().useGravity = false;
    }

    //�θ��� �������� �޼ҵ带 �������� �ʰ� �޼ҵ� �ۼ��� �����ϰ� �ϴ� �޼ҵ��
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
