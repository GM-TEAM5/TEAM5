using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNodeViewer : MonoBehaviour
{
    public GameObject prefab_line;


    public Transform t_viewer;

    public List<NodeLineUI> lineUIs;

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


    public void ShowStageNodes(int w, int h, List<List<int>> nodes)
    {
        // Destroy
        for(int i=0;i<t_viewer.childCount;i++)
        {
            Destroy(t_viewer.GetChild(i).gameObject);
        }

        // Generate
        lineUIs = new();
        for(int i=0;i<h;i++)
        {
            NodeLineUI lineUI = Instantiate(prefab_line,t_viewer).GetComponent<NodeLineUI>();
            lineUI.Init(w);
            lineUI.transform.SetAsFirstSibling();

            lineUIs.Add(lineUI);
        }

        // Allocate
        for(int i=0;i<nodes.Count;i++)
        {
            List<RectTransform> points= new();

            Color color = colors[i];
            for(int j=0;j<nodes[i].Count;j++)
            {  
                int nodeIdx = nodes[i][j]; 
                NodeUI node = lineUIs[j].nodeUIs[nodeIdx];
                node.SetColor(color);

                points.Add( node.GetComponent<RectTransform>());
            }
            lineRenderer.DrawLines(points,color);
        }


    }
    
}
