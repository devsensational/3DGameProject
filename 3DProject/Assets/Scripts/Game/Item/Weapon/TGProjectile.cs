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
    public void CommandFire(Vector3 direction) // �߻簡 ȣ����� ��
    {

    }

    private void Fly() // �߻�ü ����
    {

    }

    private void OnProjectileImpact() // ���ư��� ���� ���� ������Ʈ�� ��� ������ ��
    {

    }
}
