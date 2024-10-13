using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    TextMeshPro text; 
    
    public void Init(Transform t, float damage)
    {
        transform.position = new Vector3(t.position.x + Random.Range(-1,1), 1, t.position.z); 
        
        text = GetComponent<TextMeshPro>();
        text.SetText( damage.ToString("0"));

        PlayAnim_MoveAndFade();
    }

    void PlayAnim_MoveAndFade()
    {
        DOTween.Sequence()
        .Append(transform.DOLocalMoveY(3, 0.5f))
        .Append(text.DOFade(0,0.5f))
        .Play();
    }
}
