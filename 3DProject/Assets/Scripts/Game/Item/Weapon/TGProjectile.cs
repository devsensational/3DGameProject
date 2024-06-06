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
    public void CommandFire(Vector3 direction) // �߻簡 ȣ����� ��
    {
        enabled = true;
    }

    private void Fly() // �߻�ü ����
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
    }

    private void OnProjectileImpact() // ���ư��� ���� ���� ������Ʈ�� ��� ������ ��
    {

    }
}
