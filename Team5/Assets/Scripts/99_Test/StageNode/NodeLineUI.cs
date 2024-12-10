using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLineUI : MonoBehaviour
{
    public GameObject prefab_node;


    public List<NodeUI> nodeUIs;
    

    public void Init(int w)
    {
        // Destroy
        for(int i=0;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        
        // Generate
        nodeUIs = new();
        for(int i=0;i<w;i++)
        {
            NodeUI nodeUI = Instantiate(prefab_node,transform).GetComponent<NodeUI>();
            nodeUI.Init();
            nodeUIs.Add(nodeUI);
        }
    }
}
