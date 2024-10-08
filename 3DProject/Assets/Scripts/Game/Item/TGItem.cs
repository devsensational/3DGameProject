using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현 프로젝트의 Item을 정의하는 최상위 클래스입니다.
public class TGItem : TGObject
{
    //Inspector
    public GameObject itemModel;
    public string groundTag = "Terrain"; // 바닥에 사용할 태그

    public int itemCount = 0; //아이템의 갯수

    //public
    public TGUILootableItemInterective  itemButton;
    public EEquipmentType               equipmentType = EEquipmentType.None;
    public EItemType                    itemType;

    public TGCharacter  itemHolder { get; set; }    //아이템을 주운 오브젝트

    public bool         isDropped = false;          //아이템이 주울 수 있는 상태인지 확인

    public MCharacterStats characterStats { get; set; }

    //protected
    protected bool isHandIn = false;


    //private
    float maxRayDistance = 100f; // Raycast의 최대 거리

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

    private void OnDestroy()
    {
        if(itemButton != null)
        {
            Destroy(itemButton.gameObject);
        }
    }

    // 아이템 상호작용 관련 메소드
    public void OnDropThisItem()    // 아이템이 버려졌을 때 호출
    {
        itemHolder = null;          //해당 아이템 소지자 초기화
        isDropped = true;           //떨어져 있는 상태로 변경
        isHandIn = false;   
        transform.SetParent(null);  //부모 해제
        itemModel.SetActive(true);

        DropToGround();

        //cl.isTrigger = false;
        //rb.isKinematic = false;
    }


    public void OnPickedUpThisItem(GameObject pickedUpCharacterObject)    // 아이템이 주워졌을 때 호출
    {
        itemHolder = pickedUpCharacterObject.GetComponent<TGCharacter>();
        isDropped = false;

        TGCharacter chracterComponent = itemHolder.GetComponent<TGCharacter>();

        // 부모 설정
        transform.SetParent(chracterComponent.HandPosition.transform);
        characterStats = chracterComponent.characterStat; //주운 캐릭터의 스탯 데이터 가져오기

        // 위치 및 회전 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if(chracterComponent.GetInHandItem() != equipmentType)
        {
            itemModel.SetActive(false);
        }

    }

    public void OnHandInThisItem() // 캐릭터가 해당 아이템을 손에 들었을 때 호출
    {
        if(isHandIn)
        {
            isHandIn = false;
            itemModel.SetActive(false);
            Debug.Log($"(TGItem:OnHandInThisItem) {objectName} hand off from {itemHolder.name}");
        }
        else
        {
            isHandIn = true;
            itemModel.SetActive(true);
            Debug.Log($"(TGItem:OnHandInThisItem) {objectName} on hand to {itemHolder.name}");
        }
    }

    public void UpdateButtonUI()     // 해당 아이템에 해당하는 UI 버튼 갱신
    {
        if (itemButton == null) return;

        itemButton.SetItemName();
        Debug.Log($"(TGItem:OnHandInThisItem) Updated item button UI");
    }

    void DropToGround()     // 아이템을 땅위에 눕혀서 드랍시킬 때 사용하는 메소드
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, maxRayDistance);

        if (hits.Length > 0)
        {
            bool groundFound = false;
            foreach (RaycastHit hit in hits)
            {
                // Raycast가 바닥에 닿았는지 확인
                if (hit.collider.CompareTag(groundTag))
                {
                    // 바닥에 닿았을 경우 오브젝트의 위치를 바닥의 위치로 변경
                    Vector3 newPosition = hit.point;
                    transform.position = newPosition;

                    // 오브젝트를 X축으로 90도 회전
                    transform.rotation = Quaternion.Euler(0, 0, 90);

                    Debug.Log("(TGItem:DropToGround) Object dropped to ground at position: " + newPosition);
                    groundFound = true;
                    break; // groundTag를 가진 객체를 찾았으므로 루프를 종료합니다
                }
            }

            if (!groundFound)
            {
                Debug.Log("(TGItem:DropToGround) Raycast did not hit an object with the tag: " + groundTag);
            }
        }
        else
        {
            Debug.Log("(TGItem) Raycast did not hit anything.");
        }
    }

    public void ReduceItemCount(int value) // 아이템 카운트를 외부로 부터 감소시킬 때 사용
    {
        if (itemCount >= 1)
        {
            itemCount -= value;
            if (itemCount <= 0) 
            { 
                Destroy(gameObject); 
            }
        }
    }

    public virtual void UseItem() 
    {
        Debug.Log("(TGItem:UseItem) Use this item: " + objectName);
    }

    public virtual void EnableAim()
    {

    }

    public virtual void DisableAim()
    {

    }

    protected virtual IEnumerator Reload()
    {
        yield return null;
    }

    //부모의 오리지날 메소드를 수정하지 않고 메소드 작성을 가능하게 하는 메소드들
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
