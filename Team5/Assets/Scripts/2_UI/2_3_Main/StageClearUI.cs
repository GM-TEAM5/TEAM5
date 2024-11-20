using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageClearUI : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI text;
    // public float bootingTime;
    
    public Sequence startSequence;


    public Sequence GetSeq_StageClear()
    {
        gameObject.SetActive(true);
        
        img.color = new Color(1,1,1,0);
        text.color = new Color(1,1,1,0);
        //

        //
        startSequence = DOTween.Sequence()
        .OnComplete( ()=>{
            gameObject.SetActive(false);
        }) 
        .Append(img.DOFade(1f,0.5f))
        //
        .Append(text.DOFade(1f,0.3f))
        //
        .AppendInterval(1f)
        //
        .Append(img.DOFade(0f,1f))
        .Join(text.DOFade(0f,1f));
        
        //
        return startSequence;
    }
}
