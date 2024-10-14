using UnityEngine;

public class BrushAttack : MonoBehaviour
{
    [Header("Draw Area")]
    public Transform drawArea;
    private Transform brush;

    private float rangeRadius;
    private Collider brushCollider;
    private TrailRenderer brushTrail;

    void Start()
    {
        brush = drawArea.Find("Brush");

        // 붓칠 충돌 설정
        brushCollider = drawArea.GetComponentInChildren<Collider>();
        brushTrail = drawArea.GetComponentInChildren<TrailRenderer>();

        // 공격 범위 계산
        rangeRadius = drawArea.localScale.x + drawArea.localScale.y;
    }

    public void StartBrushing()
    {
        brushCollider.enabled = true;
        brushTrail.enabled = true;
        brushTrail.Clear();
    }

    public void Brushing(Vector3 mouseWorldPos)
    {
        // 마우스 영역 가져오기
        mouseWorldPos.y = 0.1f;

        // 붓칠 범위
        Vector3 direction = mouseWorldPos - drawArea.position;
        float distance = direction.magnitude;
        bool isWithinRange = distance <= rangeRadius;

        // 범위를 벗어나면 중심에서 반지름만큼 이동
        if (!isWithinRange)
        {
            direction.Normalize();
            mouseWorldPos = drawArea.position + direction * rangeRadius;
        }
        brush.position = mouseWorldPos;
    }

    // 그리기 종료
    public void StopBrushing()
    {
        brushCollider.enabled = false;
        brushTrail.enabled = false;
    }
}
