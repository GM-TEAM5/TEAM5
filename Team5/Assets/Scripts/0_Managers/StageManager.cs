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

    public float stageStartTime;    // 해당 스테이지 시작 시간.
    public float stagePlayTime => Time.time - stageStartTime; 

    void Start()
    {
        // 여기서 스테이지 로드 완료 이벤트 재생
        GameEventManager.Instance.onStageLoad.Invoke();
    }


    public void Init(StageDataSO stageData)
    {
        this.stageData = stageData;

        stageStartTime = Time.time;

        // 웨이브 시작. 
        foreach(var wave in stageData.waves)
        {
            StartCoroutine(wave.WaveRoutine());
        }

        //
        initialized = true;
    }

    public Vector3 GetRandomPosition(Vector3 center,float range )
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
