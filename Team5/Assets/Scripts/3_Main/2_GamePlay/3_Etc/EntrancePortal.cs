using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class EntrancePortal : MonoBehaviour
{
    Transform t_camera;
    Transform t_sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        t_camera = Camera.main.transform;
        t_sprite = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Billboard();
    }

        void Billboard()
    {
        t_sprite.rotation = Quaternion.LookRotation(t_sprite.position - t_camera.position);
        t_sprite.rotation = Quaternion.Euler(t_sprite.rotation.eulerAngles.x,0,0);
    }


    public Sequence GetSeq_GeneratePortal(float playTime)
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1,1,1,0);

        return DOTween.Sequence()
        .Append( sr.DOFade(1f, playTime));
    }

    public Sequence PlaySeq_DestroyPortal(float playTime)
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;

        return DOTween.Sequence()
        .OnComplete( ()=>{Destroy(gameObject);})
        .Append( sr.DOFade(0f, playTime))
        .Play();
    }
}
