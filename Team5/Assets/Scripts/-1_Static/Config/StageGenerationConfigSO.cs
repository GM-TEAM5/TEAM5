using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel;
using Unity.VisualScripting;


/// <summary>
/// 스테이지 노드 및 스테이지 생성 관련 설정
/// </summary>
[CreateAssetMenu(fileName = "StageGenerationConfig", menuName = "SO/Config/StageGeneration", order = int.MaxValue)]
public class StageGenerationConfigSO : ScriptableObject 
{
    public List<StageNodeWeight> nodeWeights;

    public SerializableDictionary<int, StageNodeType> fixedNodeType = new();

    public List<int> soloNodeLevel = new(){4,5,9,10};

    
    
    //================================================================
    
    private void OnValidate()
    {
        ValidateProp();
    }
    
    
    //================================================================
    
    private void ValidateProp()
    {
        int maxProp = 100;
    
        // 총합이 100인지 확인 (약간의 오차 허용)
        for(int i=0;i<nodeWeights.Count-1;i++)
        {
            StageNodeWeight nw = nodeWeights[i];
            nw.weight = Mathf.Clamp(nw.weight, 0, maxProp);
            maxProp-= nw.weight;
        }
        nodeWeights[nodeWeights.Count-1].weight = maxProp;
    }

    //========================================================================

    public StageNodeType GetRandomNodeType()
    {
        StageNodeType ret = StageNodeType.NormalBattle;
        

        int rand = BW.Util.BwMath.GetRandom(0,100);
        int cumulation = 0;
        for(int i =0;i<nodeWeights.Count;i++)
        {
            StageNodeWeight nw = nodeWeights[i];
            cumulation += nw.weight;

            if (rand < cumulation)
            {
                ret = nw.type;
                break;
            }
        }

        return ret;
    } 

    public void AssignNodeTypes(List<StageNode> stageNodes)
    {
        int totalNodeCnt = stageNodes.Count;
        
        // 일단 특정 레벨의 노드는 타입을 지정함. 
        foreach(var kv in fixedNodeType)
        {
            int level = kv.Key;
            StageNodeType type = kv.Value;

            List<StageNode> targetNodes = stageNodes.Where(x=>x.level == level).ToList();
            for(int i=0;i<targetNodes.Count;i++)
            {
                targetNodes[i].SetType(type);
                totalNodeCnt--;
            }
        }

        // 그리고 나머지 노드의 타입은 이제 제약과 가중치에 따라 배치.
        int totalWeight = nodeWeights.Sum(x=>x.weight);
        List<StageNodeType> typeList = new();

        foreach(StageNodeWeight nw in nodeWeights)
        {
            int cnt = totalNodeCnt * nw.weight / totalWeight;
            for(int i=0;i<cnt*1.5f;i++) // 1.5는 
            {
                typeList.Add(nw.type);
            }
        }

        //
        System.Random random = new();
        typeList = typeList.OrderBy(x => random.Next()).ToList();   // shuffle
        Queue<StageNodeType> q = new (typeList);
        
        // 
        List<StageNode> unassginedNodes = stageNodes.Where(x=>x.type == StageNodeType.Unassigned ).ToList();
        foreach(StageNode node in unassginedNodes)
        {
            StageNodeType type = q.Dequeue();
            if (CanAssignType(stageNodes, node, type))
            {
                node.SetType(type);
            }
            else
            {
                q.Enqueue(type);    // 재활용하려고.
            }
        }

        // 나머지 처리
        unassginedNodes = unassginedNodes.Where(x=>x.type == StageNodeType.Unassigned ).ToList();
        foreach(StageNode node in unassginedNodes)
        {
            node.SetType(StageNodeType.NormalBattle);
        }
    } 


    /// <summary>
    /// 해당 노드에 해당 타입을 지정할 수 있는지 여부 판단 - 동일 타입의 노드간 거리가 minInterval보다 커야함. -bfs 알고리즘으로 구성.
    /// </summary>
    /// <param name="stageNode"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    bool CanAssignType(List<StageNode> stageNodes,StageNode stageNode,StageNodeType type)
    {
        int minInterval = 0;
        StageNodeWeight nw =  nodeWeights.Find(x=>x.type==type);
        if(nw !=null)
        {
            minInterval = nw.minInterval;
        }

        var visited = new HashSet<string>();
        var queue = new Queue<(string id, int depth)>();
        
        queue.Enqueue((stageNode.id, 0));
        visited.Add(stageNode.id);

        while (queue.Count > 0)
        {
            var (id, depth) = queue.Dequeue();

            StageNode node = stageNodes.Find(x=>x.id == id);

            // 시작 노드 제외 && 타입이 일치하면 반환
            if (depth > 0 && node.type == type)
            {
                return false;
            }

            // minInterval 범위 내에서만 탐색
            if (depth < minInterval)
            {
                // 다음 노드 탐색
                foreach (var next in node.nextNodes)
                {
                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue((next, depth + 1));
                    }
                }

                // 이전 노드 탐색
                foreach (var prev in node.prevNodes)
                {
                    if (!visited.Contains(prev))
                    {
                        visited.Add(prev);
                        queue.Enqueue((prev, depth + 1));
                    }
                }
            }
        }

        // 탐색 종료 후 동일 타입 노드를 못 찾은 경우
        return true; 
    }







    //=================================================================================================
    [System.Serializable]
    public class StageNodeWeight
    {
        public StageNodeType type;
        [Range(0, 100)] public int weight; // 인스펙터 슬라이더로 범위를 제한할 수도 있음
        public int minInterval;  // 배치가능한 최소 간격
    }
}
