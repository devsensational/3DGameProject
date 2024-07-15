using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TGCharacter�� ��ӹ޴� ��ũ��Ʈ�� ����ϴ� ������Ʈ�� �����Ͽ� ����մϴ�.
// TGCharacter�� Root�� �����Ǿ� �־�� �մϴ�.
public class TGCharacterHitbox : MonoBehaviour
{
    //Inspector
    public EHitboxType hitboxType;
    public float damageMultiple = 1f;

    TGCharacter characterRoot;

    private void Awake()
    {
        characterRoot = transform.root.GetComponent<TGCharacter>();
        Debug.Log($"(TGCharacterHitBox:Awake) {characterRoot.objectName} Loaded ref");
    }

    public float OnReceiveDamage(float damageValue)
    {
        float finalDamage = damageValue * damageMultiple;
        characterRoot.ReceiveDamage(finalDamage);

        return finalDamage;
    }
}
