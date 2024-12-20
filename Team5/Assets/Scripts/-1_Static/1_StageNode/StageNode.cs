using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using BW;
using Unity.VisualScripting;
using UnityEngine;


public enum StageNodeType
{
    Unassigned,             // 사용되지 않음.
    NormalBattle,       // 일반 전투
    EliteBattle,        // 정예 전투
    // Event,
    Unknown,            // 미지 
    Store,              // 상점
    MiddleBoss,         // 중간 보스
    ChapterBoss         // 챕터 보스 
}

[Serializable]
public class StageNode
{
    public StageNodeType type;
    public bool isBattleStage => type == StageNodeType.NormalBattle || 
                                type == StageNodeType.EliteBattle ||
                                type ==  StageNodeType.MiddleBoss ||
                                type == StageNodeType.ChapterBoss; 
                                
    public string id =>$"{chapter}_{level}_{number}";

    public int chapter;
    public int level;       // 세로번호
    public int number;      // 가로번호

    public int rank;
    
    public Sprite thumbnail;        // 썸네일 (포탈에 보임)


    public List<string> prevNodes;
    public List<string> nextNodes;       // 이거 hideInspector 해도 렉걸림. 

    public StageWaveInfo waveInfo;  // 웨이브 정보
    public StageRewardInfo rewardInfo;  // 클리어시 보상 정보
    
    public StageFormInfo formInfo;  // 형태 정보

    [NonSerialized] public bool unvalid;

    //==============================
    public StageNode(int chapter, int level, int number)
    {
        this.chapter = chapter;
        this.level = level;
        this.number = number;

        rank = GetDifficulty(level);

        prevNodes = new();
        nextNodes = new();

        int minSize = 12 +  3 * rank;
        int maxSize = 24 +  4 * rank;
        formInfo = new(BW.Math.GetRandom(minSize,maxSize), BW.Math.GetRandom(minSize,maxSize));
    }

    // public StageNode(StageNodeType type, int level, int number)
    // {
    //     this.type = type;
    //     this.level = level;
    //     this.number = number;
    // }    /// <summary>
    ///  해당 레벨에 맞는 난이도를 계산한다. 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    int GetDifficulty(int level)
    {
        int rank = 0;
        switch( level)
        {
            case 1:
            case 2:
                rank = 1;
                break;
            case 3:
            case 4:
            case 5:
                rank =2;
                break;
            case 6:
            case 7:
                rank = 3;
                break;
            case 8:
            case 9:
            case 10:
                rank = 4;
                break;
        }

        return rank;
    }

    //===================
    public void AddPrevNode(StageNode node)
    {
        if (node !=null && prevNodes.Contains(node.id) == false)
        {
            prevNodes.Add(node.id);
            node.AddNextNode(this);        
        }
    }
    public void AddNextNode(StageNode node)
    {
        if (node != null && nextNodes.Contains(node.id) == false)
        {
            nextNodes.Add(node.id);
        }
    }

    //==================
    public void RemovePrevNode(string id)
    {
        if (prevNodes.Contains(id))
        {
            prevNodes.Remove(id);
        }
    }

    public void RemoveNextNode(string id)
    {
        if (nextNodes.Contains( id))
        {
            nextNodes.Remove( id );
        }
    }

    //======================================
    public void SetType(StageNodeType type)
    {
        this.type = type;
        SetStageReward();   // 타입설정되었으니 보상 설정 ㅋ - 추후 규칙이 생기면 밖으로 빼자.
        SetThumbnail();
    }

    public void SetWaveInfo(List<WaveDataSO> waves)
    {
        waveInfo = new( waves );
    }

    public void SetStageReward()
    {
        List<StageRewardSO> rewards = new();
        StageRewardDictionarySO stageRewardDic = ResourceManager.Instance.stageRewardDic;
        
        // 일반 전투 : 스탯 보상
        if(type == StageNodeType.NormalBattle)
        {
            if (stageRewardDic.TryGetStatusPointReward(1, out StatusPointRewardSO statusPointReward))
            {
                rewards.Add(statusPointReward);
            }
        } 
        // 정예 전투 : 스킬보상
        else if(type == StageNodeType.EliteBattle)
        {
            if( stageRewardDic.TryGetRandomSkillReward(out  SkillRewardSO skillReward))
            {
                rewards.Add(skillReward);
            }
        }
        // 중간보스 : 스킬보상 + 스텟2
        else if(type == StageNodeType.MiddleBoss)
        {
            if( stageRewardDic.TryGetRandomSkillReward(out  SkillRewardSO skillReward))
            {
                rewards.Add(skillReward);
            }
            if (stageRewardDic.TryGetStatusPointReward(2, out StatusPointRewardSO statusPointReward))
            {
                rewards.Add(statusPointReward);
            }
        }
        // 챕터보스 : 스킬보상 + 스탯3
        else if(type == StageNodeType.ChapterBoss)
        {
            if( stageRewardDic.TryGetRandomSkillReward(out  SkillRewardSO skillReward))
            {
                rewards.Add(skillReward);
            }
            if (stageRewardDic.TryGetStatusPointReward(3, out StatusPointRewardSO statusPointReward))
            {
                rewards.Add(statusPointReward);
            }
        }


        rewardInfo = new(rewards);
        
    }

    void SetThumbnail()
    {
        if (rewardInfo.data.Count>0)
        {
            thumbnail = rewardInfo.data[0].sprite;
        }
        
    }

}




/// <summary>
/// 스테이지 적 등장 정보.
/// </summary>
[Serializable]
public class StageWaveInfo
{
    public List<WaveDataSO> waves = new();
    public int totalWaveCount => waves.Count;


    public StageWaveInfo(List<WaveDataSO> waves)
    {
        this.waves = waves;
    }
}


/// <summary>
/// 스테이지 적 등장 정보.
/// </summary>
[Serializable]
public class StageRewardInfo
{   
    public List<StageRewardSO> data;
    
    public StageRewardInfo(List<StageRewardSO> rewards)
    {
        this.data = rewards;
    }
}



/// <summary>
/// 스테이지 형태 정보
/// </summary>
[Serializable]
public class StageFormInfo
{
    public float width;
    public float height; 

    public StageFormInfo(float width, float height)
    {
        this.width = width;
        this.height = height;
    }
}