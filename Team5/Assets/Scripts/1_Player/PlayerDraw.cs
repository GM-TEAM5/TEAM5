using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public enum DrawType
{
    GroundPattern,  // E 스킬용 바닥 패턴
    QuickSlash      // Q 스킬용 빠른 슬래시
}

public class PlayerDraw : MonoBehaviour
{
    [Header("Drawing Settings")]
    [SerializeField] DrawingArea drawingArea;
    [SerializeField] GameObject drawingCamera;
    [SerializeField] float minDistance = 0.1f;
    [SerializeField] float intensity_onDraw = 0.35f;

    private LineRenderer currentLine;
    private List<Vector3> currentPositions = new List<Vector3>();
    public bool isDrawing { get; private set; }
    public bool isInDrawMode { get; private set; }
    private PlayerInputManager playerInput;
    private Player player;
    private DrawType currentDrawType;
    private System.Action<LineRenderer, List<Vector3>> onDrawComplete;
    private SkillItemSO currentSkill;

    public void Init()
    {
        playerInput = PlayerInputManager.Instance;
        player = Player.Instance;
        drawingCamera.SetActive(false);
        drawingArea.Init();
    }

    void Start()
    {
        Init();
    }

    public void StartDrawing(DrawType drawType, SkillItemSO skill, System.Action<LineRenderer, List<Vector3>> callback)
    {
        if (!player.HasEnoughInk(GetMinInkRequired(skill)))
        {
            Debug.Log("Not enough ink to start drawing!");
            return;
        }

        currentSkill = skill;
        currentDrawType = drawType;
        onDrawComplete = callback;
        isInDrawMode = true;
        drawingArea.Activate();
        drawingCamera.SetActive(true);
        DirectingManager.Instance.SetVignette(intensity_onDraw, 0.5f);

        if (currentDrawType == DrawType.QuickSlash)
        {
            Time.timeScale = 0.05f;
            Player.Instance.SetTimeScale(1f);
        }
    }

    public void FinishDraw()
    {
        if (isDrawing)
        {
            EndDrawing();
        }
        DirectingManager.Instance.InitVignette(0.5f);
        drawingCamera.SetActive(false);
        drawingArea.Deactivate();
        isInDrawMode = false;
        onDrawComplete = null;

        Time.timeScale = 1f;
        Player.Instance.SetTimeScale(1f);
    }

    private void EndDrawing()
    {
        if (currentLine != null && onDrawComplete != null)
        {
            onDrawComplete.Invoke(currentLine, new List<Vector3>(currentPositions));
        }

        isDrawing = false;
        currentLine = null;
        currentPositions.Clear();
    }

    public void OnUpdate()
    {
        if (!isInDrawMode) return;

        if (Input.GetMouseButtonDown(0) && player.HasEnoughInk(GetMinInkRequired(currentSkill)))
        {
            StartLine();
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }
    }

    private void StartLine()
    {
        if (!player.HasEnoughInk(GetMinInkRequired(currentSkill)))
        {
            Debug.Log("Not enough ink to start line!");
            return;
        }

        GameObject newLine = new GameObject("DrawingLine");
        newLine.transform.SetParent(drawingArea.transform);
        currentLine = newLine.AddComponent<LineRenderer>();
        SetupLine(currentLine);

        currentPositions.Clear();
        isDrawing = true;

        AddPoint(GetMouseWorldPosition());
    }

    private void ContinueDrawing()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        if (currentPositions.Count == 0) return;

        Vector3 lastPos = currentPositions[currentPositions.Count - 1];
        float distance = Vector3.Distance(lastPos, mousePos);

        if (distance > minDistance)
        {
            var drawableSkill = currentSkill as IDrawableSkill;
            float inkCost = distance * (drawableSkill?.inkCostPerUnit ?? 1f);

            if (player.HasEnoughInk(inkCost))
            {
                AddPoint(mousePos);
                player.UseInk(inkCost);
            }
            else
            {
                Debug.Log("Out of ink!");
                EndDrawing();
            }
        }
    }

    private void AddPoint(Vector3 point)
    {
        currentPositions.Add(point);
        currentLine.positionCount = currentPositions.Count;
        currentLine.SetPosition(currentPositions.Count - 1, point);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = playerInput.mouseWorldPos;
        mousePos.y = 0.1f;
        return mousePos;
    }

    private float GetMinInkRequired(SkillItemSO skill)
    {
        var drawableSkill = skill as IDrawableSkill;
        return drawableSkill?.minInkRequired ?? 10f;
    }

    private void SetupLine(LineRenderer line)
    {
        var drawableSkill = currentSkill as IDrawableSkill;
        if (drawableSkill != null)
        {
            line.material = drawableSkill.lineMaterial != null ?
                drawableSkill.lineMaterial :
                new Material(Shader.Find("Sprites/Default"));
            line.startColor = line.endColor = drawableSkill.lineColor;
            line.startWidth = line.endWidth = drawableSkill.lineWidth;
        }
        else
        {
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = line.endColor = Color.black;
            line.startWidth = line.endWidth = 0.2f;
        }
        line.positionCount = 0;
        line.useWorldSpace = true;
    }
}

