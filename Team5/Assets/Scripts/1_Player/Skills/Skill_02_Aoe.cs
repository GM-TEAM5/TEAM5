using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Skill_02_Aoe", menuName = "SO/PlayerSkill/02")]
public class Skill_02_Aoe : PlayerSkillSO
{
    private Transform drawArea;
    private Transform brush;
    private float rangeRadius;
    private Collider brushCollider;
    private bool isDrawing = false;

    [Header("Effect")]
    [SerializeField] private float explosionDelay = 0.05f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float skillDamage = 10f;
    [SerializeField] private bool isEnhancedAttack = true;
    [SerializeField] private ParticleSystem explosionEffectPrefab;

    [Header("Line")]
    [SerializeField] private float lineWidth = 1f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float segmentDistance = 0.05f;

    private List<Vector3> linePositions = new List<Vector3>();
    private LineRenderer mainLine;
    private GameObject lineContainer;
    private Coroutine skillCoroutine;
    private float initialLength;
    private int explosionIndex = 0;

    public override void On()
    {
        isDrawing = false;
        linePositions.Clear();
        ClearLine();

        drawArea = Player.Instance.t_player.Find("DrawArea");
        drawArea.gameObject.SetActive(true);
        rangeRadius = 10f;

        brush = Player.Instance.t_player.Find("Brush");
        brushCollider = brush.GetComponentInChildren<Collider>();

        if (lineContainer == null)
        {
            lineContainer = new GameObject("LineContainer");
            mainLine = lineContainer.AddComponent<LineRenderer>();
            SetupLineRenderer(mainLine);
        }
    }

    private void SetupLineRenderer(LineRenderer line)
    {
        line.material = lineMaterial;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 0;
        line.useWorldSpace = true;
    }

    private void ClearLine()
    {
        if (lineContainer != null)
        {
            Destroy(lineContainer);
            mainLine = null;
        }
    }

    public override void Off()
    {
        if (linePositions.Count > 1)
        {
            explosionIndex = 0;
            initialLength = GetTotalLineLength();
            StartExplosionSequence();
        }

        drawArea.gameObject.SetActive(false);
        brush.gameObject.SetActive(false);
        brushCollider.enabled = false;
    }

    public override void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos)
    {
        if (isMouseLeftButtonOn)
        {
            Brushing(mouseWorldPos);

            if (!isDrawing)
            {
                StartBrushing();
                isDrawing = true;
            }

            RecordLinePosition();
            UpdateLine();
        }
        else if (isDrawing)
        {
            StopBrushing();
            isDrawing = false;
            Off();
        }
    }

    private float GetTotalLineLength()
    {
        float length = 0;
        for (int i = 0; i < linePositions.Count - 1; i++)
        {
            length += Vector3.Distance(linePositions[i], linePositions[i + 1]);
        }
        return length;
    }

    private void StartBrushing()
    {
        brush.gameObject.SetActive(true);
        brushCollider.enabled = true;
        linePositions.Clear();
        mainLine.positionCount = 0;
    }

    private void Brushing(Vector3 mouseWorldPos)
    {
        mouseWorldPos.y = 0.1f;

        Vector3 direction = mouseWorldPos - Player.Instance.t_player.position;
        float distance = direction.magnitude;
        bool isWithinRange = distance <= rangeRadius;

        if (!isWithinRange)
        {
            direction.Normalize();
            mouseWorldPos = Player.Instance.t_player.position + direction * rangeRadius;
        }
        brush.position = mouseWorldPos;
    }

    private void StopBrushing()
    {
        brush.gameObject.SetActive(false);
        brushCollider.enabled = false;
    }

    private void RecordLinePosition()
    {
        if (linePositions.Count == 0 ||
            Vector3.Distance(linePositions[linePositions.Count - 1], brush.position) > segmentDistance)
        {
            linePositions.Add(brush.position);
        }
    }

    private void UpdateLine()
    {
        if (mainLine != null && linePositions.Count > 0)
        {
            mainLine.positionCount = linePositions.Count;
            mainLine.SetPositions(linePositions.ToArray());
        }
    }

    private void StartExplosionSequence()
    {
        if (skillCoroutine == null)
        {
            skillCoroutine = Player.Instance.StartCoroutine(ExplosionCoroutine());
        }
    }

    private IEnumerator ExplosionCoroutine()
    {
        float currentLength = 0;

        for (int i = 0; i < linePositions.Count - 1; i++)
        {
            Vector3 explosionPos = linePositions[i];
            Vector3 nextPos = linePositions[i + 1];

            // 폭발 이펙트 생성
            if (explosionEffectPrefab != null)
            {
                ParticleSystem explosion = Instantiate(explosionEffectPrefab, explosionPos, Quaternion.identity);
                explosion.Play();
                float duration = explosion.main.duration;
                Destroy(explosion.gameObject, duration);
            }

            // 대미지 적용
            Collider[] hitColliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null && enemy.isAlive)
                {
                    enemy.GetDamaged(explosionPos, skillDamage, isEnhancedAttack);
                }
            }

            // 라인 업데이트 - 폭발 지점까지의 라인을 제거하고 나머지만 표시
            List<Vector3> remainingPositions = new List<Vector3>();
            remainingPositions.Add(explosionPos); // 현재 폭발 지점을 시작점으로
            remainingPositions.AddRange(linePositions.GetRange(i + 1, linePositions.Count - (i + 1))); // 나머지 점들 추가

            mainLine.positionCount = remainingPositions.Count;
            mainLine.SetPositions(remainingPositions.ToArray());

            yield return new WaitForSeconds(explosionDelay);
        }

        // 마지막 폭발
        if (linePositions.Count > 0)
        {
            Vector3 lastPos = linePositions[linePositions.Count - 1];
            if (explosionEffectPrefab != null)
            {
                ParticleSystem explosion = Instantiate(explosionEffectPrefab, lastPos, Quaternion.identity);
                explosion.Play();
                Destroy(explosion.gameObject, explosion.main.duration);
            }

            mainLine.positionCount = 0;
        }

        // 모든 폭발이 끝나면 정리
        Destroy(lineContainer);
        lineContainer = null;
        mainLine = null;
        linePositions.Clear();
        Off();
        skillCoroutine = null;
    }
}