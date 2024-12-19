using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWaveGenerator
{   
    public WaveDictionarySO waveDic;

    public StageWaveGenerator(WaveDictionarySO waveDic)
    {
        this.waveDic = waveDic;

    }


    /// <summary>
    /// 해당 스테이지 노드에 맞는 웨이브를 할당한다. 
    /// </summary>
    /// <param name="stageNode"></param>
    public void GenerateStageWave(StageNode stageNode)
    {        
        if (stageNode.isBattleStage == false)
        {
            return;
        }
        
        List<WaveDataSO> waves;
        if (stageNode.type == StageNodeType.NormalBattle 
        || stageNode.type == StageNodeType.EliteBattle)
        {
            waves = GetNormalWaves(stageNode);
        } 
        else
        {
            waves  = GetBossWaves(stageNode);
        }

        // set
        stageNode.SetWaveInfo(waves);
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="stageNode"></param>
    /// <returns></returns>
    List<WaveDataSO> GetNormalWaves(StageNode stageNode)
    {
         // init
        int rank = stageNode.level;
        int waveCount = stageNode.type == StageNodeType.NormalBattle?2:3;
         // add
        List<WaveDataSO> waves = new();
        for(int i=0;i<waveCount;i++)
        {
            waves.Add( waveDic.GetNormalWave(rank));
        }

        return waves;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stageNode"></param>
    /// <returns></returns>
    List<WaveDataSO> GetBossWaves(StageNode stageNode)
    {
        int chapter = stageNode.chapter;

        List<WaveDataSO> waves = new();
        if ( stageNode.type == StageNodeType.MiddleBoss)
        {
            waves.Add( waveDic.GetMiddleBossWave(chapter) );
        }
        else if (stageNode.type == StageNodeType.ChapterBoss)
        {
            waves.Add(  waveDic.GetChapterBossWave(chapter) );
        }
        
        return waves;
    }




}
