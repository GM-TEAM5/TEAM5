using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1006_TestMDmgUp2", menuName = "SO/EquipmentItem/1006", order = int.MaxValue)]
public class ItemSO_1006_TestMDmgUp2 : EquipmentItemSO
{
    public override string id => "1006";
    public override string dataName => "마법 공격력 향상 2";

    public float amount = 10;

    public override void OnEquip()
    {
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.mDmg, amount, true);      
    }
    
    public override void OnUnEquip()
    {
        // 아무것도 안함
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.mDmg, amount, false);      
    }


    protected override void EquipEvent(bool isEquip)
    {
        GameEventManager.Instance.onChangePlayerStatus_mDmg.Invoke();
    }
}
