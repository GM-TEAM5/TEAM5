using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_03_Ink", menuName = "SO/DropItem/03", order = int.MaxValue)]
public class DropItem_03_Ink : DropItemDataSO
{
    public override void PickUp(float value)
    {
        Player.Instance.GetInk(value);
    }
}
