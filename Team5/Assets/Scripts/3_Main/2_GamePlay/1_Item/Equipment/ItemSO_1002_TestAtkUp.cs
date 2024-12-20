using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "EquipmentItem_1002_TestAtkUp", menuName = "SO/EquipmentItem/1002_TestAtkUp", order = int.MaxValue)]
public class ItemSO_1002_TestAtkUp : EquipmentItemSO
{
    public override string id => "1002";
    public override string dataName => "물리 공격력 향상 1";

    public int amount = 5;

    

    //
    public override void OnEquip()
    {
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.pDmg, amount, true);      
    }
    
    public override void OnUnEquip()
    {
        // 아무것도 안함
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.pDmg, amount, false);      
    }


    protected override void EquipEvent(bool isEquip)
    {
        // GameEventManager.Instance.onChangePlayerStatus_pDmg.Invoke();
    }


}
