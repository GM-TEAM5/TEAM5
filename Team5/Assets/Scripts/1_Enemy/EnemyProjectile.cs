using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class EnemyProjectile : MonoBehaviour, IPoolObject
{
    Transform t_sprite;
    Transform t_camera;
    
    public Collider projCollider;
    public Rigidbody rb;

    public float damage;
    public float speed;
    public float lifeTime;

    Coroutine destroyRoutine;
    
    //=======================================================
    void Update()
    {
        Billboard();
    }

        void Billboard()
    {
        t_sprite.rotation = Quaternion.LookRotation(t_sprite.position - t_camera.position);
        t_sprite.rotation = Quaternion.Euler(t_sprite.rotation.eulerAngles.x,0,0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectile(0); 
        }
    }


    /// <summary>
    /// 투사체 초기화
    /// </summary>
    /// <param name="skillData"></param> 크기 및 스프라이트 설정
    /// <param name="enemy"></param>      데미지 설정
    /// <param name="initPos"></param>     초기위치
    /// <param name="lifeTime"></param>     수명
    public void Init(EnemySkillSO skillData, Enemy enemy,Vector3 initPos, float lifeTime)
    {
        damage = enemy.enemyData.ad;
        this.lifeTime = lifeTime;

        //
        transform.position = initPos;
        DestroyProjectile(lifeTime);

        t_camera = Camera.main.transform;
        t_sprite = GetComponentInChildren<SpriteRenderer>().transform;
    }

    public void SetDirAndSpeed(Vector3 dir,float speed)
    {
        this.speed = speed;
        rb.velocity = dir * speed;
    }

    //================================================

    public void OnCreatedInPool()
    {
        projCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnGettingFromPool()
    {
        projCollider.enabled = true;
    }

    //================================================

    public void DestroyProjectile(float delay)
    {
        if(destroyRoutine!=null)
        {
            StopCoroutine(destroyRoutine);
        }
        destroyRoutine = StartCoroutine( DelayedDestroy(delay));
    }

    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        projCollider.enabled = false;
        PoolManager.Instance.TakeToPool<EnemyProjectile>(this);
    }

}
