using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    public string weaponName;      //���� �̸�, json �ĺ� �ÿ��� ���
    public string weaponType;      //���� Ÿ��, enum �Ľ��ؼ� ���

    public float damage;           //������
    public float fireRate;         //����ӵ�
    public float defaultAccurate;  //���߷�
    public float aimingAccurate;   //���� ���߷�
    public float minAccurate;      //�ּ� ���߷�, ���߷��� �ִ� �󸶱��� ������ ������ �����ϴ� ����
    public float bulletVelocity;   //ź��
    public float reloadTime;       //�������ð�

    public int maxAmmo;            //źâ�� ���� �� �ִ� �ִ� �Ѿ� ����
    public int currentAmmo;        //���� źâ�� �Ѿ� ����
}
