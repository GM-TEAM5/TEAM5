using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class StageMissionInfo
{
    public StageGoalType goalType;
    public float targetTime;
    public SerializableDictionary<int, List<SpawnInfo>>waveInfos;
}


[Serializable]
public class StageMapInfo
{
    public float width;
    public float height; 

    public StageMapInfo(float width, float height)
    {
        this.width = width;
        this.height = height;
    }
}


/// <summary>
/// 스테이지 세팅을 위한 데이터 - 일ㄹ단은 SO인데 나중에는 절차적 맵생성해야함. 
/// </summary>
[CreateAssetMenu(fileName = "StageData", menuName = "SO/Stage/StageData", order = int.MaxValue)] 
public class StageDataSO : ScriptableObject
{
    public string id;   // 스테이지 id
    public StageMissionInfo missionInfo;
    public StageMapInfo mapInfo;











    // public  SerializableDictionary<int,List<WaveInfo>> waves;



    // public int totalWavesNum =>waves.Count;


    // public List<WaveInfo> GetCurrWaveInfo(int waveNum)
    // {
    //     return waves[waveNum];
    // }

    /// <summary>
    /// 해당 스테이지에서 사용되는 적들의 id를 전부 반환한다. - 스테이지 초기화할 때 필요한 것만 메모리에 올리기 위함. 
    /// </summary>
    /// <returns></returns>
    // public List<string> GetEntireEnemyId()
    // {
    //     SortedSet<string> set = new ();
        
    //     for(int i=0;i<waves.Count;i++)
    //     {
    //         for(int j=0; j<waves[i].enemyIds.Count;j++)
    //         {
    //             string id = waves[i].enemyIds[j];
    //             set.Add(id);
    //         }
    //     }

    //     return new List<string>(set);
    // }

}
