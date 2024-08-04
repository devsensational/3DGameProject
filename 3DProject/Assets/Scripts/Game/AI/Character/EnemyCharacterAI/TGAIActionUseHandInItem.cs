using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TGAIActionUseHandInItem : TGAIActionNode
{
    //Inspector
    public int burstCount;

    TGCharacter character;

    public override void Start()
    {
        base.Start();
        character = controller.character;
    }


    public override IEnumerator Run()
    {
        if(character == null) { yield break;}
        if(character.HandInItem == EEquipmentType.None) { yield break; }

        TGItemWeapon weapon = (TGItemWeapon)character.equipItems[character.HandInItem];

        WaitForSeconds autoWeaponFireWaitForSeconds = new WaitForSeconds(60 / weapon.weaponStats.fireRate);
        for (int i = 0; i < burstCount; i++)
        {
            weapon.UseItem();
            yield return autoWeaponFireWaitForSeconds;
        }
    }
}
