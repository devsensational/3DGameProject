using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGAIConditionMagazineEmpty : TGAIConditionNode
{
    TGCharacter character;

    public override void Start()
    {
        base.Start();
        character = controller.character;
    }

    public override bool TriggerAction(out TGAIActionNode ptrNode)
    {
        TGItemWeapon weapon = (TGItemWeapon)character.equipItems[character.HandInItem];

        if (weapon.currentAmmo == 0)
        {
            ptrNode = action;
            return true;
        }

        ptrNode = null;
        return false;
    }
}
