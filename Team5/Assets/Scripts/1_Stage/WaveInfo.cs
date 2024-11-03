using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum WaveForm
{
    RandomArea,     //영역 내 랜덤 지정
    SpecificPoint   // 특정 좌표 
}


/// <summary>
/// 웨이브 1개에 대한 정보 -  
/// </summary>
[System.Serializable]
public class WaveInfo
{
    public int waveNum;
    public List<string> enemyIds= new();
    public int totalNum;                    // 생성할 유닛 전체 수
    public float spawnDuration;         //  전체 생성 지속 시간  
    [Min(1f)] public int totalCycle = 1;         //  분할 생성 수 . 
    // public int currCycle;
    public bool isWaveFinished  {get;private set;}    // 웨이브가 끝났는지. 

    // public WaveFormSO waveForm;     // 생성 형태 
    public WaveForm waveForm;     // 생성 형태

    public float spawnStartTime;

    



    //
    public IEnumerator WaveRoutine()
    {
        if (totalCycle >0)
        {
            int spawnPerCycle = (int)Math.Ceiling( (double)totalNum/totalCycle);
            float cycleInterval = spawnDuration/totalCycle;
            
            yield return new WaitForSeconds(spawnStartTime);   
            
            //
            for(int i=0;i<totalCycle;i++)
            {
                for(int j=0;j<spawnPerCycle;j++)
                {
                    SpawnWave();
                }

                yield return new WaitForSeconds(cycleInterval); // 추후엔 하데스 처럼 모든 적이 섬멸 되었을 때 바로 웨이브 시작하도록 수정하자. 
            }
            
        }
        isWaveFinished  = true;
    }


    /// <summary>
    /// 지정된 스폰 타입에 맞춰 스폰
    /// </summary>
    public void SpawnWave()
    {   
        //

        switch( waveForm )
        {
            case  WaveForm.SpecificPoint: 
                Spawn_SpecificPoint(Vector3.zero);
                break;
            default:
                Spawn_RandomArea();
                break; 
        }
    }

    //=======================================================================================
    /// <summary>
    /// 스폰 영역 내 임의의 지점에서 생성.
    /// </summary>
    public void Spawn_RandomArea()
    {
        for(int i=0;i<enemyIds.Count;i++)
        {
            string id = enemyIds[i];
        
            Vector3 randPos = StageManager.Instance.GetRandomEnemySpawnPoint();
            PoolManager.Instance.GetEnemySpawner(id, randPos);
        }
    }

    /// <summary>
    /// 지정된 위치에 생성 - 보통 보스 소환에 사용됨.
    /// </summary>
    /// <param name="spawnPoint"></param>
    public void Spawn_SpecificPoint(Vector3 spawnPoint)
    {
        for(int i=0;i<enemyIds.Count;i++)
        {
            string id = enemyIds[i];
            PoolManager.Instance.GetEnemySpawner(id, spawnPoint);
        }
    }



    // public void SpawnWave(Vector3 spawnPoint)
    // {
    //     // waveForm.Spawn( enemyIds ,spawnPoint );
    // }


}

