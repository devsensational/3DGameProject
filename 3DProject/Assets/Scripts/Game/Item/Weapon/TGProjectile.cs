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
    public void CommandFire(Vector3 muzzlePosition, Quaternion muzzleRotation, float velocity) // 발사가 호출됐을 때
    {
        transform.rotation  = muzzleRotation;
        transform.position = muzzlePosition;
        rb.velocity         = transform.forward * velocity;
        //rb.velocity = transform.forward *  0f;
        isFlying = true;

        Debug.Log($"(TGProjectile:CommandFire) muzzlePosition: {muzzlePosition}, ProjectilePositino: {transform.position}");
        //Invoke("ReleaseProjectile", releaseTime); //
    }

    private void Fly() // 발사체 비행
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
    }

    private void OnProjectileImpact() // 날아가던 도중 게임 오브젝트에 닿아 막혔을 때
    {
        rb.velocity = Vector3.zero;
        isFlying = false;
    }

    private void ReleaseProjectile()
    {
        TGObjectPoolManager.Instance.ReleaseTGObject(ETGObjectType.Projectile, gameObject);

    }
}
