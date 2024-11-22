using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageStatistics 
{
    public SerializableDictionary<int, SerializableDictionary<string,int>> spawnedEnemyCount;  
    public SerializableDictionary<int, SerializableDictionary<string,int>> killedEnemyCount;
}
