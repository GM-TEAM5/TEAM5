using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public Image img;

    public void Init()
    {
        img = GetComponent<Image>();

        img.color = Color.black;
    }


    public void SetColor(Color color)
    {   
        img.color = color;
    }
}
