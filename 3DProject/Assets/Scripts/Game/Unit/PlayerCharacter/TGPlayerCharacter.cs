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
    //public List<TGItem> lootableItems = new List<TGItem>();

    // private
    // Capsule Colider ref
     CapsuleCollider col;
     Rigidbody       rb;

    //References
    TGEventManager  eventManager;
    MWeaponStats    weaponStats;

    //Unity lifecycle
    protected override void ChildAwake()
    {
        InitReferences();
        InitEvent();
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
                //lootableItems.Add(itemObject);
                eventManager.TriggerEvent(EEventType.EnterInteractiveItem, itemObject);
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
                //lootableItems.Remove(itemObject.GetComponent<TGItem>());
                eventManager.TriggerEvent(EEventType.ExitInteractiveItem, itemObject);
                Debug.Log("exit from " + itemObject.objectName);
            }
        }
    }

    //Init
    void InitReferences()
    {
        characterStat   = new MCharacterStats();
        eventManager    = TGEventManager.Instance;
    }

    void InitEvent()
    {
       
    }

    //item 관련 method
    // "TGPlayerCharacterController"에서 Item 드랍을 호출했을 때 실행
    public void CommandDropItem(EItemType itemType) 
    {
        equipItems[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    // "TGPlayerCharacterController"에서 특정 아이템을 손에 드는 명령을 내릴때 수행
    public void CommandHandInItem(EItemType itemType) 
    {
        if(handInItem != null)
        {
            ChangeHandInItem(handInItem, equipItems[itemType]);
        } 
        else
        {

        }
    }

    //손에 든 아이템을 교체할 때 사용하는 메소드
    private void ChangeHandInItem(TGItem previousItem, TGItem nextItem)
    {
        if (previousItem != null)
        {
            previousItem.enabled = false;
        }
        nextItem.enabled = true;
    }

    //땅에 
}