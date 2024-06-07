using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum MoveDirection
{
    None = 0,

    Forward,
    Backward,
    Left,
    Right,

    End = 999
}

// 플레이어가 직접 조종하는 캐릭터의 스탯 및 상호작용을 위한 클래스입니다
public class TGPlayerCharacter : TGCharacter
{
    // public
    public List<TGItem> lootableItems = new List<TGItem>();

    // private
    // Capsule Colider ref
    private CapsuleCollider col;
    private Rigidbody       rb;

    //private
    TGEventManager  eventManager;
    MWeaponStats    weaponStats;
    

    //Unity lifecycle
    protected override void ChildAwake()
    {
        characterStat = new MCharacterStats();
    }

    private void Start()
    {
        eventManager = TGEventManager.Instance;
    }

    void OnTriggerEnter(Collider other) // 트리거와 충돌할 때 실행되는 메소드
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //해당 무기가 떨어져 있는지 확인
            {
                lootableItems.Add(itemObject);
                Debug.Log("enter to " + itemObject.objectName);
            }
        }
    }

    private void OnTriggerExit(Collider other) //트리거에서 빠져나올 때 실행되는 메소드
    {
        if (other.gameObject.tag == "LootableItem")
        {
            TGItem itemObject = other.transform.parent.GetComponent<TGItem>();
            if (itemObject.isDropped) //해당 무기가 떨어져 있는지 확인
            {
                lootableItems.Remove(itemObject.GetComponent<TGItem>());
                Debug.Log("exit from " + itemObject.objectName);
            }
        }
    }

    //item 관련 method
    public void CommandDropItem(ItemType itemType) // "TGPlayerCharacterController"에서 Item 드랍을 호출했을 때 실행
    {
        equipItems[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    public void CommandHandInItem(ItemType itemType) // "TGPlayerCharacterController"에서 특정 아이템을 손에 드는 명령을 내릴때 수행
    {
        if(handInItem != null)
        {
            ChangeHandInItem(handInItem, equipItems[itemType]);
        } 
        else
        {

        }
    }

    private void ChangeHandInItem(TGItem previousItem, TGItem nextItem)
    {
        if (previousItem != null)
        {
            previousItem.enabled = false;
        }
        nextItem.enabled = true;
    }
}