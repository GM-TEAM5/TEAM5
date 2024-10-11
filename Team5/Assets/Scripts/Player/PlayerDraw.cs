using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer))]
public class PlayerDraw : MonoBehaviour
{
    PlayerInputManager inputManager;

    [Header("Sketch")]
    public float sketchLineWidth = 0.1f;
    public Material sketchLineMaterial;

    private bool isDrawing = false;
    private List<Vector3> points = new List<Vector3>();
    private LineRenderer lineRenderer;

    void Start()
    {
        // 입력 설정
        inputManager = PlayerInputManager.Instance;

        // Line 설정
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = sketchLineWidth;
        lineRenderer.endWidth = sketchLineWidth;
        lineRenderer.material = sketchLineMaterial;
    }

    void Update()
    {
        // 그림 그리기 여부에 따라 처리
        if (inputManager.drawAction.ReadValue<float>() > 0)
        {
            if (!isDrawing)
            {
                StartDrawing();
            }
            Drawing();
        }
        else if (isDrawing)
        {
            StopDrawing();
        }
    }

    // 그리기 시작
    void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.positionCount = 0;
        points.Clear();
    }

    // 그리기
    void Drawing()
    {
        Vector3 mouseWorldPos = inputManager.mouseWorldPos;
        mouseWorldPos.y = 0.1f;
        points.Add(mouseWorldPos);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, mouseWorldPos);
    }

    // 그리기 종료
    void StopDrawing()
    {
        isDrawing = false;

        // 기본 공격 선 생성
        if (points.Count > 0)
        {
            // TODO: 공격 선 생성
        }
        lineRenderer.positionCount = 0;
        points.Clear();
    }
}
