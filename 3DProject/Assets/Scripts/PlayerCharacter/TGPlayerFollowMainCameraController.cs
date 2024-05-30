using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TGPlayerFollowMainCameraController : MonoBehaviour
{
    //카메라 옵션
    [Header("Camera Parameter")]
    public float CameraHeight;
    public float Yaxis;
    public float Xaxis;

    public float CameraZoomDistanceUnit = 0.2f;
    public float MaxCameraZoomDistance;
    public float MinCameraZoomDistance;
    [Range(0, 5)]
    public float RotationSensitive = 3f;    //카메라 회전 감도
    [Range(0, 1)]
    public float SmoothTime = 0.12f;        //카메라가 회전하는데 걸리는 시간
    public float RotationMin = -10f;        //카메라 회전각도 최소
    public float RotationMax = 80f;         //카메라 회전각도 최대

    public Transform target;                //Player

    private float dist = 4f;                 //카메라와 플레이어사이의 거리

    private Vector3 cameraHeightVec3;
    private Vector3 targetRotation;
    private Vector3 currentVel;

    private Dictionary<KeyValues, KeyCode> keyValuePairs; // KeyValuePair map ref

    private bool isMouseCursorLock = false;
    private void Start()
    {
        cameraHeightVec3    = new Vector3(0, CameraHeight, 0);
        keyValuePairs       = TGPlayerKeyManager.Instance.KeyValuePairs;
        MouseCursorLockSwitch();
    }

    private void LateUpdate()
    {
        if(isMouseCursorLock)
        {
            //PlayerMainCameraControl();
            PlayerMainCameraControl2();
            PlayerMainCameraZoomControl();
        }
        PlayerCameraFollow();

        if (Input.GetKeyDown(keyValuePairs[KeyValues.MOUSECURSORSWITCH])) //MOUSECURSORSWITCH에 할당 된 키가 입력되면 호출
        {
            MouseCursorLockSwitch();
        }
    }

    void PlayerCameraFollow()
    {
        transform.position = cameraHeightVec3 + target.position - transform.forward * dist;
        //카메라의 위치는 플레이어보다 설정한 값만큼 떨어져있게 계속 변경된다.
    }

    void PlayerMainCameraControl()
    {
        Yaxis = Yaxis + Input.GetAxis("Mouse X") * RotationSensitive;//마우스 좌우움직임을 입력받아서 카메라의 Y축을 회전시킨다
        Xaxis = Xaxis - Input.GetAxis("Mouse Y") * RotationSensitive;//마우스 상하움직임을 입력받아서 카메라의 X축을 회전시킨다
        //Xaxis는 마우스를 아래로 했을때(음수값이 입력 받아질때) 값이 더해져야 카메라가 아래로 회전한다 

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);
        //X축회전이 한계치를 넘지않게 제한해준다.

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, SmoothTime);
        transform.eulerAngles = targetRotation;
        //SmoothDamp를 통해 부드러운 카메라 회전
    }

    void PlayerMainCameraControl2()
    {
        // 카메라가 따라갈 위치 계산
        Vector3 desiredPosition = target.position * dist;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothTime);
        transform.position = smoothedPosition;

        // 카메라가 항상 대상의 방향을 보도록 설정
        transform.LookAt(target.position);
    }



    void PlayerMainCameraZoomControl()
    {
        // 마우스 휠 축의 값을 가져옵니다.
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // 마우스 휠 업
        {
            dist -= dist > MinCameraZoomDistance ? CameraZoomDistanceUnit : 0;
        }
        else if (scroll < 0f) // 마우스 휠 다운
        {
            dist += dist < MaxCameraZoomDistance ? CameraZoomDistanceUnit : 0;
        }
    }

    // 마우스 커서 On/Off
    void MouseCursorLockSwitch()
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
            Cursor.visible = true;                          //마우스 커서가 보이게 않게 함
            Cursor.lockState = CursorLockMode.Confined;     //마우스 커서 고정 해제시킴
            isMouseCursorLock = false;
            Debug.Log("Mouse cursor unlock");
        }
    }
}
