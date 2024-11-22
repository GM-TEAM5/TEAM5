using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "RectDirAreaIndicator", menuName = "SO/AreaIndicator/RectDir", order = int.MaxValue)]
public class RectDirAreaIndicator : AreaIndicatorSO
{
    public override string id => "01";

    public override string dataName => "RectDirArea";

    public override void OnInit(AreaIndicator areaIndicator)
    {
        
    }

    public override Sequence PlaySeq_Appear(AreaIndicator areaIndicator, float duration)
    {
        // z 스케일 ++;
        var sr_currProgress = areaIndicator.sr_currProgress;
        var sr_outline = areaIndicator.sr_outline;

        sr_currProgress.color = new Color(1,0,0,0);
        sr_outline.color = new Color(1,0,0,0);

        var seq = DOTween.Sequence()
        .Append(sr_currProgress.DOFade(1f, duration).SetEase(Ease.OutCirc))
        .Join(sr_outline.DOFade(1f, duration).SetEase(Ease.OutCirc))
        .Play();

        return seq;
    }

    public override Sequence PlaySeq_Fill(AreaIndicator areaIndicator, float duration)
    {
        // z 스케일 ++;
        var sr_currProgress = areaIndicator.sr_currProgress;
        var sr_outline = areaIndicator.sr_outline;

        sr_currProgress.color = new Color(1,0,0,0);
        sr_outline.color = new Color(1,0,0,0);

        var seq = DOTween.Sequence()
        .Append(sr_currProgress.DOFade(1f, duration).SetEase(Ease.OutCirc))
        .Join(sr_outline.DOFade(1f, duration).SetEase(Ease.OutCirc))
        .Play();

        return seq;
    }
}
