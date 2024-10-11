using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // CapsuleCollider collider;
    SpriteRenderer spriteRenderer;
    


    public void Init(Vector3 targetPos)
    {
        // collider = GetComponent<CapsuleCollider>();
        
        // data 에 따라 radius 및 이동속도 도 세팅해야함. 
        Sprite sprite= TestManager.Instance.initProjSprite;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprite;


        // 목표 방향을 계산합니다.
        Vector3 fixedTargetPos = new Vector3(targetPos.x,0,targetPos.z);
        Vector3 fixedProjPos = new Vector3(transform.position.x,0,transform.position.z);

        Vector3 dir = (fixedTargetPos - fixedProjPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Quaternion fixedRotation = Quaternion.Euler(90, lookRotation.eulerAngles.y,0 );
        transform.rotation = fixedRotation;


        // 발사
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 10);
    }
}
