using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGProjectile : TGObject
{
    //private
    TGItemWeapon parentWeapon;

    Vector3 direction;

    float velocity = 0;

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

    }

    private void Fly() // 발사체 비행
    {

    }

    private void OnProjectileImpact() // 날아가던 도중 게임 오브젝트에 닿아 막혔을 때
    {

    }
}
