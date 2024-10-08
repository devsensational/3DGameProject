﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TGPlayerFollowMainCameraController : MonoBehaviour
{
    //public
    //카메라 옵션
    [Header("Camera Parameter")]
    public float cameraHeight;                      //카메라 높이

    public float cameraCurrentDistance = 0.5f;      //카메라와 플레이어사이의 거리
    public float cameraZoomDistanceUnit = 0.2f;     //카메라 줌 확대/축소 시 이동 거리
    public float maxCameraZoomDistance;             //카메라 줌 최대 거리
    public float minCameraZoomDistance;             //카메라 줌 최소 거리
    [Range(0, 100)]
    public float cameraRotationSensitive = 40f;      //카메라 회전 감도
    [Range(0, 1)]
    public float smoothTime = 0.12f;                //카메라가 회전하는데 걸리는 시간
    public float rotationMin = -10f;                //카메라 회전각도 최소
    public float rotationMax = 90f;                 //카메라 회전각도 최대

    public Transform target;                        //플레이어 캐릭터 ref
    public Transform FPPTarget;                     //1인칭 타겟

    //private
    Dictionary<EKeyValues, KeyCode> keyValuePairs; // KeyValuePair map ref

    TGEventManager eventManager;

    Vector3 cameraHeightVec3;
    Vector3 currentRotation; // 현재 카메라의 회전 값
    Vector3 targetRotation;
    Vector3 currentVel;

    float yAxis;
    float xAxis;

    bool isMouseCursorLock  = false;
    bool isTPP              = true;


    //Unity lifecycle
    private void Start()
    {
        InitReferences();
        InitEvent();
        ToggleMouseCursorLock(null); //게임 시작 시 마우스 커서 자동으로 잠금
    }

    private void Update()
    {
        if(isMouseCursorLock) //마우스 커서가 잠겨있을 때만 카메라 회전을 허용
        {
            PlayerMainCameraControl();
            if(isTPP)
            {
                PlayerMainCameraZoomControl();
            }
        }
        if (isTPP)
        {
            PlayerCameraFollow();
        }

        if (Input.GetKeyDown(keyValuePairs[EKeyValues.MouseCursorSwitch])) // MOUSECURSORSWITCH에 할당 된 키가 입력되면 호출
        {
            ToggleMouseCursorLock(null);
        }
        if (Input.GetKeyDown(keyValuePairs[EKeyValues.ToggleCameraView]))
        {
            SwitchCameraPerspective();
        }

    }

    //Init
    void InitReferences()
    {
        cameraHeightVec3    = new Vector3(0, cameraHeight, 0);
        keyValuePairs       = TGPlayerKeyManager.Instance.KeyValuePairs;
        eventManager        = TGEventManager.Instance;
    }

    void InitEvent()
    {
        eventManager.StartListening(EEventType.ToggleInventoryUI, ToggleMouseCursorLock);
    }

    // 카메라 컨트롤 관련 메소드
    void PlayerCameraFollow()
    {
        transform.position = cameraHeightVec3 + target.position - transform.forward * cameraCurrentDistance; // 카메라 위치 갱신, "dist"에 따라 거리 조절

    }

    void PlayerMainCameraControl()
    {
        /* yAxis += Input.GetAxis("Mouse X") * cameraRotationSensitive * 10 * Time.deltaTime; // 마우스 좌우움직임을 입력받아서 카메라의 Y축을 회전
         xAxis -= Input.GetAxis("Mouse Y") * cameraRotationSensitive * 10 * Time.deltaTime; // 마우스 상하움직임을 입력받아서 카메라의 X축을 회전

         xAxis = Mathf.Clamp(xAxis, rotationMin, rotationMax); // X축회전 제한

         targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis), ref currentVel, smoothTime);
         transform.eulerAngles = targetRotation;*/

        xAxis = Input.GetAxis("Mouse X") * cameraRotationSensitive * 10 * Time.deltaTime;
        yAxis = Input.GetAxis("Mouse Y") * cameraRotationSensitive * 10 * Time.deltaTime;

        targetRotation.y += xAxis;
        targetRotation.x -= yAxis;
        targetRotation.x = Mathf.Clamp(targetRotation.x, rotationMin, rotationMax);

        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref currentVel, smoothTime);
        transform.eulerAngles = currentRotation;
    }

    void PlayerMainCameraZoomControl()
    {
        // 마우스 휠 축의 값을 가져옵니다.
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // 마우스 휠 업
        {
            cameraCurrentDistance -= cameraCurrentDistance > minCameraZoomDistance ? cameraZoomDistanceUnit : 0;
        }
        else if (scroll < 0f) // 마우스 휠 다운
        {
            cameraCurrentDistance += cameraCurrentDistance < maxCameraZoomDistance ? cameraZoomDistanceUnit : 0;
        }
    }

    //TPP/FPP 변경
    void SwitchCameraPerspective()
    {
        if(isTPP)
        {
            transform.SetParent(FPPTarget);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(null);
        }
        isTPP = !isTPP;
    }

    // 마우스 커서 On/Off
    void ToggleMouseCursorLock(object parameters)
    {
        if (!isMouseCursorLock)
        {
            Cursor.visible = false;                         //마우스 커서가 보이지 않게 함
            Cursor.lockState = CursorLockMode.Locked;       //마우스 커서를 고정시킴
            isMouseCursorLock = true;
            Debug.Log("Mouse cursor lock");
        }
        else
        {
            Cursor.visible = true;                          //마우스 커서가 보이게 함
            Cursor.lockState = CursorLockMode.Confined;     //마우스 커서 고정 해제시킴
            isMouseCursorLock = false;
            Debug.Log("Mouse cursor unlock");
        }
    }

    // 조준 관련 메소드
    public void EnableAim()
    {
        cameraCurrentDistance = minCameraZoomDistance;
    }

    public void DisableAim()
    {
        cameraCurrentDistance = maxCameraZoomDistance;
    }

    // 반동 관련 메소드
    public void ApplyRecoil(Vector3 recoilVector)
    {
        targetRotation = targetRotation + recoilVector;
    }

    private void RecoverRecoil()
    {

    }
}
