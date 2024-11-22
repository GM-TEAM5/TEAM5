using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System;




public class AreaIndicator : MonoBehaviour, IPoolObject
{   

    // [SerializeField] SpriteRenderer sr_maxProgress;
    AreaIndicatorSO areaIndicatorData;
    
    public Vector2 initSize;
    public SpriteRenderer sr_currProgress;
    public SpriteRenderer sr_outline;
    public Color defaultColor;
    public Color emphasisColor;

    public Sequence seq_fill;
    public  Sequence seq_appear;

    //=================================================================
    public void OnCreatedInPool()
    {
        
    }

    public void OnGettingFromPool()
    {
        
    }

    //=================================================================
    public void Init(AreaIndicatorSO data, Vector3 initPos, Vector3 targetPos, Vector2 size, float duration)
    {
        areaIndicatorData = data;
        initSize = size;
        transform.localScale = initSize;
        transform.position = initPos + new Vector3(0, 0.01f,0);
        // RotateObjectTowardsTarget(targetPos,initPos);
        data.OnInit(this);
        StartCoroutine(AreaRoutine(duration));
    }

    // void RotateObjectTowardsTarget(Vector3 targetPos,Vector3 initPos)
    // {
    // Vector3 direction = targetPos - transform.position;

    // // 2. 방향 벡터를 정규화
    // direction.Normalize();

    // // 3. 회전값 계산
    // Quaternion rotation = Quaternion.LookRotation(direction);

    // // 4. 추가 회전 적용 (Z축에서 X축으로 변환)
    // Quaternion adjustment = Quaternion.Euler(0f, -90f, 0f);

    // // 5. 초기 회전값 `(90, 0, 0)` 적용
    // Quaternion initialRotation = Quaternion.Euler(90f, 0f, 0f);

    // // 6. 최종 회전값 계산
    // transform.rotation = rotation * adjustment * initialRotation;
    // }


    IEnumerator AreaRoutine(float duration)
    {
        seq_appear = areaIndicatorData.PlaySeq_Appear(this, duration * 0.25f);
        seq_fill = areaIndicatorData.PlaySeq_Fill(this, duration);
        yield return new WaitWhile(( )=>seq_fill.IsActive() );

        // 풀로반환
        PoolManager.Instance.TakeToPool<AreaIndicator>(this);
    }


}
