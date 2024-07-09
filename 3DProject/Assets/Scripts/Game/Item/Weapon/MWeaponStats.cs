using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    // 해당 변수들은 모두 default 값으로 "Data" 폴더의 WeaponStats.Json에 의해 변경됨
    // 데이터가 정상적으로 들어가지 않을 경우 weaponName과 objectName이 일치하는 지 확인할 것

    public string weaponName        = "default_name";       //무기 이름, json 식별 시에도 사용

    public EEquipmentType   weaponType  = EEquipmentType.None;    //무기 타입
    public EItemType        ammoType    = EItemType.None;         //탄 타입
    public EGunFireMode     fireMode    = EGunFireMode.Default;   //발사모드

    public float damage                     = 0f;       //데미지
    public float fireRate                   = 100f;     //연사속도
    public float defaultAccuracy            = 0.3f;     //명중률
    public float currentAccuracy            = 0.3f;     //현재 명중률
    public float aimingAccuracy             = 0.1f;     //조준 명중률
    public float minAccuracy                = 0.1f;     //최대 명중률, 명중률이 최대 얼마까지 높아질 것인지 결정하는 변수
    public float maxAccuracy                = 0.8f;     //최소 명중률, 명중률이 최대 얼마까지 낮아질 것인지 결정하는 변수
    public float recoilMultiplier           = 1.15f;    //반동에 의한 명중률 저하 배수
    public float recoilRecoveryMultiplier   = 0.95f;
    public float bulletVelocity             = 300f;     //탄속
    public float reloadTime                 = 3.0f;     //재장전시간
    public float range                      = 800f;     //재장전시간

    public int   maxAmmo                    = 30;       //탄창에 넣을 수 있는 최대 총알 갯수
    public int   currentAmmo                = 0;        //현재 탄창의 총알 개수
}
