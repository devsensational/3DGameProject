using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자 인풋을 이용하여 플레이어 캐릭터를 조종하기 위한 클래스입니다
public class TGPlayerCharacterController : MonoBehaviour
{
    // inspector
    public GameObject MainCamera;

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
    MCharacterStats                 playerStats;                // 플레이어 스탯 ref
    TGGameManager                   gameManager;                // 게임 매니저 ref      

    private Vector3 targetRotation;
    private Vector3 currentVel;

    float Yaxis = 0;

    //Unity lifetime
    void Start()
    {
        keyValuePairs       = TGPlayerKeyManager.Instance.KeyValuePairs; //KeyManager ref
        gameManager         = new TGGameManager();  
        playerCharacter     = GetComponent<TGPlayerCharacter>();
        playerStats         = playerCharacter.playerStat;
    }

    void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        MoveControl();
        FollowRotationCamera();
    }

    void MoveControl()    // Rigidbody와 연결되어 있기 때문에 FixedUpdate에서 호출해야 함
    {
        // 이동
        if (Input.GetKey(keyValuePairs[KeyValues.Forward])) //앞으로 이동
        {
            playerCharacter.CommandMove(Vector3.forward, forwardSpeed);
            playerStats.velocity = forwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[KeyValues.Backward])) //뒤로 이동
        {
            playerCharacter.CommandMove(Vector3.back, backwardSpeed);
            playerStats.velocity = -backwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[KeyValues.Left])) //왼쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.left, sidestepSpeed);
            playerStats.velocity = -sidestepSpeed;
        }
        if (Input.GetKey(keyValuePairs[KeyValues.Right])) //오른쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.right, sidestepSpeed);
            playerStats.velocity = -sidestepSpeed;
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

    private void FollowRotationCamera()
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
    public void OnStopCharacter()
    {
        playerStats.velocity /= 2f;
    }

}
