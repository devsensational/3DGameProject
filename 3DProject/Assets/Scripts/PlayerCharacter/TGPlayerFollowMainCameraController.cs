using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TGPlayerFollowMainCameraController : MonoBehaviour
{
    //ī�޶� �ɼ�
    [Header("Camera Parameter")]

    public float CameraHeight;
    public float Yaxis;
    public float Xaxis;

    public Transform target;//Player

    private float rotSensitive = 3f;//ī�޶� ȸ�� ����
    private float dis = 4f;//ī�޶�� �÷��̾������ �Ÿ�
    private float RotationMin = -10f;//ī�޶� ȸ������ �ּ�
    private float RotationMax = 80f;//ī�޶� ȸ������ �ִ�
    private float smoothTime = 0.12f;//ī�޶� ȸ���ϴµ� �ɸ��� �ð�

    private Vector3 cameraHeightVec3;
    private Vector3 targetRotation;
    private Vector3 currentVel;

    private void Start()
    {
        cameraHeightVec3 = new Vector3(0, CameraHeight, 0);
    }
    private void LateUpdate()
    {
        PlayerMainCameraControl();
    }

    void PlayerMainCameraControl()
    {
        Yaxis = Yaxis + Input.GetAxis("Mouse X") * rotSensitive;//���콺 �¿�������� �Է¹޾Ƽ� ī�޶��� Y���� ȸ����Ų��
        Xaxis = Xaxis - Input.GetAxis("Mouse Y") * rotSensitive;//���콺 ���Ͽ������� �Է¹޾Ƽ� ī�޶��� X���� ȸ����Ų��
        //Xaxis�� ���콺�� �Ʒ��� ������(�������� �Է� �޾�����) ���� �������� ī�޶� �Ʒ��� ȸ���Ѵ� 

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);
        //X��ȸ���� �Ѱ�ġ�� �����ʰ� �������ش�.

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation;
        //SmoothDamp�� ���� �ε巯�� ī�޶� ȸ��

        transform.position = cameraHeightVec3 + target.position - transform.forward * dis;
        //ī�޶��� ��ġ�� �÷��̾�� ������ ����ŭ �������ְ� ��� ����ȴ�.
    }
}
