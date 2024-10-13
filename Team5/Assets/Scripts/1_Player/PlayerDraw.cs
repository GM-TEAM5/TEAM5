using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class PlayerDraw : MonoBehaviour
{
    PlayerInputManager inputManager;

    [Header("Sketch")]
    public float sketchLineWidth = 0.1f;
    public Material sketchLineMaterial;

    [Header("Prefab")]
    public GameObject basicAttachPrefab;

    [Header("Brush")]
    public float maxBrushVolume = 100f; // 최대 용량
    public float brushUseRate = 20f; // 초당 소모량
    public float brushChargeRate = 1f; // 초당 회복량
    private float currentBrushVolume;

    // TODO: PlayerStateUI로 이동
    public Slider brushBar;

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

        // 초기 붓칠 게이지 설정
        currentBrushVolume = maxBrushVolume;

        // 붓칠 게이지 UI 설정
        brushBar.maxValue = maxBrushVolume;
        brushBar.value = currentBrushVolume;
    }

    void Update()
    {
        // 붓칠 충전
        ChargeBrush();

        // 붓 게이지 UI 업데이트
        brushBar.value = currentBrushVolume;

        // 그림 그리기 여부에 따라 처리
        if (inputManager.drawAction.ReadValue<float>() > 0 && currentBrushVolume > 0)
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
        // 붓칠 게이지 소모
        UseBrush();

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
            BrushAttack.CreateBrushLine(basicAttachPrefab, points.ToArray());
        }
        lineRenderer.positionCount = 0;
        points.Clear();
    }

    // 붓칠 게이지 소모
    void UseBrush()
    {
        // 붓칠 게이지가 0이 되지 않도록 소모
        currentBrushVolume -= brushUseRate * Time.deltaTime;
        currentBrushVolume = Mathf.Max(currentBrushVolume, 0f);
    }

    // 붓칠 게이지 충전
    void ChargeBrush()
    {
        // 그리지 않고 있을 때만 충전
        if (!isDrawing && currentBrushVolume < maxBrushVolume)
        {
            currentBrushVolume += brushChargeRate * Time.deltaTime;
            currentBrushVolume = Mathf.Min(currentBrushVolume, maxBrushVolume);
        }
    }
}
