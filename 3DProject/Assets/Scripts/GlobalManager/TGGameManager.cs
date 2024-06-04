using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGGameManager : UMonoSingleton<TGGameManager>
{
    [Range(0, 5)]
    public float CameraRotationSensitive = 3f;      //카메라 회전 감도


    protected override void ChildAwake()
    {

    }

    protected override void ChildOnDestroy()
    {

    }
}
