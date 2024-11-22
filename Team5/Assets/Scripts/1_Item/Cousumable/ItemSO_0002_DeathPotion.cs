using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0002_DeathPotion", menuName = "SO/ConsumableItem/0002", order = int.MaxValue)]
public class ItemSO_0002_DeathPotion : ConsumableItemSO
{
    public override string id => "0002";
    public override string dataName => "RestoreHp";

    
    public override void Consume(float value)
    {
        Player.Instance.GetDamaged(defaultValue);
    }
}
