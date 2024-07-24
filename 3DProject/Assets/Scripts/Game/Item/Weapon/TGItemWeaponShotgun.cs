using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeaponShotgun : TGItemWeapon
{
    protected override IEnumerator FireWeapon() // 무기가 발사되는 과정
    {
        if (currentAmmo <= 0) yield break; // 장탄 수가 0이면 실행 안함
        if (isReloading) yield break; // 장전 중이면 실행 안함
        if (!isWeaponReady) yield break;

        currentAmmo--;

        if (itemHolder.tag == "Player")
        {
            TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        }

        isWeaponReady = false;

        itemHolder.OnFire();

        // 반동에 의한 명중률 저하 구현
        currentAccuracy = Mathf.Clamp(currentAccuracy * weaponStats.recoilMultiplier, currentMinAccuracy, weaponStats.maxAccuracy);
        ApplyRecoil();

        projectileFire();

        yield return fireRateWaitForSeconds;

        isWeaponReady = true;

        // 자동 발사 무기를 계속 발사해야 하는 경우
        if (weaponStats.fireMode == EGunFireMode.Auto && Input.GetKey(TGPlayerKeyManager.Instance.KeyValuePairs[EKeyValues.Fire]))
        {
            StartCoroutine(FireWeapon());
        }
    }

}
