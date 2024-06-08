using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

// 사용자 인풋을 이용하여 플레이어 캐릭터를 조종하기 위한 클래스입니다
public class TGPlayerCharacterController : MonoBehaviour
{
    // inspector
    public GameObject MainCamera;
    [Header("Movement Parameter")]
    // 캐릭터 이동 속도 최대치 설정
    public float forwardSpeed           = 7.0f;
    public float backwardSpeed          = 2.0f;
    public float sidestepSpeed          = 5.0f;
    public float moveSpeedMultiplier    = 1.0f;
    // 점프 높이
    public float jumpHeight = 3.0f;

    //private
    Dictionary<EKeyValues, KeyCode>  keyValuePairs;      // KeyValuePair map ref
    TGPlayerCharacter               playerCharacter;    // 플레이어 캐릭터 ref
    MCharacterStats                 playerStats;        // 플레이어 스탯 ref
    TGGameManager                   gameManager;        // 게임 매니저 ref      
    TGEventManager                  gameEventManager;   // 이벤트 매니저 ref

    Vector3 targetRotation;
    Vector3 currentVel;

    float Yaxis = 0;

    //Unity lifecycle
    void Start()
    {
        InitReferences();
    }

    void FixedUpdate()
    {

    }

    void LateUpdate()
    {
        MoveControl();
        FollowRotationCamera();
    }

    //Init
    void InitReferences()
    {
        keyValuePairs       = TGPlayerKeyManager.Instance.KeyValuePairs;
        gameManager         = TGGameManager.Instance;
        playerCharacter     = GetComponent<TGPlayerCharacter>();
        playerStats         = playerCharacter.characterStat;
        gameEventManager    = TGEventManager.Instance;
    }

    void InitEventListner()
    {

    }

    //이동 관련 메소드
    void MoveControl()    // Rigidbody와 연결되어 있기 때문에 FixedUpdate에서 호출해야 함
    {
        // 이동
        if (Input.GetKey(keyValuePairs[EKeyValues.Forward])) //앞으로 이동
        {
            playerCharacter.CommandMove(Vector3.forward, forwardSpeed);
            playerStats.velocity = forwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[EKeyValues.Backward])) //뒤로 이동
        {
            playerCharacter.CommandMove(Vector3.back, backwardSpeed);
            playerStats.velocity = -backwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[EKeyValues.Left])) //왼쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.left, sidestepSpeed);
            playerStats.velocity = -sidestepSpeed;
        }
        if (Input.GetKey(keyValuePairs[EKeyValues.Right])) //오른쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.right, sidestepSpeed);
            playerStats.velocity = -sidestepSpeed;
        }

        if (!Input.GetKey(keyValuePairs[EKeyValues.Forward]) && !Input.GetKey(keyValuePairs[EKeyValues.Backward])
            && !Input.GetKey(keyValuePairs[EKeyValues.Left]) && !Input.GetKey(keyValuePairs[EKeyValues.Right]))  //이동 키를 누르지 않을 때 속도를 서서히 감소
        {
            OnStopCharacter();
        }

        // 아이템 들기
        if (Input.GetKey(keyValuePairs[EKeyValues.Item1])) //아이템1 들기
        {
            GetComponent<TGPlayerCharacter>().CommandHandInItem(EItemType.PrimaryWeapon);
        }

    }
    public void OnStopCharacter()
    {
        playerStats.velocity /= 2f;
    }

    //카메라 관련 메소드
    void FollowRotationCamera()
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
