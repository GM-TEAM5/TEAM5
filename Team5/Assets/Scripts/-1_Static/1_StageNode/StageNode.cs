using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
    
    public string id =>$"{chapter}_{level}_{number}";

    public int chapter;
    public int level;       // 세로번호
    public int number;      // 가로번호

    public List<string> prevNodes;
    public List<string> nextNodes;       // 이거 hideInspector 해도 렉걸림. 

    [NonSerialized] public bool unvalid;

    //==============================
    public StageNode(int chapter, int level, int number)
    {
        this.chapter = chapter;
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
    }
}
