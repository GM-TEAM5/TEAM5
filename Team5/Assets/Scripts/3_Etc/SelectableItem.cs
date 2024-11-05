using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(BillboardSprite))]
public class SelectableItem : MonoBehaviour
{
    public TextMeshPro debugText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Init(int i =0)
    {
        debugText.SetText(i.ToString());
    }
}
