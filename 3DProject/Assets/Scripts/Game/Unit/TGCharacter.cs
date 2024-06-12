using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// 현 프로젝트의 유닛을 정의하기 위한 클래스입니다.
public class TGCharacter : TGObject
{
    //Inspector
    public GameObject HandPosition;   //아이템을 주웠을 때 위치를 결정할 GameObject

    //public
    public MCharacterStats characterStat { get; protected set; } // 플레이어 캐릭터 스탯

    public Dictionary<EItemType, TGItem> equipItems = new Dictionary<EItemType, TGItem>();

    public List<TGItem> inventory = new List<TGItem>(); // 주운 아이템 리스트

    //protected
    protected EItemType handInItem = EItemType.Default;   //플레이어 캐릭터가 들고 있는 아이템 type
    protected TGEventManager eventManager;  //이벤트매니저
    //private


    //Unity lifecycle
    private void Awake()
    {
        equipItems.Add(EItemType.Default, null);
        equipItems.Add(EItemType.PrimaryWeapon, null);
        equipItems.Add(EItemType.SecondaryWeapon, null);

        ChildAwake();
        InitEvent();
    }

    private void OnDestroy()
    {
        ChildOnDestroy();   
    }
    //Init
    protected virtual void InitReferences()
    {
        eventManager = TGEventManager.Instance;
    }

    protected virtual void InitEvent()
    {

    }

    // item 상호작용 관련 메소드
    protected void TakeItem(TGItem item)    // item 습득 시도 메소드
    {
        if (item == null) return;                        // 선택된 item object가 null일 경우 메소드 종료
        if(equipItems[item.itemType] != null)
        {
            eventManager.TriggerEvent(EEventType.UIDropItemFromInventory, equipItems[item.itemType]);
            DropItem(item.itemType);
        }

        equipItems[item.itemType] = item;
        equipItems[item.itemType].OnPickedUpThisItem(gameObject);        // Item이 습득 됐을 때 item instance가 실행되야 할 명령 수행
        Debug.Log(gameObject.name + " picked up " + item.name);
    }

    protected void DropItem(EItemType itemType)  // item 드랍 시도 메소드
    {
        equipItems[itemType].OnDropThisItem();
        equipItems[itemType] = null;
        if(handInItem == itemType)
        {
            handInItem = EItemType.Default;
        }
    }

    // 이동 관련 메소드
    public void CommandMove(Vector3 direction, float moveSpeed)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    //손에 든 아이템을 교체할 때 사용하는 메소드
    protected void ChangeHandInItem(TGItem previousItem, TGItem nextItem)
    {
        if (previousItem != null)
        {
            previousItem.OnHandInThisItem();
        }
        if (nextItem != null)
        {
            nextItem.OnHandInThisItem();
            handInItem = nextItem.itemType;
        }
    }

    // getter/setter
    public EItemType GetHandInItem()
    {
        return handInItem;
    }

    // child
    protected virtual void ChildAwake() {}
    protected virtual void ChildOnDestroy() { }
}
