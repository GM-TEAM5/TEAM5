using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class TotalNodeData
{
    public static Dictionary<string,StageNode> dic;

    public TotalNodeData(string nodeDataJson)
    {
        // 
        dic = new ();
        if (string.IsNullOrEmpty(nodeDataJson))
        {
            Debug.LogWarning("JSON 데이터가 비어있습니다.");
            return;
        }
        //
        try
        {
            // JsonUtility로 JSON 데이터를 변환
            JsonListWrapper<StageNode> jsonListWrapper = JsonUtility.FromJson<JsonListWrapper<StageNode>>(nodeDataJson);

            // totalNodeData에 리스트 할당
            foreach(StageNode node in jsonListWrapper.data)
            {
                dic[node.id] = node;
            }
            Debug.Log($"노드 데이터 로드 완료: {dic.Count}개");
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON 파싱 중 오류 발생: {ex.Message}");
        }

    }

    public static void SaveTotalNodeData()
    {
        List<StageNode> stageNodes = new List<StageNode>(dic.Values);

        stageNodes = stageNodes
            .OrderBy(node => node.chapter)
            .ThenBy(node => node.level)
            .ThenBy(node => node.number)
            .ToList();



        // Debug.Log("===== saved ======");
        // string str = "";
        // stageNodes.ForEach( x=> str+=$" {x.id}, ");
        // Debug.Log(str);

        LocalDataHandler.SaveTotalNodeData( stageNodes );
    }

    public static void AddData(List<StageNode> stageNodes)
    {
        if (dic == null)
        {
            dic = new();
        }
        
        foreach(StageNode node in stageNodes)
        {
            dic[node.id] = node;
        }

        // Debug.Log($"[TotalNodeInfo] 데이터 추가 {dic.Count}개");
    }

    public static void RemoveData(string id)
    {
        if (dic == null)
        {
            return;
        }
        
        dic.Remove(id);
    }

    public static void RemoveNodes(List<string> ids)
    {
        if( dic == null)
        {
            return;
        }

        foreach( string id in ids)
        {

            dic.Remove(id);
        }
    }

    public static void ClearAllNodes()
    {
        dic?.Clear();
    }

    //=======================================================================================================

    // public static bool TryGetNodeInfo(string id, out StageNode node)
    // {
    //     node = null;
    //     if ( dic !=null && dic.TryGetValue(id,  out node) )
    //     {
    //         return true;
    //     }
    //     return false;
    // }


    // public static bool TryGetNodeInfos(List<string> ids, out List<StageNode> nodes)
    // {
    //     nodes = new();
    //     if ( dic != null)
    //     {
    //         foreach(string id in ids)
    //         {
    //             if ( dic.TryGetValue(id, out StageNode node))
    //             {
    //                 nodes.Add(node);
    //             }
    //         }

    //         return ids.Count == nodes.Count;
    //     }

    //     return false;
    // }
}


//==========================================================

[Serializable]
public class JsonListWrapper<T>
{
    public List<T> data;

    public JsonListWrapper(List<T> data)
    {
        this.data = data;
    }
}