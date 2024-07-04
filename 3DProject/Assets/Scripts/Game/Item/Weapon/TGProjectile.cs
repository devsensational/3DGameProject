using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGProjectile : TGObject
{
    //Insepector
    public float releaseTime = 10;

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"(TGProjectile:OnTriggerEnter) {transform.name}'s projectile collides with {other.gameObject.name}");

        if (other != null)
        {
            OnProjectileImpact();
        }
    }

    //
    public void CommandFire(Vector3 muzzlePosition, Quaternion muzzleRotation, float velocity) // �߻簡 ȣ����� ��
    {
        transform.rotation  = muzzleRotation;
        transform.position = muzzlePosition;
        rb.velocity         = transform.forward * velocity;
        //rb.velocity = transform.forward *  0f;
        isFlying = true;

        Debug.Log($"(TGProjectile:CommandFire) muzzlePosition: {muzzlePosition}, ProjectilePositino: {transform.position}");
        //Invoke("ReleaseProjectile", releaseTime); //
    }

    private void Fly() // �߻�ü ����
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
    }

    private void OnProjectileImpact() // ���ư��� ���� ���� ������Ʈ�� ��� ������ ��
    {
        rb.velocity = Vector3.zero;
        isFlying = false;
    }

    private void ReleaseProjectile()
    {
        TGObjectPoolManager.Instance.ReleaseTGObject(ETGObjectType.Projectile, gameObject);

    }
}
