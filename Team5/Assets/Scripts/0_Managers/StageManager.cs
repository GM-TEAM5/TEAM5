
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : Singleton<StageManager>
{
    public bool initialized;
    
    // [SerializeField] NavMeshSurface stageNavMesh;
    public int clearedWaveNum;
    
    public StageDataSO stageData;
    public Stage currStage;

    public bool isBattleInProgress;
    public float battleTime;

    //=====================================================================

    /// <summary>
    /// 스테이지 초기화 - 데이터 및 지형 세팅
    /// </summary>
    /// <param name="stageData"></param>
    public void Init(StageDataSO stageData)
    {
        //
        this.stageData = stageData;
        
        //
        currStage = Instantiate( stageData.prefab_stage, Vector3.zero, Quaternion.identity ).GetComponent<Stage>();
        currStage.Init();   // 플레이어 위치 정보, 적 스폰 위치 등 초기화 


        //
        Player.Instance.t_player.position = currStage.playerInitPos;  // 플레이어 위치 지정 

        //
        initialized = true;
    }

    //=================================================================================================
    
    #region Stage
    public bool IsStageClear()
    {
        bool ret = stageData.totalWavesNum == clearedWaveNum;
        return ret;
    }

    public void OnStageClear()
    {
        Debug.Log("스테이지 클리어");
    }


    #endregion
    //==========================================================================================
    #region Wave
    /// <summary>
    /// 웨이브를 활성화한다. 
    /// </summary>
    public void StartWave()
    {
        List<WaveInfo> currWaveInfo =   stageData.GetCurrWaveInfo(clearedWaveNum+1);
        
        foreach(var wave in currWaveInfo)
        {
            StartCoroutine(wave.WaveRoutine());
        }        
    }


    /// <summary>
    /// 웨이브 클리어 처리. 
    /// </summary>
    public void OnWaveClear()
    {
        Debug.Log("웨이브 클리어");
        
        clearedWaveNum ++;
    }
    #endregion
    //============================================================================================


    /// <summary>
    /// Enemy Spawn Area에서 임의의 좌표를 얻는다. 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomEnemySpawnPoint()
    {
        if( currStage ==null )
        {
            Debug.Log("현재 스테이지가 없음");
            return Vector3.zero;
        }
        return currStage.GetRandomSpawnPoint();
    }



    // public static Vector3 GetRandomPosition(Vector3 center,float range )
    // {
    //     Vector3 ret = center;
        
    //     for (int i = 0; i < 30; i++)
    //     {
    //         Vector3 randomPoint = center + Random.insideUnitSphere * range;
    //         NavMeshHit hit;
    //         if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
    //         {
    //             ret = hit.position;
    //             break;
    //         }
    //     }

    //     // Debug.Log( $" random Pos {ret}" );
    //     return ret;
    // }
}
