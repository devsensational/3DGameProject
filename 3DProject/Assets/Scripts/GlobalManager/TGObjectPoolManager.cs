using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class TGObjectPoolManager : UMonoSingleton<TGObjectPoolManager>
{
    // "TGObjectType"�� ���� ���� ������Ʈ Ǯ�� ���۷��� �ϱ� ���� ��ųʸ�
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

    // Unity ObjectPool�� �ʿ��� �޼ҵ��
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

    // Ư�� ��ü ������ ���� ��ü Ǯ�� �����ϰ� �������� �޼ҵ�
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

    // Ǯ���� ��ü�� �������� �޼���
    public GameObject GetTGObject(ETGObjectType type)
    {
        if (poolDictionary.ContainsKey(type) && poolDictionary[type] != null)
        {
            return poolDictionary[type].Get();
        }

        Debug.LogError($"Pool for type {type} does not exist.");
        return null;
    }

    // ��ü�� Ǯ�� �ٽ� �������ϴ� �޼���
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
