using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    public string weaponName        = "default_name";       //무기 이름, json 식별 시에도 사용

    public EEquipmentType weaponType    = EEquipmentType.None;    //무기 타입
    public EItemType ammoType           = EItemType.None;

    public float damage             = 0f;       //데미지
    public float fireRate           = 100f;     //연사속도
    public float defaultAccuracy    = 0.3f;     //명중률
    public float currentAccuracy    = 0.3f;     //현재 명중률
    public float aimingAccuracy     = 0.1f;     //조준 명중률
    public float minAccuracy        = 0.8f;     //최소 명중률, 명중률이 최대 얼마까지 낮아질 것인지 결정하는 변수
    public float bulletVelocity     = 300f;     //탄속
    public float reloadTime         = 3.0f;     //재장전시간
    public float range              = 800f;     //재장전시간

    public int maxAmmo              = 30;       //탄창에 넣을 수 있는 최대 총알 갯수
    public int currentAmmo          = 0;        //현재 탄창의 총알 개수
}
