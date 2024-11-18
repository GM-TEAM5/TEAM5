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
    public void Init(AreaIndicatorSO data, Vector3 initPos, Vector2 size, float duration)
    {
        areaIndicatorData = data;
        initSize = size;
        transform.localScale = initSize;
        transform.position = initPos + new Vector3(0, 0.01f,0);
        StartCoroutine(AreaRoutine(duration));
    }


    IEnumerator AreaRoutine(float duration)
    {
        seq_appear = areaIndicatorData.PlaySeq_Appear(this, duration * 0.25f);
        seq_fill = areaIndicatorData.PlaySeq_Fill(this, duration);
        yield return new WaitWhile(( )=>seq_fill.IsActive() );

        // 풀로반환
        PoolManager.Instance.TakeToPool<AreaIndicator>(this);
    }


}
