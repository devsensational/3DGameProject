using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGProjectile : TGObject
{
    //Insepector
    public float releaseTime = 10;

    //public
    public float damage { get; private set; }

    //private
    Rigidbody rb; // rigidbody ref

    float velocity = 20;

    bool isFlying = false;

    //Unity lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isFlying)
        {
            //Fly();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"(TGProjectile:OnTriggerEnter) {transform.name}'s projectile collides with {collision.gameObject.name}");

        if (collision.gameObject.tag == "Hitbox")
        {
            collision.gameObject.GetComponent<TGCharacterHitbox>().OnReceiveDamage(damage);
        }

        if (collision != null)
        {
            OnProjectileImpact();
        }
    }

    //
    public void CommandFire(Vector3 muzzlePosition, Quaternion muzzleRotation, float velocity, float mass, float damage) // �߻簡 ȣ����� ��
    {
        this.damage = damage;

        transform.rotation = muzzleRotation;
        transform.position = muzzlePosition;

        //Rigidbody �Ķ���� ����
        rb.isKinematic = false;
        rb.MovePosition(muzzlePosition);
        rb.velocity     = transform.forward * velocity;
        rb.useGravity   = true;
        rb.mass         = mass;

        isFlying = true;

        Invoke("ReleaseProjectile", releaseTime); // �� �� �� ������Ʈ ������
    }

    private void Fly() // �߻�ü ����
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
    }

    private void OnProjectileImpact() // ���ư��� ���� ���� ������Ʈ�� ��� ������ ��
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        isFlying = false;
    }

    private void ReleaseProjectile()
    {
        TGObjectPoolManager.Instance.ReleaseTGObject(ETGObjectType.Projectile, gameObject);

    }
}
