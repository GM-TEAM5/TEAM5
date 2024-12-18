using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;




[System.Serializable]
public class StageWave_Survival : StageWave
{
    
    public float targetTime;

    public StageWave_Survival(float targetTime , SerializableDictionary<int, List<_SpawnInfo>> waveInfos): base(waveInfos)
    {
        goalType = StageGoalType.Survival;
        this.targetTime = targetTime;
        
        //     // 0 ~ 24
        //     // 001  2초에 1마리
        //     // 002  6초에 1마리

        //     // 24 ~ 60 
        //     // 001 3초에 2마리
        //     // 002 5초에 1마리 

        //     // 60 ~ 90
        //     // 001 5초에 3마리
        //     // 002 5초에 1마리
        //     // 003 5초에 1마리

        GamePlayManager.Instance.SetAlarm(targetTime);
        GameEventManager.Instance.onAlarm.AddListener(OnAlarm);
    }



    public override bool IsWaveClear()
    {
        // Debug.Log( $"{GamePlayManager.Instance.gamePlayTime >= targetTime}  &&  { Player.Instance.isAlive}" );
        return GamePlayManager.Instance.gamePlayTime >= targetTime  && Player.Instance.isAlive;
    }

    protected override void OnStageClear()
    {
        GameEventManager.Instance.onAlarm.RemoveListener(OnAlarm);
    }

    protected override IEnumerator SpawnRoutine(_SpawnInfo spawnInfo)
    {
        // 
        float startTime = spawnInfo.startTime;
        float endTime = spawnInfo.endTime;
        
        // startTime 동아 대기
        yield return new WaitUntil(()=>GamePlayManager.Instance.gamePlayTime >=startTime);
        
        // endTime까지 사이클 진행
        var waitForSeconds = spawnInfo.cycleInterval > 0 ? new WaitForSeconds(spawnInfo.cycleInterval) : null;
        
        if ( waitForSeconds == null)    // cycleInterval 이 0이하인 경우엔 한번만 실행. 
        {
            SpawnEnemy(spawnInfo);
        }
        else    // 일반적인경우
        {
            while (GamePlayManager.Instance.gamePlayTime < spawnInfo.endTime)
            {
                // 적생성
                SpawnEnemy(spawnInfo);
                yield return waitForSeconds;
            }
        }
        

    }


    void OnAlarm(float timer)
    {
        CheckWaveClear();
    }
}



