using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGPlayerCharacterMoveController : MonoBehaviour
{
    // 애니메이션 재생 속도 조절
    public float animSpeed = 1.5f;
    public GameObject MainCamera;
    [Header("Movement Parameter")]
    // 캐릭터 이동 속도 최대치 설정
    public float forwardSpeed           = 7.0f;
    public float backwardSpeed          = 2.0f;
    public float sidestepSpeed          = 5.0f;
    public float moveSpeedMultiplier    = 1.0f;
    //점프 높이
    public float jumpHeight = 3.0f;

    // Capsule Colider ref
    private CapsuleCollider col;
    private Rigidbody rb;

    // 캐릭터 컨트롤러(Capsule Colider)의 이동량
    private float velocity = 0;

    // CapsuleCollider에서 설정된 Collider의 Heiht, Center의 초기값을 담는 변수
    private float   orgColHight;
    private Vector3 orgVectColCenter;

    private Animator            anim;                               // 캐릭터 애니메이션 ref
    private AnimatorStateInfo   currentBaseState;                   // base layer에서 사용되는 애니메이터의 현재 상태 참조

    private GameObject cameraObject;                                // Main camera ref

    private Dictionary<KeyValues, KeyCode>  keyValuePairs;           // KeyValuePair map ref

    // 애니메이터 Ref
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");

    void Start()
    {
        anim            = GetComponent<Animator>();
        col             = GetComponent<CapsuleCollider>();
        rb              = GetComponent<Rigidbody>();
        cameraObject    = GameObject.FindWithTag("MainCamera");

        // CapsuleCollider 컴포넌트의 Height, Center의 초기값 저장하기
        orgColHight      = col.height;
        orgVectColCenter = col.center;

        //KeyManager ref
        keyValuePairs = TGPlayerKeyManager.Instance.KeyValuePairs;
    }

    void FixedUpdate()
    {
        PlayerCharacterControl();
    }

    private void LateUpdate()
    {

    }

    // Rigidbody와 연결되어 있기 때문에 FixedUpdate에서 호출해야 함
    void PlayerCharacterControl()
    {
        anim.SetFloat("Speed", velocity);                              // Animator 측에서 설정한 "Speed" 파라미터에 v를 전달
        //anim.SetFloat("Direction", h);                          // 애니메이터 측에서 설정한 "Direction" 파라미터에 h를 전달
        anim.speed = animSpeed;                                 // 애니메이터의 모션 재생 속도에 animSpeed 설정하기
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 참조용 상태 변수에 Base Layer의 현재 상태(0)를 설정
        rb.useGravity = true;                                   // 점프하는 동안 중력 차단, 그 외의 상황에서는 중력의 영향을 받도록 함

        if(velocity > 0.1f)
        {

        }
        else if(velocity < 0.1f)
        {
            velocity = 0;
        }

        if (Input.anyKey)
        {
            PlayerCharacterFollowRotationCamera();
        }

        if (Input.GetKey(keyValuePairs[KeyValues.FORWARD])) //앞으로 이동
        {
            velocity = forwardSpeed * moveSpeedMultiplier;
            UnitMoveControl(Vector3.forward, forwardSpeed);
        }
        if (Input.GetKey(keyValuePairs[KeyValues.BACKWARD])) //뒤로 이동
        {
            velocity = -(backwardSpeed * moveSpeedMultiplier);
            UnitMoveControl(Vector3.back, backwardSpeed);
        }
        if (Input.GetKey(keyValuePairs[KeyValues.LEFT])) //왼쪽으로 이동
        {
            velocity = sidestepSpeed * moveSpeedMultiplier;
            UnitMoveControl(Vector3.left, sidestepSpeed);
        }
        if (Input.GetKey(keyValuePairs[KeyValues.RIGHT])) //오른쪽으로 이동
        {
            velocity = sidestepSpeed * moveSpeedMultiplier;
            UnitMoveControl(Vector3.right, sidestepSpeed);
        }

        //이동 키를 누르지 않을 때 속도를 서서히 감소
        if (!Input.GetKey(keyValuePairs[KeyValues.FORWARD]) && !Input.GetKey(keyValuePairs[KeyValues.BACKWARD])
            && !Input.GetKey(keyValuePairs[KeyValues.LEFT]) && !Input.GetKey(keyValuePairs[KeyValues.RIGHT]))
        {
            velocity /= 2f;
            
        }
    }

    void PlayerCharacterFollowRotationCamera()
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

    void UnitMoveControl(Vector3 direction, float moveSpeed)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
