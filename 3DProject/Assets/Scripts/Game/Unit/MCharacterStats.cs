using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여러 클래스들과 값을 동기화하기 위해 사용하는 데이터 클래스 입니다.
public class MCharacterStats
{
    public string characterName = "";

    public float maxHp              = 200f;
    public float velocity           = 0f;

    public MWeaponStats weaponStats = null;
}
