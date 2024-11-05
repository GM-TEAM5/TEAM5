using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItemSO : ItemDataSO
{
    public override void Get()
    {
        Equip();        // 장비아이템 획득 시 자동으로 장착.
    }

    public void Equip()
    {
        //플레이어 장비창으로 들어가야지. 능력치 수정. 

    
        OnEquip();  // 장착효과
    }


    public void UnEquip()
    {
        //플레이어 장비창에서 사라짐. 획득 능력치는삭제

        OnUnEquip(); // 장착효과 해제
    }


    public abstract void OnEquip();
    public abstract void OnUnEquip();
}
