using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    //
    public int level = 1;
    public float currExp;
    public float maxExp = 100;
    
    //
    [SerializeField] float _hp;
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

    // 붓칠 관련 변수
    public float drawRange; // 그리기 사용 가능 범위  
    public float maxInk = 100f;   // 최대 잉크 양
    public float currInk;   // 현재 잉크 양
    public float inkUseRate = 20f;    // 초당 잉크 사용량
    public float inkChargeRate = 20f; // 초당 잉크 충전량

    public PlayerStatus()
    {
        hp = maxHp;     
        currInk = maxInk;
    }

    //====================================
}
