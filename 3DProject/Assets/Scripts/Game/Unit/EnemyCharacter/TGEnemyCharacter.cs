using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적이 되는 캐릭터를 정의합니다.
public class TGEnemyCharacter : TGCharacter
{
    GameObject damageText;
    public override void ReceiveDamage(float damageValue)
    {
        base.ReceiveDamage(damageValue);
        Instantiate(damageText, transform.position, Quaternion.identity);
    }

}
