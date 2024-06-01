using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGObject : MonoBehaviour
{
    //public
    public float MaxHp = 0;         // 최대 체력
    public float CurrentHp = 0;     // 현재 체력

    //private
    bool isGodMode = false;         // 오브젝트가 무적인지 판별하는 변수

    virtual protected void OnTakeDamage()
    {

    }

}
