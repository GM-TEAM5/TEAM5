using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public abstract class AreaIndicatorSO : GameData
{
    
    
    public void PlaySeq(AreaIndicator areaIndicator, float duration)
    {
        if (areaIndicator.seq_appear!=null && areaIndicator.seq_appear.IsActive())
        {
            areaIndicator.seq_appear.Kill();
        }
        if (areaIndicator.seq_fill!=null && areaIndicator.seq_fill.IsActive())
        {
            areaIndicator.seq_fill.Kill();
        }
        areaIndicator.seq_appear = PlaySeq_Appear(areaIndicator, duration * 0.25f);
        areaIndicator.seq_fill = PlaySeq_Fill(areaIndicator,duration);
    }
    public abstract void OnInit(AreaIndicator areaIndicator);
    public abstract Sequence PlaySeq_Appear(AreaIndicator areaIndicator, float duration);
    public abstract Sequence PlaySeq_Fill(AreaIndicator areaIndicator, float duration);

}
