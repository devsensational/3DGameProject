using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGAIConditionMagazineEmpty : TGAIConditionNodeBase
{
    //private
    TGCharacter character;

    // Unity Lifecycle
    public override void Start()
    {
        base.Start();
        character = controller.character;
    }

    public override bool TriggerAction(out TGAIActionNodeBase ptrNode)
    {
        TGItemWeapon weapon = character.GetInHandWeapon();

        if (weapon.currentAmmo == 0)
        {
            ptrNode = action;
            return true;
        }

        ptrNode = null;
        return false;
    }
}
