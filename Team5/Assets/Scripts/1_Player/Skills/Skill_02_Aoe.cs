using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Skill_02_Aoe", menuName = "SO/PlayerSkill/02")]
public class Skill_02_Brush : PlayerSkillSO
{
    // 그리는 영역 설정
    private Transform drawArea;
    private float rangeRadius;
    private Transform brush;
    private TrailRenderer brushTrail;
    private bool isDrawing = false;
    private bool canDrawing = true;
    private List<Vector3> drawnPoints = new List<Vector3>();
    private float pointInterval = 0.2f;
    private Vector3 lastPoint;

    // TrailRenderer 설정
    private float trailDuration = 2f;

    // 폭발 설정
    private float damage = 10f;
    private float explosionInterval = 0.5f;
    private float explosionRadius = 1f;

    // 폭발 파티클
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem explosionParticlePrefab;
    private float particleLifetime = 1f;

    public override void On()
    {
        // 그리기 영역 설정
        drawArea = Player.Instance.t_player.Find("DrawArea");
        drawArea.gameObject.SetActive(true);
        rangeRadius = 10f;

        // 붓칠 설정
        brush = Player.Instance.t_player.Find("Aoe");
        brushTrail = brush.GetComponentInChildren<TrailRenderer>();

        // TrailRenderer 설정
        brushTrail.time = trailDuration;
    }

    public override void Off()
    {
        drawArea.gameObject.SetActive(false);
    }

    public override void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos)
    {
        if (isMouseLeftButtonOn && canDrawing)
        {
            if (!isDrawing)
            {
                StartDrawing(mouseWorldPos);
            }
            else
            {
                UpdateDrawing(mouseWorldPos);
            }
        }
        else
        {
            StopDrawing();
        }
    }

    private void StartDrawing(Vector3 mouseWorldPos)
    {
        brush.gameObject.SetActive(true);
        brushTrail.Clear();

        isDrawing = true;
        drawnPoints.Clear();
    }

    private void UpdateDrawing(Vector3 mouseWorldPos)
    {
        // 마우스 영역 가져오기
        mouseWorldPos.y = 0.1f;

        // 붓칠 범위
        Vector3 direction = mouseWorldPos - Player.Instance.transform.position;
        float distance = direction.magnitude;
        bool isWithinRange = distance <= rangeRadius;

        // 범위를 벗어나면 중심에서 반지름만큼 이동
        if (!isWithinRange)
        {
            direction.Normalize();
            mouseWorldPos = Player.Instance.transform.position + direction * rangeRadius;
        }

        brush.position = mouseWorldPos;
        if (drawnPoints.Count == 0)
        {
            brushTrail.Clear();
        }

        if (Vector3.Distance(mouseWorldPos, lastPoint) >= pointInterval)
        {
            RecordPoint(mouseWorldPos);
        }
    }

    private void RecordPoint(Vector3 point)
    {
        drawnPoints.Add(point);
        lastPoint = point;
    }

    private void StopDrawing()
    {
        isDrawing = false;

        if (drawnPoints.Count > 0)
        {
            Player.Instance.StartCoroutine(FadeAndExplode());
        }

        brush.gameObject.SetActive(false);
        brushTrail.Clear();
    }

    private IEnumerator FadeAndExplode()
    {
        canDrawing = false;

        // 트레일이 사라지는 시간동안 대기 후 폭발
        float elapsedTime = 0f;
        int currentPointIndex = 0;

        while (elapsedTime < trailDuration)
        {
            // 폭발할 포인트 계산
            float explosionProgress = elapsedTime / trailDuration;
            int targetPointIndex = Mathf.FloorToInt(drawnPoints.Count * explosionProgress);

            // 새로운 포인트에 도달할 때마다 폭발
            while (currentPointIndex < targetPointIndex)
            {
                Vector3 explosionPoint = drawnPoints[currentPointIndex];
                CreateExplosion(explosionPoint);
                currentPointIndex++;
            }
            elapsedTime += explosionInterval;
            yield return new WaitForSeconds(explosionInterval);
        }
        drawnPoints.Clear();
        canDrawing = true;
    }

    private void CreateExplosion(Vector3 position)
    {
        // 파티클 생성
        if (explosionParticlePrefab != null)
        {
            ParticleSystem particleInstance = GameObject.Instantiate(explosionParticlePrefab, position, Quaternion.identity);
            particleInstance.Play();
            GameObject.Destroy(particleInstance.gameObject, particleLifetime);
        }

        // TODO: Enemy.cs로 이동 / 데미지 처리
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy.isAlive)
            {
                enemy.GetDamaged(position, damage);
            }
        }
    }
}