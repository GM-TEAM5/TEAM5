using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof(SpriteEntity))]
public class DropItem : MonoBehaviour, IPoolObject
{
    public DropItemDataSO itemData;
    SpriteEntity spriteEntity;
    Rigidbody rb;
    Collider itemCollider;

    Transform t;


    float value; // 해당 아이템 효과의 수치 ( ex, 경험치 or 회복량 등)

    // 속도 관련
    float speed; // 아이템이 플레이어 쪽으로 이동하는 속도 
    readonly float initSpeed = 3;
    readonly float maxSpeed = 30;
    // float pickUpRange = 3f;
    // float weight_range = 0.5f;    
    Vector3 offset = new Vector3(0,0.5f,0);

    bool captured;   //

    bool inRange // 해당 아이템이 플레이어
    {
        get
        {
            float distSqr = Vector3.SqrMagnitude( Player.Instance.t_player.position+offset - t.position );
            float range= Player.Instance.status.range;
            float rangeSqr = range * range ;  // 일단 그리기 반경
            //Debug.Log($" item  {distSqr} {rangeSqr}" );
            //
            if ( distSqr <= rangeSqr  && captured == false) // 25는 획득범위의 제곱 - 나중에 수정해야함. 
            {
                // Debug.Log("캡처!");
                return true;
            }
            return false;

        }
    }



    //==============================================================================
    public void OnCreatedInPool()
    {
        spriteEntity = GetComponent<SpriteEntity>();
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();

        t = transform;
    }

    public void OnGettingFromPool()
    {
        captured = false;
        rb.velocity = Vector3.zero;
    }


    //====================================
    
    /// <summary>
    /// 초기화 - 
    /// </summary>
    /// <param name="itemData"></param> - 
    /// <param name="value"></param>    
    public void Init(DropItemDataSO itemData, float value, Vector3 initPos) 
    {
        this.itemData = itemData;
        this.value = value;

        speed = initSpeed;

        spriteEntity.Init(itemData.sprite, 0.5f,0.5f);
        
        
        transform.position = initPos + offset;
        
        Vector3 dir = new Vector3(Random.Range(-1,1 ),0,Random.Range(-1,1 ) ).normalized;
        rb.AddForce(dir*3f, ForceMode.Impulse);
    }


    void Update()
    {
        // 아이템이 범위 안인지
        if(inRange)
        {
            captured = true;
        }


        // 캡처시 플레이어쪽으로 이동
        if(captured)
        {
            
            // 방향구하기
            Vector3 dir = (Player.Instance.t_player.position+offset - t.position).normalized;
            
            rb.velocity = dir * speed;

            speed = Mathf.Lerp(speed, maxSpeed,Time.deltaTime);  // 아이템 속도가 점점 빨라짐
        }
    }



    //================================
    public void PickUp()
    {
        itemData.PickUp(value);


        PoolManager.Instance.TakeToPool<DropItem>(this);
    }
}
