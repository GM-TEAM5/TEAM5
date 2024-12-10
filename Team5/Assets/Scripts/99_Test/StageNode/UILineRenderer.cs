using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : MonoBehaviour
{
    public GameObject prefab_line;

    public Transform t_lineParent;

    public float width = 5;


    public void DrawLines(List<RectTransform> points, Color color)
    {
        for(int i=0;i<t_lineParent.childCount;i++)
        {
            Destroy(t_lineParent.GetChild(i).gameObject);
        }
        
        
        for(int i=0;i<points.Count-1;i++)
        {
            StartCoroutine(DelayedDraw(points[i], points[i+1],color ));
        }
    }

    public void DrawLine(RectTransform rt1, RectTransform rt2, Color color)
    {
        Vector3 posA = rt1.anchoredPosition + rt1.parent.GetComponent<RectTransform>().anchoredPosition;
        Vector3 posB = rt2.anchoredPosition + rt2.parent.GetComponent<RectTransform>().anchoredPosition;


        // 스크린 상 거리 계산
        float distance = Vector2.Distance(posA, posB);


        // 3. 각도 계산
        Vector3 direction = posB - posA;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // 4. 선 프리팹 생성
        RectTransform line = Instantiate(prefab_line, t_lineParent).GetComponent<RectTransform>(); // Canvas의 자식으로 생성
        line.sizeDelta = new Vector2( width, distance); // 선의 길이 설정 (width 변경)
        line.position = rt1.position; // 선의 중심을 두 점 사이로 설정
        line.rotation = Quaternion.Euler(0, 0, angle ); // 선의 회전 설정

        line.GetComponent<Image>().color= color;
    }
    

    IEnumerator DelayedDraw(RectTransform rt1, RectTransform rt2, Color color)
    {
        yield return null;
        DrawLine(rt1, rt2,color );

    }


}
