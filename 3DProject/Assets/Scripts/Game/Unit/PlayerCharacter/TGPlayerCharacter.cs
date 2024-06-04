using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public MCharacterStats playerStat { get; set; } // 플레이어 캐릭터 스탯

    // private
    // Capsule Colider ref
    private CapsuleCollider col;
    private Rigidbody       rb;

    //private
    TGItem handInItem = null;   //플레이어 캐릭터가 들고 있는 아이템 ref

    //Unity lifetime
    protected override void ChildAwake()
    {
        playerStat = new MCharacterStats();
    }

    void OnCollisionStay(Collision collision) // collision과 충돌할 때 실행되는 메소드
    {
        if (collision.gameObject.tag == "Item")
        {
            if (collision.gameObject.GetComponent<TGItem>().isDropped) //해당 무기가 떨어져 있는지 확인
            {
                TakeItem(collision.gameObject.GetComponent<TGItem>());
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
