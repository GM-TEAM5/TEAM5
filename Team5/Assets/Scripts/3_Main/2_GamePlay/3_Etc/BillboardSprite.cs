using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] Transform t_sprite;
    [SerializeField] Transform t_camera;
    
    void Start()
    {
        Init();        
    }

    void Update()
    {
        Billboard();
    }

    //======================================================
    public void Init()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        t_sprite = spriteRenderer.transform;
        t_camera = Camera.main.transform;

    }

    

     /// <summary>
    /// 스프라이트가 항상 카메라를 정면으로 보도록 회전시킴. 
    /// </summary>
    void Billboard()
    {
        if (t_sprite == null)
        {
            return;
        }
        t_sprite.rotation = Quaternion.LookRotation(t_sprite.position - t_camera.position);
        t_sprite.rotation = Quaternion.Euler(t_sprite.rotation.eulerAngles.x,0,0);
    }

}
