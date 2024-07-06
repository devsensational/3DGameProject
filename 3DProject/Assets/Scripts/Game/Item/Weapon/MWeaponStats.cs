using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    public string weaponName        = "default_name";       //���� �̸�, json �ĺ� �ÿ��� ���

    public EEquipmentType weaponType    = EEquipmentType.None;    //���� Ÿ��
    public EItemType ammoType           = EItemType.None;

    public float damage             = 0f;       //������
    public float fireRate           = 100f;     //����ӵ�
    public float defaultAccuracy    = 0.3f;     //���߷�
    public float currentAccuracy    = 0.3f;     //���� ���߷�
    public float aimingAccuracy     = 0.1f;     //���� ���߷�
    public float minAccuracy        = 0.8f;     //�ּ� ���߷�, ���߷��� �ִ� �󸶱��� ������ ������ �����ϴ� ����
    public float bulletVelocity     = 300f;     //ź��
    public float reloadTime         = 3.0f;     //�������ð�
    public float range              = 800f;     //�������ð�

    public int maxAmmo              = 30;       //źâ�� ���� �� �ִ� �ִ� �Ѿ� ����
    public int currentAmmo          = 0;        //���� źâ�� �Ѿ� ����
}
