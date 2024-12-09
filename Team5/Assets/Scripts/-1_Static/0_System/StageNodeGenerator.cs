using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;

using BW.Util;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;



public class StageNodeGenerator : MonoBehaviour
{
    public StageGenerationConfigSO stageGenConfig;
    
    public int w = 6;
    public int h = 12;
    public int startNodeCnt = 3;
    public int leftStartOffset = 1;
    public int rightStartOffset = 1;

    public int repeatCnt = 5;

    public List<List<int>> nodeNums;
    public List<int> uniqueNodeIdxs = new(){4,9,10};

    //
    public List<List<StageNode>> mergedNodes = new();


    public StageNodeViewer nodeViewer;
    

    void Start()
    {
        GenerateStageNodes();

        //
        // for(int i=0;i<nodes.Count;i++)
        // {
        //     List<int> way = nodes[i];
            
        //     Debug.Log("==============");

        //     string str = $"way {i} :";
        //     for(int j=0;j<way.Count;j++)
        //     {
        //         str+= $" - {way[j]}";

        //     }
        //     Debug.Log(str);
        // }
    }

    public void GenerateStageNodes()
    {
        GenerateRawNodes();
        MergeRawNodes();
        SimplifyNodes();
    }


    /// <summary>
    /// 번호를 지닌 노드로 경로를 생성한다. 
    /// </summary>
    void GenerateRawNodes()
    {
        nodeNums = new();

        List<int> startNodeIdxs = GetStartNodeNums();

        // 시작 노드 선택
        for(int i=0;i<startNodeIdxs.Count;i++)  // 필수로 집어넣기
        {
            nodeNums.Add( new (){startNodeIdxs[i]} );
        }
        for(int i=0;i<repeatCnt - startNodeIdxs.Count;i++)  // 나머지는 랜덤~
        {
            int idx = BwMath.GetRandom(0, startNodeIdxs.Count);
            int startNode = startNodeIdxs[idx];
            nodeNums.Add( new (){startNode});
        }

        // 시작 지점부터 경로 생성 
        for(int i=0;i<repeatCnt;i++)
        {
            List<int> currLine = nodeNums[i];

            int startNode = currLine[0];
            for(int j=1;j<h;j++)
            {
                int nextNode = startNode + BwMath.GetRandom(-1,2);
                CorrectNextIdx(ref nextNode);

                if ( uniqueNodeIdxs.Contains( j ) )
                {
                    nextNode = w/2;
                }

                
                currLine.Add(nextNode);
                startNode = nextNode;
            }
        }

        // 보여주기.
        nodeViewer.ShowRawNodes(w,h,nodeNums);
    }

    /// <summary>
    /// 같은 번호를 가진 노드는 하나로 묶고, 노드 데이터를 생성한다. 
    /// </summary>
    void MergeRawNodes()
    {
        // 중첩된 노드 결합. 
        StageNode[,] mergingNodes = new StageNode[ h,w ];
        for(int i=0;i<nodeNums.Count;i++)     
        {
            List<int> targetNodeNums = nodeNums[i];

            StageNode prevNode = null;
            for(int j=0; j < targetNodeNums.Count; j++)   
            {
                // j 는 층수, 값은 번호
                int number = targetNodeNums[j];
                StageNode currNode = mergingNodes[j,number];    // 이미 생성한 노드 체크 
                if (currNode == null)                           // 생성되어 있지 않으면 생성
                {
                    StageNode stageNode = new(j, number );
                    currNode = stageNode;
                    mergingNodes[j,number] = currNode;
                }
                else
                {
                    
                }
                //
                currNode.AddPrevNode(prevNode);         // 노드 간 연결.
                prevNode = currNode;
            }
        }


        mergedNodes = new();
        for(int y=0;y< mergingNodes.GetLength(0);y++)
        {
            mergedNodes.Add(new());
            for(int x=0;x<mergingNodes.GetLength(1);x++)
            {
                StageNode node = mergingNodes[y,x];
                if ( node != null)
                {
                    mergedNodes[y].Add(node);
                }
            }
        }


        // Debug.Log("------------");
        // for(int y=0;y< mergingNodes.GetLength(0);y++)
        // {
        //     string str = "";
        //     for (int x=0;x< mergingNodes.GetLength(1);x++)
        //     {
        //         if (mergingNodes[y,x]== null)
        //         {
        //             str += $" {0}";

        //         }
        //         else
        //         {
        //             str+= $" {7}";
        //         }
        //     }
        //     Debug.Log(str);
        // }
        
    }


    /// <summary>
    /// 시작 노드를 하나만 남기고, 도달할 수 없는 노드는 제거한다. 
    /// </summary>
    void SimplifyNodes()
    {
        // 1.시작 노드 1개만 남기고 제거하고, 그에따라 도달할 수 없는 노드 제거 플래그 켜기
        int randIdx = BwMath.GetRandom(0, mergedNodes[0].Count );
        List<StageNode> excludedNodes = mergedNodes[0].Where((val, i) => i != randIdx).ToList();
        for(int i=0;i<excludedNodes.Count;i++)
        {
            StageNode excludedNode = excludedNodes[i];

            RecursiveDisconnectNode(excludedNode);
        }

        // 2. 제거플래그가 켜진 노드들은 리스트에서 제거
        for(int i=0;i<mergedNodes.Count;i++)
        {
            mergedNodes[i] = mergedNodes[i].Where(x=>x.unvalid == false ).ToList();
        }


        // 보여주기.
        nodeViewer.ShowMergedNodes(w,h,mergedNodes);
    } 


    void RecursiveDisconnectNode(StageNode targetNode)
    {
        if (targetNode.prevNodes.Count <=0)
        {
            targetNode.unvalid = true;
            foreach( StageNode nextNode in targetNode.nextNodes )
            {
                nextNode.RemovePrevNode(targetNode);

                RecursiveDisconnectNode( nextNode ); 
            }

        }


    }



    List<int> GetStartNodeNums()
    {
        List<int> numbers = new List<int>();
        for (int i = leftStartOffset; i < w- rightStartOffset; i++)
        {
            numbers.Add(i);
        }

        System.Random random = new System.Random();
        List<int> result = numbers.OrderBy(x => random.Next()).Take(startNodeCnt).ToList();

        return result;
    }

    void CorrectNextIdx(ref int idx)
    {
        if( idx<0)
        {
            
            idx =  BwMath.GetRandom(0,2);
        }
        if ( idx>= w)
        {
            idx =  BwMath.GetRandom(w-2,w);
        }
    }

    
}
