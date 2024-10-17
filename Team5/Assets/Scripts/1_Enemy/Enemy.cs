using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

using Redcode.Pools;
using DG.Tweening;



[RequireComponent(typeof(NavMeshAgent),
                    typeof(SpriteEntity))]
public class Enemy : MonoBehaviour, IPoolObject
{
    public EnemySO enemyData;   //적의 데이터
    EnemyStateUI stateUI;
    SpriteEntity spriteEntity;
    NavMeshAgent navAgent;
    Collider enemyCollider;

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
        // Init(); // 초반에만, -  
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

    //===========================
     
    /// <summary>
    ///  이게 init 보다 먼저 호출됨.
    /// </summary>
    public void OnCreatedInPool()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stateUI = GetComponent<EnemyStateUI>();
        spriteEntity = GetComponent<SpriteEntity>();
        enemyCollider = GetComponent<Collider>();
    }

    public void OnGettingFromPool()
    {
        
    }


   //=====================================
    /// <summary>
    /// 척 스텟 초기화 - pool에서 생성되거나, 재탕될 때 호출됨. 
    /// </summary>
    /// <param name="enemyData"></param>
    public void Init(EnemySO enemyData)
    {
        //
        enemyCollider.enabled = true;
        navAgent.isStopped = false;
        t_target = Player.Instance.transform;
        
        //
        this.enemyData = enemyData;
        hp = enemyData.maxHp;
        navAgent.speed = enemyData.movementSpeed;
        // data 에 따라 radius 및 이동속도 도 세팅해야함. 

        //
        stateUI.Init(this);
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
        enemyCollider.enabled = false;       // 적 탐색 및 총알 충돌에 걸리지 않도록.
        navAgent.isStopped = true;          // 이동중지

        //
        PlaySequence_Death();   //

        stateUI.OnDie();
    }

    //==================================================
    /// <summary>
    /// 적 사망 애니메이션을 재생하고, 해당 애니메이션이 종료후 오브젝트를 제거한다. 
    /// </summary>
    void PlaySequence_Death()
    {
        DOTween.Sequence()
        .OnComplete( ()=> PoolManager.Instance.TakeToPool<Enemy>(this) )
        .Append(spriteEntity.spriteRenderer.DOFade(0,1f))
        .Play();
    }

}
