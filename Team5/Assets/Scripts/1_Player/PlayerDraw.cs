using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDraw : MonoBehaviour
{
    [SerializeField] DrawingArea drawingArea;
    [SerializeField] GameObject drawingCamera;

    public bool isDrawing;
    
    //======================================================


    public void Init()
    {
        drawingCamera.SetActive(false);
        drawingArea.Init();
    }


    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(isDrawing == false)
            {
                StartDraw();
            }
            else
            {
                FinishDraw();
            }
        }
    }

    public void StartDraw() 
    {
        isDrawing = true;
        drawingArea.Activate();
        drawingCamera.SetActive(true);

    }

    public void FinishDraw()
    {
        
        drawingCamera.SetActive(false);
        drawingArea.Deactivate();
        isDrawing = false;
    }
}
