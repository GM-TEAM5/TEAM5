using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "CircleAreaIndicator", menuName = "SO/AreaIndicator/Circle", order = int.MaxValue)]
public class CircleAreaIndicator : AreaIndicatorSO
{
    public override string id => "00";

    public override string dataName => "CircleArea";

    public override void OnInit(AreaIndicator areaIndicator)
    {
        
    }

    public override DG.Tweening.Sequence PlaySeq_Appear(AreaIndicator areaIndicator, float duration)
    {
        areaIndicator.transform.localScale = new Vector3(0,0,1f);
        var seq = DOTween.Sequence()
        .Append(areaIndicator.transform.DOScale(areaIndicator.initSize , duration).SetEase(Ease.OutCirc))
        .Play();

        return seq;
    }

    public override DG.Tweening.Sequence PlaySeq_Fill(AreaIndicator areaIndicator,float duration)
    {
        var sr_currProgress = areaIndicator.sr_currProgress;
        var sr_outline = areaIndicator.sr_outline;
        var defaultColor = areaIndicator.defaultColor;
        var emphasisColor = areaIndicator.emphasisColor;
        
        sr_currProgress.transform.localScale = new Vector3(0,0,1);
        
        sr_currProgress.color = defaultColor;
        sr_outline.color = sr_currProgress.color.WithAlpha(1);
        

        //
        var seq = DOTween.Sequence()
        .Append(sr_currProgress.transform.DOScale(1f,duration).SetEase(Ease.Linear))
        .Append(sr_currProgress.DOColor(emphasisColor,0.2f))
        .Append(sr_currProgress.DOColor(defaultColor,0.2f))
        .Join(sr_outline.DOFade(0f,1.5f))
        .Join(sr_currProgress.DOFade(0f,1.5f))
        .Play();

        return seq;
    }


    // void OnGUI () 
    // {
    //     if (GUI.Button (new Rect (20,20, 100, 100), "클릭버튼")) 
    //     {
    //         Init(Vector3.zero, 1f);

    //     }
    // }
}
