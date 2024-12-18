
using UnityEngine;
using System.Linq;


public class StageManager : Singleton<StageManager>
{
    public bool initialized;
    
    // [SerializeField] NavMeshSurface stageNavMesh;
    // public int clearedWaveNum;
    
    // public StageDataSO stageData;
    public StageNode nodeData;
    public Stage currStage;
    // public StageWave stageWave;

    public bool isBattleInProgress;
    public float battleTime;

    public int currWaveNum;

    public SerializableDictionary<string, int> currTargets = new();





    //=====================================================================

    /// <summary>
    /// 스테이지 초기화 - 데이터 및 지형 세팅
    /// </summary>
    /// <param name="playerData"></param> 
    public void Init()
    {
        //
        // 노드 생성 임시 - 나중에는 천계에서 노드 생성하고 넘어오자.
        if(GameManager.Instance.totalNodeData == null || GameManager.Instance.totalNodeData.initialized == false)
        {
            StageNodeGenerator sng = new(ResourceManager.Instance.stageGenConfig);
            GameManager.Instance.totalNodeData = sng.GenerateStageNodes();
        }

        // 노드 데이터 할당
        PlayerDataSO userData = GameManager.Instance.playerData;
        string nodeId = userData.currstageNodeId;

        TotalNodeData totalNodeData = GameManager.Instance.totalNodeData;
        if( totalNodeData.TryGetNodeInfo(nodeId, out this.nodeData) )
        {
            
        }
        else
        {
            nodeData = totalNodeData.GetFirstNode();
        }
        Debug.Log($"[스테이지] {nodeData.id} 시작");

        // int currStageNum = userData.currStageNum;
        // StageDataSO stageData = userData.GetCurrStageInfo();
        
        currStage = Instantiate( ResourceManager.Instance.prefab_stage, Vector3.zero, Quaternion.identity ).GetComponent<Stage>();
        currStage.Init(nodeData);   // 플레이어 위치 정보, 적 스폰 위치 등 초기화 

        // if( stageData.missionInfo.goalType == StageGoalType.Survival)
        // {
        //     stageWave = new StageWave_Survival(stageData.missionInfo.targetTime,stageData.missionInfo.waveInfos );
        // }
        // else 
        // {
        //     stageWave = new StageWave_Elimination(stageData.missionInfo.waveInfos);
        // }

        currWaveNum = 0;



        GameEventManager.Instance.onEnemyDie.AddListener( OnEnemyDie );


        initialized = true;
    }

    //=================================================================================================
    #region Stage
    public void StartStage()
    {
        if(nodeData.isBattleStage)
        {
            StartWave(currWaveNum);

        }
        else
        {
            
        }
    }



    /// <summary>
    /// 스테이지 클리어인지.
    /// </summary>
    /// <returns></returns>
    public bool IsStageClear()
    {
        return currWaveNum >= nodeData.waveInfo.totalWaveCount;
    }


    public void StageClear()
    {
        Debug.Log("스테이지 클리어");
        GameEventManager.Instance.onEnemyDie.RemoveListener( OnEnemyDie );
        GamePlayManager.Instance.OnStageClear();
        
    }


    #endregion
    //==========================================================================================
    #region Wave
    /// <summary>
    /// 웨이브를 활성화한다. 
    /// </summary>
    public void StartWave(int waveNum)
    {
        Debug.Log("웨이브 시작");
        StageWaveInfo currWaveInfo =  nodeData.waveInfo; 
        WaveDataSO currWave = currWaveInfo.waves[waveNum];
        RegisterTarget(currWave);
        SpawnEnemy(currWave);
    }

    /// <summary>
    /// 이벤트에 달려서 웨이브 클리어 체크를 진행시키도록 해야함. - 
    /// </summary>
    void CheckWaveClear()
    {
        if (IsWaveClear())
        {
            WaveClear();
        }
    }
    
    
    bool IsWaveClear()
    {
        return currTargets.Values.All(x => x <= 0);     //모든 타겟들 수가 0미만이 되면.
    }




    /// <summary>
    /// 웨이브 클리어 처리. 
    /// </summary>
    public void WaveClear()
    {
        Debug.Log("웨이브 클리어");
        
        currWaveNum++;

        if (IsStageClear())
        {
            StageClear();
        }
        else
        {
            StartWave(currWaveNum);
        }
    }


    //==================================================================================================
    
    void RegisterTarget(WaveDataSO waveData)
    {
        foreach(SpawnInfo spawnInfo in waveData.spawnInfos)
        {
            string enemyId = spawnInfo.enemyData.id;
             
             if( currTargets.ContainsKey(enemyId ) ==false)
            {
                currTargets[enemyId ] = 0;
            }

            currTargets[enemyId] += spawnInfo.count;
        } 
    }


    void SpawnEnemy(WaveDataSO waveData)
    {
        foreach(SpawnInfo spawnInfo in waveData.spawnInfos)
        {
            string id = spawnInfo.enemyData.id;
            for(int i=0;i<spawnInfo.count;i++)
            {
                Vector3 spawnPos = GetRandomEnemySpawnPoint();
                PoolManager.Instance.GetEnemySpawner( id,spawnPos );
            }
           
        }  
    }

    /// <summary>
    /// 적 처치 이벤트 핸들러. 적 ID에 따라 카운트를 감소시키고 웨이브 종료 조건을 검사합니다.
    /// </summary>
    /// <param name="enemy"></param>
    void OnEnemyDie(Enemy enemy)
    {   
        // Debug.Log("짠");
        string enemyId = enemy.data.id;
        if( currTargets.ContainsKey(enemyId))
        {
            currTargets[enemyId]--;
        }

        CheckWaveClear();
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
