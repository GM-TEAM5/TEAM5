using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class DontDestroy : Singleton<DontDestroy>
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
