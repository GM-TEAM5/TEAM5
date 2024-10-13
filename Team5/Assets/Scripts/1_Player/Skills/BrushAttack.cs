using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAttack : MonoBehaviour
{
    public static float duration = 2f;
    public static float maxWidth = 0.5f;
    public static float growRatio = 0.8f;
    public LayerMask enemyLayer;

    // 선을 생성하는 함수
    public static void CreateBrushLine(GameObject prefab, Vector3[] points)
    {
        GameObject lineObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // LineRenderer 설정
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // 애니메이션 시작
        lineObject.AddComponent<AttackAnimator>().StartAnimation(lineRenderer);
    }
}

// 공격 애니메이션
class AttackAnimator : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public void StartAnimation(LineRenderer renderer)
    {
        lineRenderer = renderer;
        StartCoroutine(AnimateLine());
    }

    private IEnumerator AnimateLine()
    {
        float elapsedTime = 0f;
        float growDuration = BrushAttack.duration * BrushAttack.growRatio;
        float fadeDuration = BrushAttack.duration * (1 - BrushAttack.growRatio);

        while (elapsedTime < BrushAttack.duration)
        {
            float t = elapsedTime / BrushAttack.duration;
            float width;
            float alpha;

            if (elapsedTime < growDuration)
            {
                // 선이 두꺼워지는 단계
                width = Mathf.Lerp(0.1f, BrushAttack.maxWidth, elapsedTime / growDuration);
                alpha = 1f;
            }
            else
            {
                width = BrushAttack.maxWidth;
                float fadeT = (elapsedTime - growDuration) / fadeDuration;
                alpha = Mathf.Lerp(1f, 0f, Mathf.Pow(fadeT, 2));
            }

            // 선의 굵기 적용
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;

            // 알파값 적용
            Color newColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, alpha);
            lineRenderer.startColor = newColor;
            lineRenderer.endColor = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 오브젝트 제거
        Destroy(gameObject);
    }
}