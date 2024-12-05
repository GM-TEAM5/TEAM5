using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PlayerDraw : MonoBehaviour, ITimeScaleable
{
    [Header("Drawing Settings")]
    [SerializeField] DrawingArea drawingArea;
    [SerializeField] GameObject drawingCamera;
    [SerializeField] float minDistance = 0.1f;
    [SerializeField] float intensity_onDraw = 0.35f;
    [SerializeField] Transform lineContainer;

    private LineRenderer currentLine;
    private List<Vector3> currentPositions = new List<Vector3>();
    public bool isDrawing { get; private set; }
    public bool isInDrawMode { get; private set; }
    private PlayerInputManager playerInput;
    private Player player;
    private float timeScale = 1f;
    private DrawAttackSO currentSkill;

    public void Init()
    {
        playerInput = PlayerInputManager.Instance;
        player = Player.Instance;
        drawingCamera.SetActive(false);
        drawingArea.Init();
    }

    public void Equip(DrawAttackSO skill)
    {
        currentSkill = skill;
    }

    public void UnEquip()
    {
        currentSkill = null;
    }

    public void TryDraw()
    {
        if (isInDrawMode)
        {
            FinishDraw();
        }
        else
        {
            StartDrawing();
        }
    }

    public void StartDrawing()
    {
        if (isInDrawMode || currentSkill == null) return;

        isInDrawMode = true;
        float finalRadius = player.status.drawRange * currentSkill.effectRadius;
        drawingArea.SetRadius(finalRadius * 2f);

        drawingArea.Activate();
        DirectingManager.Instance.SetVignette(intensity_onDraw, 0.5f);

        drawingCamera.SetActive(true);
        Time.timeScale = 0.05f;
        Player.Instance.SetTimeScale(1f);
        TestManager.Instance.TestSFX_RyoikiTenkai(true);
    }

    public void FinishDraw()
    {
        if (isDrawing)
        {
            EndDrawing();
        }
        DirectingManager.Instance.InitVignette(0.5f);

        drawingCamera.SetActive(false);
        Time.timeScale = 1f;
        Player.Instance.SetTimeScale(1f);

        drawingArea.Deactivate();
        isInDrawMode = false;
        TestManager.Instance.TestSFX_RyoikiTenkai(false);
    }

    private void EndDrawing()
    {
        if (currentLine != null)
        {
            StartCoroutine(SlashRoutine(currentLine, new List<Vector3>(currentPositions)));
        }

        isDrawing = false;
        currentLine = null;
        currentPositions.Clear();
    }

    public void OnUpdate()
    {
        if (!isInDrawMode) return;

        float deltaTime = Time.deltaTime * timeScale;

        if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
            FinishDraw();
            return;
        }

        if (Input.GetMouseButtonDown(0) && player.HasEnoughInk(GetMinInkRequired(currentSkill)))
        {
            StartLine();
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueDrawing();
        }
    }

    private void StartLine()
    {
        if (!player.HasEnoughInk(GetMinInkRequired(currentSkill)))
        {
            Debug.Log("Not enough ink to start line!");
            return;
        }

        Vector3 mousePos = GetMouseWorldPosition();

        if (!IsInsideDrawingArea(mousePos))
        {
            return;
        }

        GameObject newLine = new GameObject("DrawingLine");
        newLine.transform.SetParent(lineContainer);
        currentLine = newLine.AddComponent<LineRenderer>();
        SetupLine(currentLine);

        currentPositions.Clear();
        isDrawing = true;

        AddPoint(GetMouseWorldPosition());
    }

    private void ContinueDrawing()
    {
        if (!isDrawing || currentPositions.Count == 0) return;

        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 lastPos = currentPositions[currentPositions.Count - 1];

        if (!IsInsideDrawingArea(mousePos))
        {
            return;
        }

        float distance = Vector3.Distance(lastPos, mousePos);
        if (distance > minDistance)
        {
            float inkCost = distance * (currentSkill?.inkCostPerUnit ?? 1f);

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

    private float GetMinInkRequired(DrawAttackSO skill)
    {
        return skill?.minInkRequired ?? 10f;
    }

    private void SetupLine(LineRenderer line)
    {
        if (currentSkill != null)
        {
            line.material = currentSkill.lineMaterial != null ?
                currentSkill.lineMaterial :
                new Material(Shader.Find("Sprites/Default"));
            line.startColor = line.endColor = currentSkill.lineColor;
            line.startWidth = line.endWidth = currentSkill.lineWidth;
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

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }

    private bool IsInsideDrawingArea(Vector3 position)
    {
        Vector3 playerPos = Player.Instance.transform.position;
        float distanceFromCenter = Vector2.Distance(
            new Vector2(position.x, position.z),
            new Vector2(playerPos.x, playerPos.z)
        );

        float actualRadius = (drawingArea.TargetRadius / 2f) - 0.2f;
        return distanceFromCenter < actualRadius;
    }

    protected IEnumerator SlashRoutine(LineRenderer line, List<Vector3> positions)
    {
        float elapsedTime = 0f;
        Color startColor = line.startColor;
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        Debug.Log("Positions count: " + positions.Count);
        foreach (var pos in positions)
        {
            Debug.Log("Position: " + pos);
        }

        while (elapsedTime < currentSkill.slashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = 1f - (elapsedTime / currentSkill.slashDuration);
            Color fadeColor = startColor;
            fadeColor.a = alpha;
            line.startColor = line.endColor = fadeColor;

            for (int i = 0; i < positions.Count - 1; i++)
            {
                RaycastHit[] hits = Physics.BoxCastAll(
                    positions[i],
                    Vector3.one * currentSkill.effectRadius,
                    (positions[i + 1] - positions[i]).normalized,
                    Quaternion.identity,
                    Vector3.Distance(positions[i], positions[i + 1]),
                    LayerMask.GetMask("Enemy")
                );

                foreach (var hit in hits)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null && !damagedEnemies.Contains(enemy))
                    {
                        float dmg = currentSkill.defaultDamage + Player.Instance.status.mDmg;
                        Debug.Log($"Damaging enemy: {enemy.name} with {dmg} damage");
                        enemy.GetDamaged(dmg);
                        damagedEnemies.Add(enemy);
                    }
                }
            }

            yield return null;
        }

        Destroy(line.gameObject);
    }
}
