using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ŭ������� ���� ����ȭ�ϱ� ���� ����ϴ� ������ Ŭ���� �Դϴ�.
public class MCharacterStats
{
    public string characterName = "";

    public float maxHp              = 100f;
    public float currentHp          = 100f;
    public float velocity           = 0f;

    public MWeaponStats weaponStats = null;
}
