using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


[Serializable]
public class StageWave_Elimination : StageWave
{
    public SerializableDictionary<string, int> currTargets = new();
    
    public StageWave_Elimination(SerializableDictionary<int, List<_SpawnInfo>> waveInfos): base(waveInfos)
    {
        goalType = StageGoalType.Elimination;
        // 1
        // 001  3
        

        // 2
        // 002   2

        // 3 
        // 001  3
        // 002  2

        // 3
        // 001    5
        // 002    3

        GameEventManager.Instance.onEnemyDie.AddListener( OnEnemyDie );
    }

    protected override void OnStageClear()
    {
        GameEventManager.Instance.onEnemyDie.RemoveListener( OnEnemyDie );
    }

    public override bool IsWaveClear()
    {
        return currTargets.Values.All(x => x <= 0);     //모든 타겟들 수가 0미만이 되면.
    }

    /// <summary>
    /// 섬멸전은 반복 생성하지 않는다. (일단은)
    /// </summary>
    /// <param name="spawnInfo"></param>
    /// <returns></returns>
    protected override IEnumerator SpawnRoutine(_SpawnInfo spawnInfo)
    {
        //
        RegisterTarget(spawnInfo);      

        // 
        float startTime = spawnInfo.startTime;
        // float endTime = spawnInfo.endTime;
        
        // startTime 동안 대기
        yield return new WaitForSeconds(startTime);
        
        SpawnEnemy(spawnInfo);
    }



    void RegisterTarget(_SpawnInfo spawnInfo)
    {
        string enemyId = spawnInfo.enemyData.id;
        
        if( currTargets.ContainsKey(enemyId ) ==false)
        {
            currTargets[enemyId ] = 0;
        }

        currTargets[enemyId] += spawnInfo.spawnPerCycle;
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

}


