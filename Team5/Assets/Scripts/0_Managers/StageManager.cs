using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : Singleton<StageManager>
{
    public bool initialized;
    
    // [SerializeField] NavMeshSurface stageNavMesh;
    public StageDataSO stageData;



    /// <summary>
    /// 스테이지 초기화 - 데이터 및 지형 세팅
    /// </summary>
    /// <param name="stageData"></param>
    public void Init(StageDataSO stageData)
    {

        this.stageData = stageData;


        //
        initialized = true;
    }

    /// <summary>
    /// 게임플레이가 시작될 때, 웨이브 시작
    /// </summary>
    public void OnStartGamePlay()
    {
        // 웨이브 시작. 
        foreach(var wave in stageData.waves)
        {
            StartCoroutine(wave.WaveRoutine());
        }
    }

    public static Vector3 GetRandomPosition(Vector3 center,float range )
    {
        Vector3 ret = center;
        
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                ret = hit.position;
                break;
            }
        }

        // Debug.Log( $" random Pos {ret}" );
        return ret;

    }
}
