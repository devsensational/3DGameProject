using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� ĳ������ �ִϸ��̼� ��Ʈ���� ���� Ŭ�����Դϴ�
public class TGCharacterAnimation : MonoBehaviour
{
    //public
    // �ִϸ��̼� ��� �ӵ� ����
    public float animSpeed = 1.5f;
    //private
    //ref
    private TGCharacter character;
    private MCharacterStats playerStats;

    private CapsuleCollider col;
    private Rigidbody rb;
    private Animator anim;   // ĳ���� �ִϸ��̼� ref

    // CapsuleCollider���� ������ Collider�� Heiht, Center�� �ʱⰪ�� ��� ����
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private AnimatorStateInfo currentBaseState;                   // base layer���� ���Ǵ� �ִϸ������� ���� ���� ����

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

    void InitAnimations() //animation ���� method
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        // CapsuleCollider ������Ʈ�� Height, Center�� �ʱⰪ �����ϱ�
        orgColHight = col.height;
        orgVectColCenter = col.center;

    }
    void OnMoveAnimation()
    {
        anim.SetFloat("Speed", playerStats.velocity);           // Animator ������ ������ "Speed" �Ķ���Ϳ� v�� ����
        //anim.SetFloat("Direction", h);                        // Animator ������ ������ "Direction" �Ķ���Ϳ� h�� ����
        anim.speed = animSpeed;                                 // Animator ��� ��� �ӵ��� animSpeed �����ϱ�
        rb.useGravity = true;                                   // �����ϴ� ���� �߷� ����, �� ���� ��Ȳ������ �߷��� ������ �޵��� ��

        if (playerStats.velocity < 0.1f)
        {
            playerStats.velocity = 0;
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
