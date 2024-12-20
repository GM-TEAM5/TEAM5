using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1005_TestPDmgUp2", menuName = "SO/EquipmentItem/1005", order = int.MaxValue)]
public class ItemSO_1005_TestPDmgUp2 : EquipmentItemSO
{
    public override string id => "1005";
    public override string dataName => "물리 공격력 향상 2";

    public int amount = 10;

    

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
    //     GameEventManager.Instance.onChangePlayerStatus_pDmg.Invoke();
    }

}
