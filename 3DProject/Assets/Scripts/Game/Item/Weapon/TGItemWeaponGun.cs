using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeaponGun : TGItemWeapon
{

    public override void UseItem()
    {
        if(weaponStats.fireMode == EGunFireMode.Semi)
        {
            base.UseItem();
        }
        if(weaponStats.fireMode == EGunFireMode.Auto)
        {
            
        }
    }
}
