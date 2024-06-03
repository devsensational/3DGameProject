using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현 프로젝트에 공통적인 기능을 정의하기 위한 클래스입니다
public class TGObject : MonoBehaviour
{
    //private
    bool isGodMode = false;         // 오브젝트가 무적인지 판별하는 변수

    virtual protected void OnTakeDamage()
    {

    }

}
