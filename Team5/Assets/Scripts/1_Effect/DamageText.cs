using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;


public enum DamageType
{
    DMG_NORMAL,
    DMG_CRITICAL,
    DMG_TICK,
    DMG_PLAYER,
    HEAL_PLAYER
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


    public void Init(Vector3 hitPoint, float damage, DamageType type = DamageType.DMG_NORMAL)
    {
        transform.position = hitPoint + new Vector3(Random.Range(-0.5f,0.5f), 0, 0); 
        
        text.SetText( damage.ToString("0"));
        Color textColor = Color.white;
        switch(type)
        {
            case DamageType.DMG_CRITICAL:
                textColor = new Color(1,1,0.5f);
                break;
            case DamageType.DMG_PLAYER:
                textColor = new Color(1,0.2f,0.2f);
                break;
            case DamageType.HEAL_PLAYER:
                textColor = new Color(0,1,0.5f);
                break;
        }
        text.color = textColor;
        PlayAnim_MoveAndFade();
    }

        public void Init(Vector3 hitPoint, string  content, DamageType type = DamageType.DMG_NORMAL)
    {
        transform.position = hitPoint + new Vector3(Random.Range(-0.5f,0.5f), 0, 0); 
        
        text.SetText( content);
        Color textColor = new Color(1,0.2f,0.2f);
        text.color = textColor;
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
