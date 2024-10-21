using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_02_Money", menuName = "SO/DropItem/02", order = int.MaxValue)]
public class DropItem_02_Money : DropItemDataSO
{
    public override void PickUp(float value)
    {
        // Player.Instance.GetHealed(value);
    }
}
