using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default = 0,

    //여기서 부터 enum 작성
    PrimaryWeapon,
    SecondaryWeapon,
    //

    End = 999
}

public class TGItem : TGObject
{
    //public
    public ItemType     itemType;
    public GameObject   itemHolder { get; set; } //아이템을 주운 오브젝트
    public bool         isDropped = false; //아이템이 주울 수 있는 상태인지 확인

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

    // 아이템 상호작용 관련 메소드
    public void OnDropThisItem()    // 아이템이 버려졌을 때 호출
    {
        itemHolder = null;
        isDropped = true;
    }


    public void OnPickedUpThisItem(GameObject pickedUpCharacterObject)    // 아이템이 주워졌을 때 호출
    {
        itemHolder = pickedUpCharacterObject;
        isDropped = false;

        //아이템을 손에 들었을 때 물리 연산이 되지 않도록 설정
        cl.isTrigger = true;
        rb.isKinematic = true;

        // 부모 설정
        transform.SetParent(pickedUpCharacterObject.GetComponent<TGCharacter>().HandObject.transform);
        characterStats = pickedUpCharacterObject.GetComponent<TGCharacter>().characterStat; //주운 캐릭터의 스탯 데이터 가져오기

        // 위치 및 회전 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public virtual void UseItem() { }

    //부모의 오리지날 메소드를 수정하지 않고 메소드 작성을 가능하게 하는 메소드들
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
