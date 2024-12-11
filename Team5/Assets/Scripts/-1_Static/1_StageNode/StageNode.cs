using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum StageNodeType
{
    Unused,             // 사용되지 않음.
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

    public int level;       // 세로번호
    public int number;      // 가로번호

    public List<StageNode> prevNodes;
    public List<StageNode> nextNodes;

    public bool unvalid;

    //==============================
    public StageNode(int level, int number)
    {
        this.level = level;
        this.number = number;

        prevNodes = new();
        nextNodes = new();
    }

    // public StageNode(StageNodeType type, int level, int number)
    // {
    //     this.type = type;
    //     this.level = level;
    //     this.number = number;
    // }

    //===================
    public void AddPrevNode(StageNode node)
    {
        if (node!=null && prevNodes.Contains(node) == false)
        {
            prevNodes.Add(node);

            node.AddNextNode(this);
        }
    }
    public void AddNextNode(StageNode node)
    {
        if (node!=null && nextNodes.Contains(node) == false)
        {
            nextNodes.Add(node);
        }
    }

    //==================
    public void RemovePrevNode(StageNode node)
    {
        if (node!=null && prevNodes.Contains(node))
        {
            prevNodes.Remove(node);
        }
    }


}
