using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    public string weaponName;      //���� �̸�, json �ĺ� �ÿ��� ���

    public float damage;           //������
    public float fireRate;         //����ӵ�
    public float defaultAccurate;  //���߷�
    public float maxAccurate;      //�ִ� ���߷�, ���߷��� �ִ� �󸶱��� ������ ������ �����ϴ� ����
    public float bulletVelocity;   //ź��
    public float reloadTime;       //�������ð�

    public int maxAmmo;            //źâ�� ���� �� �ִ� �ִ� �Ѿ� ����
    public int currentAmmo;        //���� źâ�� �Ѿ� ����
}
