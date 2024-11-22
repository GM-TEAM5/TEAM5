using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1007_TestHpUp3", menuName = "SO/EquipmentItem/1007", order = int.MaxValue)]
public class ItemSO_1007_TestHpUp2 :  EquipmentItemSO
{

    public override string id => "1007";
    public override string dataName => "체력 향상 3";

    public int amount = 7777;

    // public float targetField => Player.Instance.status.maxHp;

    //
    public override void OnEquip()
    {
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.Inc_maxHp, amount, true);   
    }

    public override void OnUnEquip()
    {
        // 아무것도 안함
        Player.Instance.status.ChangeStatus(ref Player.Instance.status.Inc_maxHp, amount, false); 
    }

    protected override void EquipEvent(bool isEquip)
    {
        GameEventManager.Instance.onChangePlayerStatus_maxHp.Invoke();
        // 장착의 경우엔 피회복
        if (isEquip)
        {
            Player.Instance.GetHealed(amount);
        }
    }
}