﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자 인풋을 이용하여 플레이어 캐릭터를 조종하기 위한 클래스입니다
public class TGPlayerCharacterController : MonoBehaviour
{
    // inspector
    public GameObject mainCamera;

    // 캐릭터 이동 속도 최대치 설정
    [Header("Movement Parameter")]
    public float forwardSpeed           = 7.0f;
    public float backwardSpeed          = 2.0f;
    public float sidestepSpeed          = 5.0f;
    public float moveSpeedMultiplier    = 1.0f;
    // 점프 높이
    public float jumpHeight = 3.0f;

    //private
    TGPlayerFollowMainCameraController  cameraController; // 카메라 컨트롤러 ref
    Dictionary<EKeyValues, KeyCode>     keyValuePairs;              // KeyValuePair map ref
    TGPlayerCharacter                   playerCharacter;            // 플레이어 캐릭터 ref
    MCharacterStats                     playerStats;                // 플레이어 스탯 ref
    TGGameManager                       gameManager;                // 게임 매니저 ref      
    TGEventManager                      gameEventManager;           // 이벤트 매니저 ref
    Camera                              cameraComponent;            // 카메라 컴포넌트 ref

    Vector3 targetRotation;
    Vector3 currentVel;

    float Yaxis = 0;
    float xRotation = 0;
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
    }

    private void Update()
    {
        EquipContorl();
        MoveControl();
        FollowRotationCamera();
        HandInItemFollowCameraRotation();
    }

    //Init
    void InitReferences()
    {
        cameraController    = mainCamera.GetComponent<TGPlayerFollowMainCameraController>();
        keyValuePairs       = TGPlayerKeyManager.Instance.KeyValuePairs;
        gameManager         = TGGameManager.Instance;
        playerCharacter     = GetComponent<TGPlayerCharacter>();
        playerStats         = playerCharacter.characterStat;
        gameEventManager    = TGEventManager.Instance;
        cameraComponent     = mainCamera.GetComponent<Camera>();
        
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
            playerCharacter.currnetSpeed = forwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[EKeyValues.Backward])) //뒤로 이동
        {
            playerCharacter.CommandMove(Vector3.back, backwardSpeed);
            playerCharacter.currnetSpeed = -backwardSpeed;
        }
        if (Input.GetKey(keyValuePairs[EKeyValues.Left])) //왼쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.left, sidestepSpeed);
            playerCharacter.currnetSpeed = -sidestepSpeed;
        }
        if (Input.GetKey(keyValuePairs[EKeyValues.Right])) //오른쪽으로 이동
        {
            playerCharacter.CommandMove(Vector3.right, sidestepSpeed);
            playerCharacter.currnetSpeed = -sidestepSpeed;
        }

        if (!Input.GetKey(keyValuePairs[EKeyValues.Forward]) && !Input.GetKey(keyValuePairs[EKeyValues.Backward])
            && !Input.GetKey(keyValuePairs[EKeyValues.Left]) && !Input.GetKey(keyValuePairs[EKeyValues.Right]))  //이동 키를 누르지 않을 때 속도를 서서히 감소
        {
            OnStopCharacter();
        }
    }
    public void OnStopCharacter()
    {
        playerCharacter.currnetSpeed /= 2f;
    }

    //카메라 관련 메소드
    //카메라와 같이 회전하는 메소드
    void FollowRotationCamera()
    {
        if (mainCamera != null)
        {
            // 카메라의 x축 회전값 가져오기
            float cameraRotationY = mainCamera.transform.eulerAngles.y;

            // 현재 게임 오브젝트의 회전값을 가져와서 x축 회전값만 변경
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y = cameraRotationY;

            // 새로운 회전값을 Quaternion으로 변환하여 게임 오브젝트에 적용
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }

    protected void HandInItemFollowCameraRotation()
    {
        if (mainCamera == null) return;
        if (playerCharacter.HandInItem == EEquipmentType.Default || playerCharacter.HandInItem == EEquipmentType.None) return;

        Ray ray = cameraComponent.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;

            playerCharacter.equipItems[playerCharacter.HandInItem].transform.LookAt(hitPoint); // 오브젝트가 적중 지점을 바라보도록 설정

            Debug.DrawRay(ray.origin, hit.point, Color.red);
        }
        else // 레이케스트에 적중하지 않을 경우 카메라 회전을 따라감
        {
            Vector3 cameraRotation = mainCamera.transform.localEulerAngles;
            playerCharacter.equipItems[playerCharacter.HandInItem].transform.localRotation = Quaternion.Euler(cameraRotation.x, 0f, 0f);
        }
    }

    // 아이템 관련 메소드
    void EquipContorl()
    {
        // 아이템 들기
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.Item1])) //아이템1 들기
        {
            playerCharacter.CommandChangeInHandItem(EEquipmentType.PrimaryWeapon);
        }
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.Item2])) //아이템2 들기
        {
            playerCharacter.CommandChangeInHandItem(EEquipmentType.SecondaryWeapon);
        }
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.Fire]))
        {
            playerCharacter.CommandUseInHandItem();
        }
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.Aim]))
        {
            playerCharacter.CommandAimWeaponItem();
            cameraController.EnableAim();
        }
        if (Input.GetKeyUp(keyValuePairs[EKeyValues.Aim]))
        {
            playerCharacter.CommandDisableAimWeaponItem();
            cameraController.DisableAim();
        }
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.Reload]))
        {
            playerCharacter.CommandReloadInHandItem();
        }
    }
}
