using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 캐릭터의 애니메이션 컨트롤을 위한 클래스입니다
public class TGPlayerCharacterAnimation : MonoBehaviour
{
    //public
    // 애니메이션 재생 속도 조절
    public float animSpeed = 1.5f;
    //private
    //ref
    private TGPlayerCharacter       playerCharacter;
    private MCharacterStats   playerStats;

    private CapsuleCollider     col;
    private Rigidbody           rb;
    private Animator            anim;   // 캐릭터 애니메이션 ref

    // CapsuleCollider에서 설정된 Collider의 Heiht, Center의 초기값을 담는 변수
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private AnimatorStateInfo currentBaseState;                   // base layer에서 사용되는 애니메이터의 현재 상태 참조


    //Unity lifetime
    void Start()
    {
        playerCharacter = GetComponent<TGPlayerCharacter>();
        playerStats     = playerCharacter.playerStat;
        AnimationInit();
    }

    void FixedUpdate()
    {
        OnMoveAnimation();
    }

    //animation 관련 method
    private void AnimationInit()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        // CapsuleCollider 컴포넌트의 Height, Center의 초기값 저장하기
        orgColHight = col.height;
        orgVectColCenter = col.center;

    }
    private void OnMoveAnimation()
    {
        anim.SetFloat("Speed", playerStats.velocity);                       // Animator 측에서 설정한 "Speed" 파라미터에 v를 전달
        //anim.SetFloat("Direction", h);                        // Animator 측에서 설정한 "Direction" 파라미터에 h를 전달
        anim.speed = animSpeed;                                 // Animator 모션 재생 속도에 animSpeed 설정하기
        rb.useGravity = true;                                   // 점프하는 동안 중력 차단, 그 외의 상황에서는 중력의 영향을 받도록 함

        if (playerStats.velocity < 0.1f)
        {
            playerStats.velocity = 0;
        }
    }
}
