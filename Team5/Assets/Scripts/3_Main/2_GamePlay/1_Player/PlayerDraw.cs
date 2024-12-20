using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using BW;

public class PlayerDraw : MonoBehaviour, ITimeScaleable
{
    [Header("Drawing Settings")]
    [SerializeField] DrawingArea drawingArea;
    [SerializeField] GameObject drawingCamera;
    [SerializeField] float minDistance = 0.1f;
    [SerializeField] float intensity_onDraw = 0.35f;
    [SerializeField] Transform lineContainer;

    private List<(LineRenderer line, List<Vector3> positions)> currentLines = new List<(LineRenderer, List<Vector3>)>();
    private int previousFullSegments = 0;
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

    public void Equip(DrawAttackSO skill) => currentSkill = skill;
    public void UnEquip() => currentSkill = null;
    public void SetTimeScale(float scale) => timeScale = scale;

    /// <summary>
    /// 그리기 모드 전환 시도
    /// </summary>
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

    /// <summary>
    /// 그리기 모드 시작
    /// </summary>
    public void StartDrawing()
    {
        if (isInDrawMode || currentSkill == null)
        {
            return;
        }

        SetupDrawingMode(true);
        previousFullSegments = 0;
    }

    /// <summary>
    /// 그리기 모드 종료
    /// </summary>
    public void FinishDraw()
    {
        if (isDrawing)
        {
            EndDrawing();
        }
        SetupDrawingMode(false);
    }

    /// <summary>
    /// 그리기 모드 환경 설정
    /// </summary>
    private void SetupDrawingMode(bool activate)
    {
        isInDrawMode = activate;

        if (activate)
        {
            SoundManager.Instance.Invoke(Player.Instance.transform, SoundEventType.PlayerRyoikiTenkaiOn);

            float finalRadius = player.status.drawRange * currentSkill.effectRadius;
            drawingArea.SetRadius(finalRadius * 2f);
            drawingArea.Activate();
            DirectingManager.Instance.SetVignette(intensity_onDraw, 0.5f);
            drawingCamera.SetActive(true);
            Time.timeScale = 0.05f;
        }
        else
        {
            SoundManager.Instance.Invoke(Player.Instance.transform, SoundEventType.PlayerRyoikiTenkaiOff);

            DirectingManager.Instance.InitVignette(0.5f);
            drawingCamera.SetActive(false);
            Time.timeScale = 1f;
            drawingArea.Deactivate();
        }

        Player.Instance.SetTimeScale(1f);
    }

    /// <summary>
    /// 매 프레임 그리기 입력 처리
    /// </summary>
    public void OnUpdate()
    {
        if (!GamePlayManager.isGamePlaying)
        {
            if (isInDrawMode)
            {
                FinishDraw();
            }
            return;
        }

        if (!isInDrawMode)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
            FinishDraw();
            return;
        }

        if (Input.GetMouseButtonDown(0) && player.HasEnoughInk(currentSkill?.minInkRequired ?? 10f))
        {
            StartLine();
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueDrawing();
        }
    }

    /// <summary>
    /// 새로운 라인 시작
    /// </summary>
    private void StartLine()
    {
        if (currentLines.Count >= player.status.totalInkSegments)
        {
            return;
        }

        Vector3 mousePos = playerInput.mouseWorldPos;
        if (!IsInsideDrawingArea(mousePos))
        {
            return;
        }

        CreateNewLine();
        AddPoint(mousePos);
        isDrawing = true;
    }

    /// <summary>
    /// 새로운 LineRenderer 생성 및 초기화
    /// </summary>
    private void CreateNewLine()
    {
        GameObject newLine = new GameObject($"DrawingLine_{currentLines.Count + 1}");
        newLine.transform.SetParent(lineContainer);
        var line = newLine.AddComponent<LineRenderer>();
        SetupLine(line);
        currentLines.Add((line, new List<Vector3>()));
    }

    /// <summary>
    /// 라인 그리기 계속
    /// </summary>
    private void ContinueDrawing()
    {
        if (!isDrawing || currentLines.Count == 0)
        {
            return;
        }

        Vector3 mousePos = playerInput.mouseWorldPos;
        Vector3 lastPos = currentLines[^1].positions[^1];

        if (!IsInsideDrawingArea(mousePos))
        {
            return;
        }

        float distance = Vector3.Distance(lastPos, mousePos);
        if (distance <= minDistance)
        {
            return;
        }

        float inkCost = distance * (currentSkill?.inkCostPerUnit ?? 1f);
        if (!player.HasEnoughInk(inkCost))
        {
            EndDrawing();
            FinishDraw();
            return;
        }

        AddPoint(mousePos);
        player.UseInk(inkCost);
        CheckSegmentChange();
    }

    /// <summary>
    /// 그리기 종료 및 데미지 처리
    /// </summary>
    private void EndDrawing()
    {
        foreach (var lineData in currentLines)
        {
            StartCoroutine(SlashRoutine(lineData.line, new List<Vector3>(lineData.positions)));
        }

        isDrawing = false;
        currentLines.Clear();
    }

    /// <summary>
    /// 현재 라인에 새로운 점 추가
    /// </summary>
    private void AddPoint(Vector3 point)
    {
        if (currentLines.Count == 0)
        {
            return;
        }

        var currentLineData = currentLines[^1];
        currentLineData.positions.Add(point);
        currentLineData.line.positionCount = currentLineData.positions.Count;
        currentLineData.line.SetPosition(currentLineData.positions.Count - 1, point);
    }

    /// <summary>
    /// LineRenderer 컴포넌트 초기 설정
    /// </summary>
    private void SetupLine(LineRenderer line)
    {
        if (currentSkill != null)
        {
            line.material = currentSkill.lineMaterial ?? new Material(Shader.Find("Sprites/Default"));
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

    /// <summary>
    /// 주어진 위치가 그리기 영역 내부인지 확인
    /// </summary>
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

    /// <summary>
    /// 그려진 라인을 따라 데미지를 주는 코루틴
    /// </summary>
    protected IEnumerator SlashRoutine(LineRenderer line, List<Vector3> positions)
    {
        float elapsedTime = 0f;
        Color startColor = line.startColor;
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

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
                    Vector3.Distance(positions[i], positions[i + 1]), GameConstants.enemyLayer
                );

                foreach (var hit in hits)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null && !damagedEnemies.Contains(enemy))
                    {
                        float dmg = currentSkill.defaultDamage + Player.Instance.status.mDmg;
                        enemy.GetDamaged( positions[i], dmg );
                        damagedEnemies.Add(enemy);
                    }
                }
            }

            yield return null;
        }

        Destroy(line.gameObject);
    }

    private void CheckSegmentChange()
    {
        var (fullSegments, _) = player.status.GetInkSegmentInfo();
        if (fullSegments < previousFullSegments && currentLines.Count < player.status.totalInkSegments)
        {
            StartLine();
        }
        previousFullSegments = fullSegments;
    }
}
