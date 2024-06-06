using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현 프로젝트에 공통적인 기능을 정의하기 위한 클래스입니다
public enum TGObjectType
{
    None = 0,
    // 여기서 부터 타입 작성
    

    //
}

public class TGObject : MonoBehaviour
{
    //public
    public string objectName = "";                          // 오브젝트 이름
    public TGObjectType objectType = TGObjectType.None;     // 오브젝트 타입

    //private
}
