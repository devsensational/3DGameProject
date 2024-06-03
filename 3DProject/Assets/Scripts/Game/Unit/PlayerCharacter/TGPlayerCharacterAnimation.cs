using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ĳ������ �ִϸ��̼� ��Ʈ���� ���� Ŭ�����Դϴ�
public class TGPlayerCharacterAnimation : MonoBehaviour
{
    //public
    // �ִϸ��̼� ��� �ӵ� ����
    public float animSpeed = 1.5f;
    //private
    //ref
    private TGPlayerCharacter       playerCharacter;
    private MCharacterStats   playerStats;

    private CapsuleCollider     col;
    private Rigidbody           rb;
    private Animator            anim;   // ĳ���� �ִϸ��̼� ref

    // CapsuleCollider���� ������ Collider�� Heiht, Center�� �ʱⰪ�� ��� ����
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private AnimatorStateInfo currentBaseState;                   // base layer���� ���Ǵ� �ִϸ������� ���� ���� ����


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

    //animation ���� method
    private void AnimationInit()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        // CapsuleCollider ������Ʈ�� Height, Center�� �ʱⰪ �����ϱ�
        orgColHight = col.height;
        orgVectColCenter = col.center;

    }
    private void OnMoveAnimation()
    {
        anim.SetFloat("Speed", playerStats.velocity);                       // Animator ������ ������ "Speed" �Ķ���Ϳ� v�� ����
        //anim.SetFloat("Direction", h);                        // Animator ������ ������ "Direction" �Ķ���Ϳ� h�� ����
        anim.speed = animSpeed;                                 // Animator ��� ��� �ӵ��� animSpeed �����ϱ�
        rb.useGravity = true;                                   // �����ϴ� ���� �߷� ����, �� ���� ��Ȳ������ �߷��� ������ �޵��� ��

        if (playerStats.velocity < 0.1f)
        {
            playerStats.velocity = 0;
        }
    }
}
