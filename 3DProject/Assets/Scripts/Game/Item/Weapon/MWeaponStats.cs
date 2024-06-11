using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    public string weaponName        = "default_name";       //���� �̸�, json �ĺ� �ÿ��� ���

    public EItemType weaponType     = EItemType.Default;    //���� Ÿ��

    public float damage             = 0f;       //������
    public float fireRate           = 100f;     //����ӵ�
    public float defaultAccurate    = 0.4f;     //���߷�
    public float aimingAccurate     = 0.1f;     //���� ���߷�
    public float minAccurate        = 0.8f;     //�ּ� ���߷�, ���߷��� �ִ� �󸶱��� ������ ������ �����ϴ� ����
    public float bulletVelocity     = 100f;     //ź��
    public float reloadTime         = 3.0f;     //�������ð�

    public int maxAmmo              = 30;       //źâ�� ���� �� �ִ� �ִ� �Ѿ� ����
    public int currentAmmo          = 0;        //���� źâ�� �Ѿ� ����
}
