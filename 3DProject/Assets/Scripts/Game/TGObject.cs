using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGObject : MonoBehaviour
{
    //public
    public float MaxHp = 0;         // �ִ� ü��
    public float CurrentHp = 0;     // ���� ü��

    //private
    bool isGodMode = false;         // ������Ʈ�� �������� �Ǻ��ϴ� ����

    virtual protected void OnTakeDamage()
    {

    }

}
