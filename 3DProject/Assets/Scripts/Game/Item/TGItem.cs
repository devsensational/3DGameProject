using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    DEFAULT = 0,

    //여기서 부터 enum 작성
    PRIMARYWEAPON,
    SECONDARYWEAPON,
    //

    END = 999
}

public class TGItem : TGObject
{
    //public
    public ItemType     itemType;
    public GameObject   itemHolder { get; set; } //아이템을 주운 오브젝트
    public bool         isDropped = false; //아이템이 주울 수 있는 상태인지 확인

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


    public void OnDropThisItem()    // 아이템이 버려졌을 때 호출
    {
        itemHolder = null;
        isDropped = true;
    }


    public void OnPickedUpThisItem(GameObject gameObject)    // 아이템이 주워졌을 때 호출
    {
        itemHolder = gameObject;
        isDropped = false;
        transform.SetParent(gameObject.transform.Find("HandInItemPos"));
        GetComponent<Rigidbody>().useGravity = false;
    }

    //부모의 오리지날 메소드를 수정하지 않고 메소드 작성을 가능하게 하는 메소드들
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
