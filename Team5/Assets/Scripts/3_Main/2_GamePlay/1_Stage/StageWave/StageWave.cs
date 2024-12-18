using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Spawninfo
public enum SpawnPositionType
{
    RandomArea,
    SpecificPoint
}

/// <summary>
/// 적 생성정보.
/// </summary>
[Serializable]
public class _SpawnInfo
{
    public float startTime;
    public float endTime;
    
    public EnemyDataSO enemyData;
    public SpawnPositionType positionType;
    public Vector3 spawnPoint;
    public int spawnPerCycle;
    public int cycleInterval;   // -1이면 한번 생성하고 끝내기

    public _SpawnInfo(float startTime, float endTime, EnemyDataSO enemyData, int cycleInterval, int spawnPerCycle )
    {
        this.startTime = startTime;
        this.endTime = endTime;
        
        this.enemyData = enemyData; 
        this.cycleInterval = cycleInterval;
        this.spawnPerCycle = spawnPerCycle;

        positionType = SpawnPositionType.RandomArea;
    }
    public _SpawnInfo(float startTime, float endTime, EnemyDataSO enemyData, int cycleInterval, int spawnPerCycle, Vector3 spawnPoint )
        : this ( startTime,endTime,enemyData, cycleInterval,spawnPerCycle )
    {
        this.spawnPoint = spawnPoint;

        positionType = SpawnPositionType.SpecificPoint;
    }
}
#endregion






#region StageWave

/// <summary>
/// 스테이지를 구성하는 적의 생성 정보들.
/// </summary>
[Serializable]
public abstract class StageWave 
{
    public StageGoalType goalType;
    public SerializableDictionary<int, List<_SpawnInfo>> waveInfos = new();

    List<Coroutine> runningSpawnRoutines = new();   // 이거 때문에 에러뜨는거 같은데... 
    public int clearedWaveNum;

    public bool allWaveClear => clearedWaveNum >= waveInfos.Count;


    public StageWave(SerializableDictionary<int, List<_SpawnInfo>> waveInfos)
    {
        this.waveInfos = waveInfos;
    }


    //================================================================================================
    
    /// <summary>
    /// 이벤트에 달려서 웨이브 클리어 체크를 진행시키도록 해야함. - 
    /// </summary>
    protected void CheckWaveClear()
    {
        if (IsWaveClear())
        {
            FinishWave();

            
            //
            if(IsStageClear())
            {
                StageManager.Instance.StageClear();
                OnStageClear();
            }
            else
            {
                StartWave(clearedWaveNum);
            }
        }
    }
    
    
    
    /// <summary>
    /// 웨이브 시작. 
    /// </summary>
    /// <param name="waveNum"></param>
    public void StartWave(int waveNum)
    {
        // Debug.Log($"웨이브 시작 {waveNum}");
        
        //
        if(runningSpawnRoutines==null)
        {
            runningSpawnRoutines = new();
        }
        
        //
        if (waveInfos.TryGetValue(waveNum, out List<_SpawnInfo> waveInfo))
        {
            foreach(_SpawnInfo spawnInfo in waveInfo)
            {
                
                Coroutine spawnRoutine =  StageManager.Instance.StartCoroutine( SpawnRoutine( spawnInfo ));
                runningSpawnRoutines.Add(spawnRoutine);
            }   
        } 
    }

    /// <summary>
    /// 웨이브 종료. 
    /// </summary>
    public void FinishWave()
    {
        StopAllRoutines();
        clearedWaveNum++;
    }


    
    void StopAllRoutines()
    {
        foreach(Coroutine c in runningSpawnRoutines)
        {
            StageManager.Instance.StopCoroutine(c);
        }
        runningSpawnRoutines.Clear();
    }


    /// <summary>
    /// 스테이지 클리어인지.
    /// </summary>
    /// <returns></returns>
    public bool IsStageClear()
    {
        return clearedWaveNum >= waveInfos.Count;
    }


    //============================================================================================================
    public abstract bool IsWaveClear();         // 각 모드별로 지정해야함. 

    protected abstract IEnumerator SpawnRoutine(_SpawnInfo spawnInfo);


    protected abstract void OnStageClear();

    //=============================================================================================================

    protected void SpawnEnemy(_SpawnInfo spawnInfo)
    {
        string enemyId = spawnInfo.enemyData.id;
        int spawnPerCycle = spawnInfo.spawnPerCycle;
        SpawnPositionType positionType = spawnInfo.positionType;

        //
        if (positionType ==SpawnPositionType.RandomArea)
        {
            Spawn_RandomArea(enemyId, spawnPerCycle);
        }
        else if (positionType == SpawnPositionType.SpecificPoint)
        {
            Spawn_SpecificPoint(enemyId,  spawnPerCycle,spawnInfo.spawnPoint );
        }
    }


    /// <summary>
    /// 스폰 영역 내 임의의 지점에서 생성.
    /// </summary>
    void Spawn_RandomArea(string id, int count)
    {
        for(int i=0;i<count;i++)
        {
            Vector3 randPos = StageManager.Instance.GetRandomEnemySpawnPoint();
            PoolManager.Instance.GetEnemySpawner(id, randPos);
        }

        Debug.Log($"[적 생성] {id}를 {count}마리");
    }

    /// <summary>
    /// 지정된 위치에 생성 - 보통 보스 소환에 사용됨.
    /// </summary>
    /// <param name="spawnPoint"></param>
    void Spawn_SpecificPoint(string id, int count, Vector3 spawnPoint)
    {
        for(int i=0;i<count;i++)
        {
            PoolManager.Instance.GetEnemySpawner(id, spawnPoint);
        }

        Debug.Log($"[적 생성] {id}를 {spawnPoint}에");

    }

}


#endregion
