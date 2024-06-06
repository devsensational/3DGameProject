using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGProjectile : TGObject
{
    //private
    TGItemWeapon parentWeapon;

    Vector3 direction;

    float velocity = 1;

    //Unity lifecycle
    private void Update()
    {
        Fly();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            OnProjectileImpact();
        }
    }

    //
    public void CommandFire(Vector3 direction) // 발사가 호출됐을 때
    {
        enabled = true;
    }

    private void Fly() // 발사체 비행
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
    }

    private void OnProjectileImpact() // 날아가던 도중 게임 오브젝트에 닿아 막혔을 때
    {

    }
}
