using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipmentItem_1001_TestHpUp", menuName = "SO/EquipmentItem/1001_TestHpUp", order = int.MaxValue)]
public class ItemSO_1001_TestHpUp : EquipmentItemSO
{

    public override string id => "1001";
    public override string dataName => "체력 향상 1";

    public int amount = 100;

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
