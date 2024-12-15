using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageNodeViewer : MonoBehaviour
{
    
    public GameObject prefab_nodeLine;
    //
    [Header("Raw")]
    public Transform t_viewer_raw;
    public Transform t_lineParent_raw;
    public List<NodeLineUI> lineUIs_raw;

    //\
    [Header("Merged")]
    public Transform t_viewer_merged;
    public Transform t_lineParent_merged;
    public List<NodeLineUI> lineUIs_merged;
    
    


    
    public UILineRenderer lineRenderer;

    public List<Color> colors = new()
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        new Color(1.0f, 0.65f, 0.0f),       // 오렌지
        Color.magenta,
        new Color(0.5f, 0.0f, 0.5f),        //퍼플
        new Color(1.0f, 0.7f, 0.8f),       //핑크
        new Color(0.65f, 0.16f, 0.16f),        //브라운
        new Color(0.75f, 1.0f, 0.0f),        // 연두
    };


    //==================================================================================


    public void ShowRawNodes(int w, int h, List<List<int>> nodes)
    {
        // Destroy
        for(int i=0;i<t_viewer_raw.childCount;i++)
        {
            Destroy(t_viewer_raw.GetChild(i).gameObject);
        }

        // Generate
        lineUIs_raw = new();
        for(int i=0;i<h;i++)
        {
            NodeLineUI lineUI = Instantiate(prefab_nodeLine,t_viewer_raw).GetComponent<NodeLineUI>();
            lineUI.Init(w);
            lineUI.transform.SetAsFirstSibling();

            lineUIs_raw.Add(lineUI);
        }

        // Allocate
        for(int i=0;i<nodes.Count;i++)
        {
            List<RectTransform> points= new();

            Color color = colors[i];
            for(int j=0;j<nodes[i].Count;j++)
            {  
                int nodeIdx = nodes[i][j]; 
                NodeUI node = lineUIs_raw[j].nodeUIs[nodeIdx];
                node.SetColor(color);

                points.Add( node.GetComponent<RectTransform>());
            }
            lineRenderer.DrawLines(points,color,t_lineParent_raw);
        }


    }


    
    public void ShowMergedNodes(int w, int h, List<StageNode> nodes)
    {
        // Destroy
        for(int i=0;i<t_viewer_merged.childCount;i++)
        {
            Destroy(t_viewer_merged.GetChild(i).gameObject);
        }

        // Generate
        lineUIs_merged = new();
        for(int i=0;i<h;i++)
        {
            NodeLineUI lineUI = Instantiate(prefab_nodeLine,t_viewer_merged).GetComponent<NodeLineUI>();
            lineUI.Init(w);
            lineUI.transform.SetAsFirstSibling();

            lineUIs_merged.Add(lineUI);
        }

        // Allocate
        Color color = colors[8];
        foreach(StageNode node in nodes)
        {
            NodeUI nodeUI = lineUIs_merged[node.level].nodeUIs[node.number];
            nodeUI.SetColor( colors[(int) node.type]);
        }

        // draw line
        foreach(StageNode startNode in nodes.Where(x=>x.level ==0))
        {
            List<StageNode> nextNodes = nodes.Where(x=> startNode.nextNodes.Contains(x.id)).ToList();
            foreach(StageNode nextNode in nextNodes)
            {
                RecursiveDrawLine(nodes, startNode, nextNode,color);
            }
        }
    }
    

    void RecursiveDrawLine(List<StageNode> nodes, StageNode currNode, StageNode nextNode,Color color)
    {
        // 1. 두 노드를 잇는다. 
        RectTransform rt1 = lineUIs_merged[currNode.level].nodeUIs[currNode.number].GetComponent<RectTransform>();
        RectTransform rt2 = lineUIs_merged[nextNode.level].nodeUIs[nextNode.number].GetComponent<RectTransform>();


        lineRenderer.DrawLines(rt1,rt2,color,t_lineParent_merged);

        // 2.s
        List<StageNode> nextnextNodes = nodes.Where( x => nextNode.nextNodes.Contains( x.id ) ).ToList();
        foreach( StageNode nextnextNode in nextnextNodes)
        {
            RecursiveDrawLine( nodes, nextNode, nextnextNode, color);
        }
    }
}
