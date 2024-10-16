using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ResourceManager : Singleton<ResourceManager>
{
    public string enemyDataPath = "EnemyData";

    public SerializedDictionary<string, EnemySO> enemyData = new();

    //=================================================


    public void Init()
    {
        
        foreach(EnemySO enemy in Resources.LoadAll<EnemySO>(enemyDataPath))
        {
            enemyData[enemy.id] = enemy;
        }

    }
}
