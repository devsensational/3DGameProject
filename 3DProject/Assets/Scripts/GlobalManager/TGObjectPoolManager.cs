using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class TGObjectPoolManager : UMonoSingleton<TGObjectPoolManager>
{
    // "TGObjectType"에 따라 여러 오브젝트 풀을 레퍼런싱 하기 위한 딕셔너리
    private Dictionary<ETGObjectType, IObjectPool<GameObject>> poolDictionary = new Dictionary<ETGObjectType, IObjectPool<GameObject>>();

    // Unity lifecycle
    protected override void ChildAwake()
    {
        Init();
    }

    protected override void ChildOnDestroy()
    {
        Clear();
    }

    //Init
    void Init()
    {
        foreach (ETGObjectType tGObject in Enum.GetValues(typeof(ETGObjectType)))
        {
            poolDictionary[tGObject] = null;
        }
    }

    void Clear()
    {
        foreach (var pool in poolDictionary.Values)
        {
            if (pool != null)
            {
                pool.Clear();
            }
        }
    }

    // Unity ObjectPool에 필요한 메소드들
    private void OnGetTGObject(GameObject ptrObject)
    {
        ptrObject.SetActive(true);
    }

    private void OnReleaseTGObject(GameObject ptrObject)
    {
        ptrObject.SetActive(false);
    }

    private void OnDestroyTGObject(GameObject ptrObject)
    {
        GameObject.Destroy(ptrObject);
    }

    // 특정 객체 유형에 대한 객체 풀을 생성하고 가져오는 메소드
    public IObjectPool<GameObject> CreateTGObjectPool(ETGObjectType type, GameObject prefab, int initialSize = 10, int maxSize = 100)
    {
        if (poolDictionary[type] == null)
        {
            poolDictionary[type] = new ObjectPool<GameObject>(
                createFunc: () => GameObject.Instantiate(prefab),
                actionOnGet: OnGetTGObject,
                actionOnRelease: OnReleaseTGObject,
                actionOnDestroy: OnDestroyTGObject,
                collectionCheck: false,
                defaultCapacity: initialSize,
                maxSize: maxSize
            );
        }

        return poolDictionary[type];
    }

    // 풀에서 객체를 가져오는 메서드
    public GameObject GetTGObject(ETGObjectType type)
    {
        if (poolDictionary.ContainsKey(type) && poolDictionary[type] != null)
        {
            return poolDictionary[type].Get();
        }

        Debug.LogError($"Pool for type {type} does not exist.");
        return null;
    }

    // 객체를 풀에 다시 릴리즈하는 메서드
    public void ReleaseTGObject(ETGObjectType type, GameObject ptrObject)
    {
        if (poolDictionary.ContainsKey(type) && poolDictionary[type] != null)
        {
            poolDictionary[type].Release(ptrObject);
        }
        else
        {
            Debug.LogError($"Pool for type {type} does not exist.");
            GameObject.Destroy(ptrObject);
        }
    }
}
