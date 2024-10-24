using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class GamePlayStartUI : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI text_stageNum;
    [SerializeField] TextMeshProUGUI text_stageStart;
    public float bootingTime;
    
    public Sequence startSequence;


    public Sequence StartGamePlaySequence()
    {
        gameObject.SetActive(true);
        
        img.color = new Color(1,1,1,0);
        text_stageNum.color = new Color(1,1,1,0);
        text_stageStart.color = new Color(1,1,1,0);
        //

        //
        startSequence = DOTween.Sequence()
        .OnComplete( ()=>{
            gameObject.SetActive(false);
        }) 
        .Append(img.DOFade(1f,0.5f))
        //
        .Append(text_stageNum.DOFade(1f,0.5f))
        .Join(text_stageStart.DOFade(1f,0.5f ))
        //
        .AppendInterval(1f)
        //
        .Append(img.DOFade(0f,1f))
        .Join(text_stageStart.DOFade(0f,1f ))
        .Join(text_stageNum.DOFade(0f,1f))
        //
        .AppendInterval(1f)
        .Play();

        //
        return startSequence;
    }


    
}
