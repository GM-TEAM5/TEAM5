using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬은 장착 아이템이면서도, 사용 효과가 있는 아이템임. 
/// </summary>
public abstract class SkillItemSO : EquipmentItemSO
{
    public abstract void Use();

}
