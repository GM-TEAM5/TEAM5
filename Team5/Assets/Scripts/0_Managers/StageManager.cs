using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public bool initialized;
    
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
}
