using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItem_1000_TestEquipment", menuName = "SO/EquipmentItem/1000_TestEquipment", order = int.MaxValue)]
public class ItemSO_TestEquipment : EquipmentItemSO
{
    public override string id => "1000";
    public override string dataName => "TestEquipment";

    //
    public override void OnEquip()
    {
        // 아무것도 안함
    }

    public override void OnUnEquip()
    {
        // 아무것도 안함
    }
}
