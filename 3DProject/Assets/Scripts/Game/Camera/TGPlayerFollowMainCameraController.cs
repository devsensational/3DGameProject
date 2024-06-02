using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TGPlayerFollowMainCameraController : MonoBehaviour
{
    //public
    //카메라 옵션
    [Header("Camera Parameter")]
    public float CameraHeight;
    public float Yaxis;
    public float Xaxis;

    public float CameraZoomDistanceUnit = 0.2f;
    public float MaxCameraZoomDistance;
    public float MinCameraZoomDistance;
    [Range(0, 5)]
    public float CameraRotationSensitive = 3f;      //카메라 회전 감도
    [Range(0, 1)]
    public float SmoothTime = 0.12f;                //카메라가 회전하는데 걸리는 시간
    public float RotationMin = -10f;                //카메라 회전각도 최소
    public float RotationMax = 90f;                 //카메라 회전각도 최대

    public Transform target;                //Player

    //private
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
        if(isMouseCursorLock) //마우스 커서가 잠겨있을 때만 카메라 회전을 허용
        {
            PlayerMainCameraControl();
            PlayerMainCameraZoomControl();
        }
        PlayerCameraFollow();

        if (Input.GetKeyDown(keyValuePairs[KeyValues.Mousecursorswitch])) // MOUSECURSORSWITCH에 할당 된 키가 입력되면 호출
        {
            MouseCursorLockSwitch();
        }
    }

    void PlayerCameraFollow()
    {
        transform.position = cameraHeightVec3 + target.position - transform.forward * dist; // 카메라 위치 갱신, "dist"에 따라 거리 조절

    }

    void PlayerMainCameraControl()
    {
        Yaxis += Input.GetAxis("Mouse X") * CameraRotationSensitive; // 마우스 좌우움직임을 입력받아서 카메라의 Y축을 회전
        Xaxis -= Input.GetAxis("Mouse Y") * CameraRotationSensitive; // 마우스 상하움직임을 입력받아서 카메라의 X축을 회전

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax); // X축회전 제한

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, SmoothTime);
        transform.eulerAngles = targetRotation;
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
            Cursor.visible = true;                          //마우스 커서가 보이게 함
            Cursor.lockState = CursorLockMode.Confined;     //마우스 커서 고정 해제시킴
            isMouseCursorLock = false;
            Debug.Log("Mouse cursor unlock");
        }
    }
}
