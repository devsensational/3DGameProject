using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object 타입을 정의하기 위한 enum입니다.
// ObjectPool에서 주로 활용됩니다.
public enum ETGObjectType
{
    None = 0,
    // 여기서 부터 타입 작성
    Projectile,
    UILootableItemButton,
    ItemDefault,
    ItemEquip,
    CharacterDefault,
    CharacterPlayer,

    //
}
