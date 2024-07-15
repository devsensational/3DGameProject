using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUIHitDamageText : MonoBehaviour
{
    // Insepector
    public float jumpForce = 5f;
    public float jumpInterval = 1f; // ���� ����
    public float randomForceRange = 3f; // �¿�� ������ ��

    // private
    GameObject mainCamera;
    Rigidbody2D rb;


    // Unity lifecycle
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");    
        rb = GetComponent<Rigidbody2D>();

        
        Jump();
    }

    void Update()
    {
        LookAtCamera();
    }

    void LookAtCamera() // ���ڰ� Camera�� �ٶ󺸵��� ȸ��
    {
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.x = direction.z = 0.0f; // y�� ȸ���� ����
        transform.LookAt(mainCamera.transform.position - direction);
        transform.Rotate(0, 180, 0); // �ؽ�Ʈ�� �������� �ʵ��� 180�� ȸ��
    }
    void Jump()
    {
        // �Ʒ� �������� �����ϰ� �¿�� ������ ���� ����
        Vector3 jumpDirection = Vector3.up * jumpForce;
        jumpDirection += Vector3.right * Random.Range(-randomForceRange, randomForceRange);

        rb.velocity = jumpDirection;
    }
}
