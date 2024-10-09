using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    float _hp;
    public float hp  // 현재체력
    {
        get => _hp;
        set 
        {
            _hp = Math.Clamp(value,0,maxHp);
        }        
    }       
    public float maxHp = 500;     // 최대체력
    public float ad = 100;     // 공격력
    public float armor;     // 방어력
    public float movementSpeed = 10;    // 이동속도
    public float brushRange; // 그리기 사용 가능 범위  
    public float inkCap;   // 잉크 양


    public PlayerStatus()
    {
        hp = maxHp;     
    }

    //====================================
}
