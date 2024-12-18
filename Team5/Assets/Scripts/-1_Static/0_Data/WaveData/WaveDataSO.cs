using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WaveData", menuName = "SO/WaveData", order = int.MaxValue)]
public class WaveDataSO : ScriptableObject
{
    public int rank;

    public List<SpawnInfo> spawnInfos;
}


[Serializable]
public class SpawnInfo
{
    public EnemyDataSO enemyData;
    public int count;

}

