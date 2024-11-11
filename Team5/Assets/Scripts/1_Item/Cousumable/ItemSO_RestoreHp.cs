using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConumeableItem_0001_RestoreHp", menuName = "SO/ConsumableItem/0001_RestoreHp", order = int.MaxValue)]
public class ItemSO_RestoreHp : ConsumableItemSO
{
    
    public override string id => "0001";
    public override string dataName => "RestoreHp";

    
    public override void Consume(float value)
    {
        Player.Instance.GetHealed(value);
    }
}
