﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

// 게임의 전체적인 부분을 관리하는 클래스입니다.
public class TGGameManager : UMonoSingleton<TGGameManager>
{
    //Inspector
    [Range(0, 100)]
    public float CameraRotationSensitive = 40f;      //카메라 회전 감도
    [Header("Script Files")]
    public TextAsset WeaponStatsScript = null;

    //public
    public Dictionary<string, MWeaponStats> loadedWeaponStatDict = new Dictionary<string, MWeaponStats>();
    public UJsonUtility jsonUtility = new UJsonUtility();

    //private


    //Json

    //Unity lifecycle
    protected override void ChildAwake()
    {
        LoadWeaponStats();
    }

    protected override void ChildOnDestroy()
    {

    }

    //Json 관련 메소드
    private void LoadWeaponStats()
    {
        loadedWeaponStatDict = jsonUtility.LoadJsonFile<Dictionary<string, MWeaponStats>>(WeaponStatsScript);
    }

    public float GetCameraRotationSensitive()
    {
        return CameraRotationSensitive * 10;
    }
}
