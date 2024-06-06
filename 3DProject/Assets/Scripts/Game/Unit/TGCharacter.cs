using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현 프로젝트의 유닛을 정의하기 위한 클래스입니다.
public class TGCharacter : TGObject
{
    //Inspector
    public GameObject HandObject;   //아이템을 주웠을 때 위치를 결정할 GameObject

    //public
    public MCharacterStats characterStat { get; protected set; } // 플레이어 캐릭터 스탯

    //protected
    protected Dictionary<ItemType, TGItem> equipItems = new Dictionary<ItemType, TGItem>();

    protected TGItem handInItem = null;   //플레이어 캐릭터가 들고 있는 아이템 ref

    //private
    
    
    //Unity lifecycle
    private void Awake()
    {
        equipItems.Add(ItemType.PrimaryWeapon, null);
        equipItems.Add(ItemType.SecondaryWeapon, null);

        ChildAwake();
    }

    // item 상호작용 관련 메소드
    protected void TakeItem(TGItem item)    // item 습득 시도 메소드
    {
        if(item == null) return;                        // 선택된 item object가 null일 경우 메소드 종료

        equipItems[item.itemType] = item;
        equipItems[item.itemType].OnPickedUpThisItem(gameObject);        // Item이 습득 됐을 때 item instance가 실행되야 할 명령 수행
        Debug.Log(gameObject.name + " picked up " + item.name);
    }

    protected void DropItem(ItemType itemType)  // item 드랍 시도 메소드
    {
        equipItems[itemType] = null;
    }

    // 이동 관련 메소드
    public void CommandMove(Vector3 direction, float moveSpeed)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // getter/setter
    public TGItem GetHandInItem()
    {
        return handInItem;
    }

    // child
    protected virtual void ChildAwake() {}

}
