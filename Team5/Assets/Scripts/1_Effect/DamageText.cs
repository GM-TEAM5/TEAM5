using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;


public enum damageType
{
    DMG_NORMAL,
    DMG_CRITICAL,
    DMG_TICK,
    DMG_PLAYER
}

public class DamageText : MonoBehaviour, IPoolObject
{
    TextMeshPro text; 
    

    //================================================================
    public void OnCreatedInPool()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void OnGettingFromPool()
    {
        text.color = Color.white;   // 사라질때 페이드인되기 때문에 다시 색을 바꿔줘야함. 
    }


    public void Init(Vector3 hitPoint, float damage)
    {
        transform.position = hitPoint + new Vector3(Random.Range(-1,1), 0, 0); 
        
        text.SetText( damage.ToString("0"));

        PlayAnim_MoveAndFade();
    }

 
    void PlayAnim_MoveAndFade()
    {
        DOTween.Sequence()
        .OnComplete( ()=>PoolManager.Instance.TakeToPool<DamageText>(this))
        .Append(transform.DOLocalMoveY(3, 0.5f))
        .Append(text.DOFade(0,0.5f))
        .Play();
    }
}
