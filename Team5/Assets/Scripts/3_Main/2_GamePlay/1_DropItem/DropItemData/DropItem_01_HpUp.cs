using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_01_HpUp", menuName = "SO/DropItem/01", order = int.MaxValue)]
public class DropItem_01_HpUp : DropItemDataSO
{
    public override void PickUp(float value)
    {
        Player.Instance.GetHealed(value);
    }
}
