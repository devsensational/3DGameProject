using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUIHitDamageText : MonoBehaviour
{
    // Insepector
    public float jumpForce = 5f;
    public float jumpInterval = 1f; // 점프 간격
    public float randomForceRange = 3f; // 좌우로 랜덤한 힘

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

    void LookAtCamera() // 글자가 Camera를 바라보도록 회전
    {
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.x = direction.z = 0.0f; // y축 회전만 유지
        transform.LookAt(mainCamera.transform.position - direction);
        transform.Rotate(0, 180, 0); // 텍스트가 뒤집히지 않도록 180도 회전
    }
    void Jump()
    {
        // 아래 방향으로 점프하고 좌우로 랜덤한 힘을 더함
        Vector3 jumpDirection = Vector3.up * jumpForce;
        jumpDirection += Vector3.right * Random.Range(-randomForceRange, randomForceRange);

        rb.velocity = jumpDirection;
    }
}
