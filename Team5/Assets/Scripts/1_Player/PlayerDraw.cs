using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDraw : MonoBehaviour
{
    [SerializeField] DrawingArea drawingArea;
    [SerializeField] GameObject drawingCamera;

    [SerializeField] float intensity_onDraw = 0.35f;

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
        DirectingManager.Instance.SetVignette(intensity_onDraw,0.5f);

        // 그리고 ui 사라지게

    }

    public void FinishDraw()
    {
        DirectingManager.Instance.InitVignette(0.5f);
        drawingCamera.SetActive(false);
        drawingArea.Deactivate();
        isDrawing = false;


        // 그리고 ui 다시 생기게 
    }

    //=========================================
}

