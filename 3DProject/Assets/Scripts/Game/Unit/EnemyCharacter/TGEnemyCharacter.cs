using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �Ǵ� ĳ���͸� �����մϴ�.
public class TGEnemyCharacter : TGCharacter
{
    GameObject damageText;
    public override void ReceiveDamage(float damageValue)
    {
        base.ReceiveDamage(damageValue);
        Instantiate(damageText, transform.position, Quaternion.identity);
    }

}
