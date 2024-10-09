using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteEntity : MonoBehaviour
{
    [SerializeField] Material spriteShadow;
    
    SpriteRenderer spriteRenderer;

    Transform t_sprite;
    Transform t_camera;

    /// <summary>
    /// 지정된 스프라이트, 크기로 초기화
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="entitySize"></param>
    public void Init(Sprite sprite, float entitySize)
    {
        Init(entitySize);

        spriteRenderer.sprite = sprite;
    }

    /// <summary>
    /// 지정된 크기로 초기화 ( 스프라이트는 컴파일 때의 스프라이트로 사용 ) - 플레이어 초기화용
    /// </summary>
    /// <param name="entitySize"></param>
    public void Init(float entitySize)
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
        spriteRenderer.material = spriteShadow;

        t_sprite = spriteRenderer.transform;
        t_camera = Camera.main.transform;

        t_sprite.localPosition = new Vector3(0,0,-entitySize);
    }

    void Update()
    {              
        Billboard();
    }

    /// <summary>
    /// 스프라이트가 항상 카메라를 정면으로 보도록 회전시킴. 
    /// </summary>
    void Billboard()
    {
        t_sprite.rotation = Quaternion.LookRotation(t_sprite.position - t_camera.position);
        t_sprite.rotation = Quaternion.Euler(t_sprite.rotation.eulerAngles.x,0,0);
    }
}
