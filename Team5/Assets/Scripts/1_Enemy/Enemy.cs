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

    public bool isAlive => _hp>0;    

    //===============================================================

    void Start()
    {
        Init(); // 초반에만, -  
    }
    
    void Update()
    {
        if(isAlive)
        {
            navAgent.SetDestination(t_target.position);
        }
        

        // navAgent.isStopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            GetDamaged(10);
        }
        else if (other.CompareTag("Brush"))
        {
            GetDamaged(100);
        }
    }




    public void Init()
    {
        hp = enemyData.maxHp;

        GetComponent<Collider>().enabled = true;


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
            Die();
            
        }

        // ui
        stateUI.UpdateCurrHp(hp);


        // 데미지 텍스트 생성
        Instantiate( TestManager.Instance.damageText).Init(transform, damage);  // 추후 이벤트로 뺄거임
    }

    public void GetHealed(float heal)
    {
        hp += heal;

        stateUI.UpdateCurrHp(hp);
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;       // 적 탐색 및 총알 충돌에 걸리지 않도록.
        navAgent.isStopped = true;          // 이동중지

        Debug.Log($"{enemyData.entityName} 사망");
    }
}
