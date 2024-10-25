using UnityEngine;

[CreateAssetMenu(fileName = "Skill_01_Brush", menuName = "SO/PlayerSkill/01")]
public class Skill_01_Brush : PlayerSkillSO
{
    private Transform drawArea;
    private Transform brush;
    private float rangeRadius;
    private Collider brushCollider;
    private TrailRenderer brushTrail;
    bool isDrawing = false;

    public override void On()
    {
        isDrawing = false;

        // 그리기 영역 설정
        drawArea = Player.Instance.t_player.Find("DrawArea");
        drawArea.gameObject.SetActive(true);
        rangeRadius = 10f;

        // 붓칠 충돌 설정
        brush = Player.Instance.t_player.Find("Brush");
        brushCollider = brush.GetComponentInChildren<Collider>();
        brushTrail = brush.GetComponentInChildren<TrailRenderer>();
    }

    public override void Off()
    {
        drawArea.gameObject.SetActive(false);
        brush.gameObject.SetActive(false);
        brushCollider.enabled = false;
        brushTrail.enabled = false;
    }

    public override void Use(bool isMouseLeftButtonOn, Vector3 mouseWorldPos)
    {
        // 그림 그리기 여부에 따라 처리
        if (isMouseLeftButtonOn)
        {
            // 공격 실행
            Brushing(mouseWorldPos);

            if (!isDrawing)
            {
                StartBrushing();
                isDrawing = true;
            }
        }
        else
        {
            StopBrushing();
            isDrawing = false;
        }
    }

    public void StartBrushing()
    {
        brush.gameObject.SetActive(true);
        brushTrail.Clear();
        brushCollider.enabled = true;
        brushTrail.enabled = true;
    }

    public void Brushing(Vector3 mouseWorldPos)
    {
        // 마우스 영역 가져오기
        mouseWorldPos.y = 0.1f;

        // 붓칠 범위
        Vector3 direction = mouseWorldPos - Player.Instance.t_player.position;
        float distance = direction.magnitude;
        bool isWithinRange = distance <= rangeRadius;

        // 범위를 벗어나면 중심에서 반지름만큼 이동
        if (!isWithinRange)
        {
            direction.Normalize();
            mouseWorldPos = Player.Instance.t_player.position + direction * rangeRadius;
        }
        brush.position = mouseWorldPos;
    }

    // 그리기 종료
    public void StopBrushing()
    {
        brush.gameObject.SetActive(false);
        brushCollider.enabled = false;
        brushTrail.enabled = false;
    }
}
