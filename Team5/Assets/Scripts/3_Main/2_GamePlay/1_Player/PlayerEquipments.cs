using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipments : MonoBehaviour
{
    //--------- equipments ------------
    // public SerializableDictionary<int, PlayerEquipment> equipments;
    public List<EquipmentItemSO> equipments;


    /// <summary>
    /// 플레이어가 초기화 될 때, 데이터를 갖고와 세팅한다. 
    /// </summary>
    public void InitEquipments()
    {
        List<EquipmentItemSO> equipmentData = GameManager.Instance.userData.equipments; 
        int len = equipmentData.Count;

        equipments = new();
        for(int i=0 ; i< len; i++)
        {
            equipments.Add(null);
            InitEquip(i, equipmentData[i]); // null 값 들어갈 수 있음. 
        }
    }


    //===========================================
    /// <summary>
    /// 첫번 째 빈공간 idx 를 반환.
    /// </summary>
    /// <returns></returns>
    public int GetFirstEmptySpaceIdx()
    {
        int idx = equipments.FindIndex( x=>x==null);
        return idx;
    }

    // 빈공간이 있는 지, 
    public bool HasEmptySpace()
    {
        int idx = GetFirstEmptySpaceIdx();
        
        if( idx != -1)
        {
            return true;
        }

        // 이미 꽉차있을 땐, 한개 버려야함. 
        return false;
    }

    //==============================================================

    public void InitEquip(int idx, EquipmentItemSO equipmentData)
    {
        equipments[idx] = equipmentData;
        if (equipmentData !=null)
        {
            equipmentData.InitEquip();
        }
    }

    /// <summary>
    /// 해당 슬롯 에 장착. 
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="equipmentData"></param>
    public void Equip(int idx, EquipmentItemSO equipmentData)
    {
        //
        EquipmentItemSO oldEquipment = equipments[idx];
        if (oldEquipment!=null)
        {
            oldEquipment.UnEquip();
        }
        
        //
        if (equipmentData !=null)
        {
            equipmentData.Equip();
        }
        
        //
        equipments[idx] = equipmentData;
    }

    

    /// <summary>
    /// 장비 장착 시도 - 아이템을 획득할 때,
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns></returns> 장비 장착에 성공했는지. - true만 그냥 끼는데, false 면 선택해야함. 
    public bool TryEquip(EquipmentItemSO equipment)
    {
        int idx = GetFirstEmptySpaceIdx();
        
        if( idx != -1)
        {
            Equip( idx, equipment);
            return true;
        }

        // 이미 꽉차있을 땐, 한개 버려야함. 
        return false;
    }



}
