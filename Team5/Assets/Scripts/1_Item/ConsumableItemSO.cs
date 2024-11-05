using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItemSO : ItemDataSO
{
    float defaultValue;        // 소비아이템 효과 기본 값
    
    
    public override void Get()
    {
        Consume(defaultValue);      // 획득 즉시 사용 효과 발동 
    }

    public abstract void Consume(float value);
}
