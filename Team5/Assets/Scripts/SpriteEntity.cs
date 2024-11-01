using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class SpriteEntity : MonoBehaviour
{
    [SerializeField] Material spriteShadow;
    
    public SpriteRenderer spriteRenderer;

    [SerializeField] Transform t_sprite;
    [SerializeField] Transform t_camera;


    public void Init()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = Color.white;     // 풀에서 다시 가져올 때 투명한 상태기 때문에. 
        
        spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
        spriteRenderer.material = spriteShadow;

        t_sprite = spriteRenderer.transform;
        t_camera = Camera.main.transform;
    }

    /// <summary>
    /// 지정된 스프라이트, 크기로 초기화
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="entitySize"></param>
    public void Init(Sprite sprite, float entitySize,float entityHeight)
    {
        Init(entitySize,entityHeight);

        spriteRenderer.sprite = sprite;
    }

    /// <summary>
    /// 지정된 크기로 초기화 ( 스프라이트는 컴파일 때의 스프라이트로 사용 ) - 플레이어 초기화용
    /// </summary>
    /// <param name="entityWidth"></param>
    public void Init(float entityWidth, float entityHeight)
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = Color.white;     // 풀에서 다시 가져올 때 투명한 상태기 때문에. 
        
        spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
        spriteRenderer.material = spriteShadow;

        t_sprite = spriteRenderer.transform;
        t_camera = Camera.main.transform;

        t_sprite.localPosition = new Vector3(0,0,-entityWidth);
        
        //
        // Canvas enemyCanvas = GetComponentInChildren<Canvas>();
        // if (enemyCanvas != null)
        // {
        //     enemyCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, entityHeight) * 1/transform.localScale.x ;
        // }
    }

    void Update()
    {              
        Billboard();
    }

    //==========================================================================================================

    /// <summary>
    /// 스프라이트가 항상 카메라를 정면으로 보도록 회전시킴. 
    /// </summary>
    void Billboard()
    {
        t_sprite.rotation = Quaternion.LookRotation(t_sprite.position - t_camera.position);
        t_sprite.rotation = Quaternion.Euler(t_sprite.rotation.eulerAngles.x,0,0);
    }

    /// <summary>
    /// dirX 방향으로 뒤집는다.  ( 오른쪽이 기본값 )
    /// </summary>
    /// <param name="dirX"></param>
    public void Flip(float dirX)
    {
        if(GameManager.isPaused)
        {
            return;
        } 
        if( dirX!=0)
        {
            spriteRenderer.flipX = dirX<0;
        }
        
    }

    
}
