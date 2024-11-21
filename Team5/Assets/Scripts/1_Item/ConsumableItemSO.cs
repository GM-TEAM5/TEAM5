using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItemSO : ItemDataSO
{
    public ConsumableItemSO ()
    {
        type = ItemType.Consumable;
    }
    
    
    
    public float defaultValue;        // 소비아이템 효과 기본 값
    
    protected override bool CanGet(out CantGetReason reason)
    {        
        reason = CantGetReason.None;
        return true;
    }
    
    protected override void Get()
    {        
        Consume(defaultValue);      // 획득 즉시 사용 효과 발동 
    }

    protected override void OnCantGet(CantGetReason reason)
    {
        
    }


    public abstract void Consume(float value);
}
