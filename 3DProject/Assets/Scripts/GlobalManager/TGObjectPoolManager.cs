using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TGObjectPoolManager : UMonoSingleton<TGObjectPoolManager>
{
    //private
    Dictionary<TGObjectType, IObjectPool<GameObject>> poolDictionary = new Dictionary<TGObjectType, IObjectPool<GameObject>>();

    //Unity lifecycle
    protected override void ChildAwake() 
    {
        foreach(TGObjectType tGObject in Enum.GetValues(typeof(TGObjectType)))
        {
            poolDictionary = new Dictionary<TGObjectType, IObjectPool<GameObject>>();
        }
    }

    protected override void ChildOnDestroy() { }

    private void OnGetProjectile(GameObject ptrObject)
    {
        ptrObject.SetActive(true);
    }

    private void OnReleaseProjectile(GameObject ptrObject)
    {
        ptrObject.SetActive(false);
    }

    private void OnDestroyProjectile(GameObject ptrObject)
    {
        GameObject.Destroy(ptrObject);
    }

    public IObjectPool<GameObject> CreateObjectPool(TGObjectType type, GameObject ptrObject)
    {
        return poolDictionary[type];
    }

}
