using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 캐릭터의 애니메이션 컨트롤을 위한 클래스입니다
public class TGCharacterAnimation : MonoBehaviour
{
    //public
    // 애니메이션 재생 속도 조절
    public float animSpeed = 1.5f;
    //private
    //ref
    private TGCharacter character;
    private MCharacterStats playerStats;

    private CapsuleCollider col;
    private Rigidbody rb;
    private Animator anim;   // 캐릭터 애니메이션 ref

    // CapsuleCollider에서 설정된 Collider의 Heiht, Center의 초기값을 담는 변수
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private AnimatorStateInfo currentBaseState;                   // base layer에서 사용되는 애니메이터의 현재 상태 참조

    //private TGEventManager eventManager;


    //Unity lifecycle
    void Awake()
    {
        InitReferences();
        InitAnimations();
    }

    private void Start()
    {
        playerStats = character.characterStat;
    }

    void FixedUpdate()
    {
        OnMoveAnimation();
    }
    //Init
    void InitReferences()
    {
        character = GetComponent<TGCharacter>();
    }

    void InitAnimations() //animation 관련 method
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        // CapsuleCollider 컴포넌트의 Height, Center의 초기값 저장하기
        orgColHight = col.height;
        orgVectColCenter = col.center;

    }
    void OnMoveAnimation()
    {
        anim.SetFloat("Speed", character.currnetSpeed);           // Animator 측에서 설정한 "Speed" 파라미터에 v를 전달
        //anim.SetFloat("Direction", h);                        // Animator 측에서 설정한 "Direction" 파라미터에 h를 전달
        anim.speed = animSpeed;                                 // Animator 모션 재생 속도에 animSpeed 설정하기
        rb.useGravity = true;                                   // 점프하는 동안 중력 차단, 그 외의 상황에서는 중력의 영향을 받도록 함

        if (character.currnetSpeed < 0.1f)
        {
            character.currnetSpeed = 0;
        }
    }

    public void EnableUpperBody()
    {
        anim.SetLayerWeight(1, 1);
    }

    public void DisableUpperBody()
    {
        anim.SetLayerWeight(1, 0);
    }

    public void OnReloadAnimation(object parameter)
    {
        Debug.Log("(TGCharacterAnimation:OnReloadAnimation) Start reload animation");
        anim.SetTrigger("OnReload");
    }

    public void OnDeadAnimation(object parameter)
    {
        anim.SetTrigger("OnDead");
    }

    public void OnFireAnimation(object parameter)
    {
        anim.SetTrigger("OnFire");
    }
}
