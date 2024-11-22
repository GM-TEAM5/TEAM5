using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1008_TestPDmgUp3", menuName = "SO/EquipmentItem/1008", order = int.MaxValue)]
public class ItemSO_1008_TestPDmgUp3 : EquipmentItemSO
{
    public override string id => "1008";
    public override string dataName => "물리 공격력 향상 3";

    public int amount = 777;

    

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
        GameEventManager.Instance.onChangePlayerStatus_pDmg.Invoke();
    }

}
