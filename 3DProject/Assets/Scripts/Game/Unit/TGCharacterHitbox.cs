using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TGCharacter를 상속받는 스크립트를 사용하는 오브젝트에 부착하여 사용합니다.
// TGCharacter는 Root에 부착되어 있어야 합니다.
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

    public void OnReceiveDamage(float damageValue)
    {
        characterRoot.ReceiveDamage(damageValue * damageMultiple);
    }
}
