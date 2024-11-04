
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
    Stage currStage;

    public bool isBattleInProgress;
    public float battleTime;

    //=====================================================================





    /// <summary>
    /// 스테이지 초기화 - 데이터 및 지형 세팅
    /// </summary>
    /// <param name="stageData"></param>
    public void Init(StageDataSO stageData)
    {
        GameEventManager.Instance.onEnemyDie.AddListener((Enemy e)=> CheckWaveClear());

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

    /// <summary>
    /// 게임플레이가 시작될 때, 웨이브 시작
    /// </summary>
    public void OnStartGamePlay()
    {
        // 웨이브 시작. 
        
    }

    //=================================================================================================
    
    #region Stage
    public bool IsStageClear()
    {
        bool ret = stageData.totalWavesNum == clearedWaveNum;
        return ret;
    }

    public void ClearStage()
    {
        Debug.Log("스테이지 클리어");

        currStage.portal.OnStageClear();
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
    /// 웨이브가 클리어 여부에 따라 처리를 진행한다. - 몬스터가 죽을 때마다 실행될 예정. 
    /// </summary>
    public void CheckWaveClear()
    {
        //
        if (IsWaveClear())
        {
            ClearWave();

            if (IsStageClear())
            {
               ClearStage();
            }
        }
    }

    /// <summary>
    /// 현재 웨이브가 클리어 되었는 지 확인한다. - 웨이브에 할당된 유닛이 전부 소환되었고, 소환된 모든 적을 섬멸했는 지
    /// </summary>
    bool IsWaveClear()
    {
        foreach(WaveInfo waveInfo in stageData.GetCurrWaveInfo(clearedWaveNum+1))
        {
            if ( ( waveInfo.isWaveFinished  && PoolManager.Instance.enemyNotExists) == false )
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 웨이브 클리어 처리. 
    /// </summary>
    public void ClearWave()
    {
        Debug.Log("웨이브 클리어");
        
        clearedWaveNum ++;
        
        // 아이템 생성
        // 웨이브 활성화 구슬 초기화.
        if (IsStageClear()==false)
        {
            currStage.waveActivationSwitch.OnWaveClear();
        }
        
        // GameEventManager.Instance.onWaveClear.Invoke(clearedWave);
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
