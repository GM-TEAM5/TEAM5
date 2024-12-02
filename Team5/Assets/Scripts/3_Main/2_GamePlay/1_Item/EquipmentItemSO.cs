using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItemSO : ItemDataSO
{
    
    
    public EquipmentItemSO()
    {
        type = ItemType.Equipment;
    }

    protected override bool CanGet(out CantGetReason reason)
    {        
        reason = CantGetReason.None;
        if (Player.Instance.equipments.HasEmptySpace()==false)
        {
            reason = CantGetReason.NoSpace;
        }


        return reason == CantGetReason.None;
    }

    protected override void Get()
    {
        if (Player.Instance.equipments.TryEquip(this) ==false)
        {
            Debug.Log("엥");
        }
    }

    protected override void OnCantGet(CantGetReason reason)
    {
        switch(reason)
        {
            case CantGetReason.NoSpace:
                GamePlayManager.Instance.OnInventoryFull(this);
                break;
        }
    }

    //=====================================
    /// <summary>
    /// 장착 효과 없이 장비 장착
    /// </summary>
    public void InitEquip()
    {
        OnEquip();
        
    }

    public void Equip()
    {
        OnEquip();  // 장착효과
        EquipEvent(true);
    }


    public void UnEquip()
    {
        //플레이어 장비창에서 사라짐. 획득 능력치는삭제
        OnUnEquip(); // 장착효과 해제
        EquipEvent(false);
    }
    //=====================================

    public abstract void OnEquip();
    public abstract void OnUnEquip();

    protected abstract void EquipEvent(bool isEquip);
}
