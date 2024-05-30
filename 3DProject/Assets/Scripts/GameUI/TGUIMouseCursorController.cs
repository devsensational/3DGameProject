using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGUIMouseCursorController : MonoBehaviour
{
    private Dictionary<KeyValues, KeyCode> keyValuePairs;           // KeyValuePair map ref

    void Start()
    {
        keyValuePairs = TGPlayerKeyManager.Instance.KeyValuePairs;
    }

    void Update()
    {
    }

}
