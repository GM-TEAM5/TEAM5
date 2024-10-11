using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


[RequireComponent(typeof(NavMeshAgent),
                    typeof(SpriteEntity))]
public class Enemy : MonoBehaviour
{
    public EnemySO enemyData;   //적의 데이터
    EnemyStateUI stateUI;
    SpriteEntity spriteEntity;
    NavMeshAgent navAgent;
    Transform t_target;


    [SerializeField] float _hp;
    public float hp  // 현재체력
    {
        get => _hp;
        set 
        {
            _hp = Math.Clamp(value,0,enemyData.maxHp);
        }        
    }      

    //===============================================================

    void Start()
    {
        Init(); // 초반에만, -  
    }
    
    void Update()
    {
        // navAgent.SetDestination(t_target.position);

        // navAgent.isStopped = true;
    }


    public void Init()
    {
        hp = enemyData.maxHp;

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = enemyData.movementSpeed;
        // data 에 따라 radius 및 이동속도 도 세팅해야함. 
        t_target = Player.Instance.transform;

        stateUI = GetComponent<EnemyStateUI>();
        stateUI.Init(this);

        spriteEntity = GetComponent<SpriteEntity>();
        spriteEntity.Init(enemyData.sprite, navAgent.radius, navAgent.height);
        
    }

    public void GetDamaged(float damage)
    {
        hp -= damage;

        if (hp <=0)
        {
            Debug.Log($"{enemyData.entityName} 사망");
        }

        // ui
        stateUI.UpdateCurrHp(hp);
    }

    public void GetHealed(float heal)
    {
        hp += heal;

        stateUI.UpdateCurrHp(hp);
    }
}
