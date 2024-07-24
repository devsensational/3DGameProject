using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGItemWeaponShotgun : TGItemWeapon
{
    protected override IEnumerator FireWeapon() // ���Ⱑ �߻�Ǵ� ����
    {
        if (currentAmmo <= 0) yield break; // ��ź ���� 0�̸� ���� ����
        if (isReloading) yield break; // ���� ���̸� ���� ����
        if (!isWeaponReady) yield break;

        currentAmmo--;

        if (itemHolder.tag == "Player")
        {
            TGEventManager.Instance.TriggerEvent(EEventType.UpdateItemInfo, this);
        }

        isWeaponReady = false;

        itemHolder.OnFire();

        // �ݵ��� ���� ���߷� ���� ����
        currentAccuracy = Mathf.Clamp(currentAccuracy * weaponStats.recoilMultiplier, currentMinAccuracy, weaponStats.maxAccuracy);
        ApplyRecoil();

        projectileFire();

        yield return fireRateWaitForSeconds;

        isWeaponReady = true;

        // �ڵ� �߻� ���⸦ ��� �߻��ؾ� �ϴ� ���
        if (weaponStats.fireMode == EGunFireMode.Auto && Input.GetKey(TGPlayerKeyManager.Instance.KeyValuePairs[EKeyValues.Fire]))
        {
            StartCoroutine(FireWeapon());
        }
    }

}
