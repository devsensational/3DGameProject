using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGPlayerCharacterController : MonoBehaviour
{
    //public
    [Header("Movement Parameter")]
    // 캐릭터 이동 속도 최대치 설정
    public float forwardSpeed = 7.0f;
    public float backwardSpeed = 2.0f;
    public float sidestepSpeed = 5.0f;
    public float moveSpeedMultiplier = 1.0f;
    // 점프 높이
    public float jumpHeight = 3.0f;

    //private
    Dictionary<KeyValues, KeyCode>  keyValuePairs;              // KeyValuePair map ref
    TGPlayerCharacter               playerCharacter;            // 플레이어 캐릭터 ref
    MPlayerCharacterStats           playerStats;                // 플레이어 스탯 ref
    
    //Unity lifetime
    void Start()
    {
        keyValuePairs               = TGPlayerKeyManager.Instance.KeyValuePairs; //KeyManager ref
        playerCharacter             = GetComponent<TGPlayerCharacter>();
        playerStats                 = playerCharacter.playerStat;
    }

    void FixedUpdate()
    {
        MoveControl();
    }

    private void LateUpdate()
    {

    }


    void MoveControl()    // Rigidbody와 연결되어 있기 때문에 FixedUpdate에서 호출해야 함
    { 
        // 카메라 방향와 캐릭터 방향 동기화
        if (Input.anyKey)
        {
            playerCharacter.FollowRotationCamera();
        }

        // 이동
        if (Input.GetKey(keyValuePairs[KeyValues.Forward])) //앞으로 이동
        {
            playerCharacter.CommandMove(Vector3.forward, forwardSpeed);
            playerStats.velocity = forwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[KeyValues.Backward])) //뒤로 이동
        {
            playerCharacter.CommandMove(Vector3.back, forwardSpeed);
            playerStats.velocity = -backwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[KeyValues.Left])) //왼쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.left, forwardSpeed);
            playerStats.velocity = sidestepSpeed;
        }
        if (Input.GetKey(keyValuePairs[KeyValues.Right])) //오른쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.right, forwardSpeed);
            playerStats.velocity = sidestepSpeed;
        }

        if (!Input.GetKey(keyValuePairs[KeyValues.Forward]) && !Input.GetKey(keyValuePairs[KeyValues.Backward])
            && !Input.GetKey(keyValuePairs[KeyValues.Left]) && !Input.GetKey(keyValuePairs[KeyValues.Right]))  //이동 키를 누르지 않을 때 속도를 서서히 감소
        {
            OnStopCharacter();
        }

        // 아이템 들기
        if (Input.GetKey(keyValuePairs[KeyValues.Item1])) //아이템1 들기
        {
            GetComponent<TGPlayerCharacter>().CommandHandInItem(ItemType.PrimaryWeapon);
        }

    }

    public void OnStopCharacter()
    {
        playerStats.velocity /= 2f;
    }

}
