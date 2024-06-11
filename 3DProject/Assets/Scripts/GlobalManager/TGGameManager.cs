using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TGGameManager : UMonoSingleton<TGGameManager>
{
    //Inspector
    [Range(0, 5)]
    public float CameraRotationSensitive = 3f;      //카메라 회전 감도
    [Header("Script Files")]
    public TextAsset WeaponStatsScript = null;

    //public
    public Dictionary<string, MWeaponStats> loadedWeaponStatDict = new Dictionary<string, MWeaponStats>();

    //private


    //Json
    private UJsonUtility jsonUtility = new UJsonUtility();

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
}
