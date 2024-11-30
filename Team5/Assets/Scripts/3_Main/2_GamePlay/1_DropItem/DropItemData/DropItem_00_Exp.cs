using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DropItem_00_Exp", menuName = "SO/DropItem/00", order = int.MaxValue)] 
public class DropItem_00_Exp : DropItemDataSO
{
    public override void PickUp(float value)
    {
        // Player.Instance.GetExp(value);
    }
}
