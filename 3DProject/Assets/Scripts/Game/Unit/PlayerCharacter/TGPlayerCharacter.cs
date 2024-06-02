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

public class TGPlayerCharacter : TGUnit
{
    // inspector
    public GameObject MainCamera;

    // public
    public MPlayerCharacterStats playerStat { get; set; } // 플레이어 캐릭터 스탯

    // private
    // Capsule Colider ref
    private CapsuleCollider col;
    private Rigidbody       rb;

    //private
    TGItem handInItem = null;   //플레이어 캐릭터가 들고 있는 아이템 ref

    //Unity lifetime
    protected override void ChildAwake()
    {
        playerStat = new MPlayerCharacterStats();
    }

    void OnCollisionStay(Collision collision) // collision과 충돌할 때 실행되는 메소드
    {
        Debug.Log("collisionEnter " + collision.gameObject.name);
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
        inventory[itemType].OnDropThisItem();
        DropItem(itemType);
    }

    public void CommandHandInItem(ItemType itemType) // "TGPlayerCharacterController"에서 특정 아이템을 손에 드는 명령을 내릴때 수행
    {
        if(handInItem != null)
        {
            ChangeHandInItem(handInItem, inventory[itemType]);
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

    // 이동 관련 method

    public void FollowRotationCamera()
    {
        if (MainCamera != null)
        {
            // 카메라의 x축 회전값 가져오기
            float cameraRotationY = MainCamera.transform.eulerAngles.y;

            // 현재 게임 오브젝트의 회전값을 가져와서 x축 회전값만 변경
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y = cameraRotationY;

            // 새로운 회전값을 Quaternion으로 변환하여 게임 오브젝트에 적용
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}
