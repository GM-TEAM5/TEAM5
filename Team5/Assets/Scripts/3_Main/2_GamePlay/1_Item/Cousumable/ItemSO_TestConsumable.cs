using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConumeableItem_0000_TestConsumable", menuName = "SO/ConsumableItem/0000_TestConsumable", order = int.MaxValue)]
public class ItemSO_TestConsumable : ConsumableItemSO
{
    public override string id => "0000";
    public override string dataName => "TestConsumable";

    
    public override void Consume(float value)
    {
        // 아무것도 안함.
    }
}
