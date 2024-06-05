using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MWeaponStats
{
    public string weaponName;      //무기 이름, json 식별 시에도 사용

    public float damage;           //데미지
    public float fireRate;         //연사속도
    public float defaultAccurate;  //명중률
    public float maxAccurate;      //최대 명중률, 명중률이 최대 얼마까지 낮아질 것인지 결정하는 변수
    public float bulletVelocity;   //탄속
    public float reloadTime;       //재장전시간

    public int maxAmmo;            //탄창에 넣을 수 있는 최대 총알 갯수
    public int currentAmmo;        //현재 탄창의 총알 개수
}
