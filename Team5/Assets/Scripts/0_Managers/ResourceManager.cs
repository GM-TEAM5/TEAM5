using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ResourceManager : Singleton<ResourceManager>
{
    public readonly string enemyDataPath = "EnemyData";

    public readonly string defaultEnemyId = "000";


    public SerializedDictionary<string, EnemySO> enemyData = new();

    //=================================================


    public void Init()
    {
        foreach(EnemySO enemy in Resources.LoadAll<EnemySO>(enemyDataPath))
        {
            enemyData[enemy.id] = enemy;
        }

    }


    public EnemySO GetEnemyData(string id)
    {
        if (enemyData.ContainsKey(id))
        {
            return enemyData[id];
        }

        return enemyData[defaultEnemyId];
    }
}
