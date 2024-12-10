using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;

using BW.Util;


public class StageNodeGenerator : MonoBehaviour
{
    public int w = 6;
    public int h = 12;
    public int startNodeCnt = 3;
    public int leftStartOffset = 1;
    public int rightStartOffset = 1;

    public int repeatCnt = 5;

    public List<List<int>> nodes;
    public List<int> uniqueNodeIdxs = new(){4,9,10};


    public StageNodeViewer nodeViewer;
    

    void Start()
    {
        GenerateNodes();

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

    public void GenerateNodes()
    {
        nodes = new();

        List<int> startNodeIdxs = GetStartNodes();

        // 시작 노드 선택
        for(int i=0;i<startNodeIdxs.Count;i++)  // 필수로 집어넣기
        {
            nodes.Add( new (){startNodeIdxs[i]} );
        }
        for(int i=0;i<repeatCnt - startNodeIdxs.Count;i++)  // 나머지는 랜덤~
        {
            int idx = Math.GetRandom(0, startNodeIdxs.Count);
            int startNode = startNodeIdxs[idx];
            nodes.Add( new (){startNode});
        }

        // 시작 지점부터 경로 생성 
        for(int i=0;i<repeatCnt;i++)
        {
            List<int> currLine = nodes[i];

            int startNode = currLine[0];
            for(int j=1;j<h;j++)
            {
                int nextNode = startNode + Math.GetRandom(-1,2);
                nextNode = GetCorrectedNextIdx(nextNode);

                if ( uniqueNodeIdxs.Contains( j ) )
                {
                    nextNode = w/2;
                }

                
                currLine.Add(nextNode);
                startNode = nextNode;
            }
        }

        // 보여주기.
        nodeViewer.ShowStageNodes(w,h,nodes);
    }


    List<int> GetStartNodes()
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

    int GetCorrectedNextIdx(int idx)
    {
        if( idx<0)
        {
            
            return Math.GetRandom(0,2);
        }
        if ( idx>= w)
        {
            return Math.GetRandom(w-2,w);
        }
        return idx;
    }
}
