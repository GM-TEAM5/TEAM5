using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq.Expressions;

public class DirectingManager : Singleton<DirectingManager>
{
    [SerializeField] CinemachineVirtualCamera zoomCamera;   
    public bool isZoomIn => zoomCamera.gameObject.activeSelf;

    //s
    [SerializeField] Image fade;
    [SerializeField] Color fadeColor;

    public bool isCompleted_fade;

    //
    [SerializeField] Volume volume;
    [SerializeField] Vignette vignette; 
    float intensity_default = 0.2f;




    //========================================
    
    void Start()
    {
        fade.color  = new Color(fadeColor.r,fadeColor.g,fadeColor.b,0);
        fade.gameObject.SetActive(false);

        zoomCamera.gameObject.SetActive(false);
    }

    //========================================
    #region ==== FADE ====

    /// <summary>
    ///  fadeIn/out - 
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeSequene()
    {
        FadeIn();   //페이드 인 하고, 

        yield return new WaitUntil( ()=>  isCompleted_fade );      //  씬넘어갈 때까지 ㄱㄷ.

        FadeOut();  
    }

    public IEnumerator FadeSequene(System.Func<bool> condition)
    {
        FadeIn();   //페이드 인 하고, 

        yield return new WaitUntil( condition );      //  씬넘어갈 때까지 ㄱㄷ.

        FadeOut();  
    }

    //

    /// <summary>
    ///  페이드인 : 화면 까매짐
    /// </summary>
    public void FadeIn()
    {
        isCompleted_fade =false;
        
        fade.gameObject.SetActive(true);

        DOTween.Sequence()
        .OnComplete( ()=>{isCompleted_fade= true;})
        .Append(fade.DOFade(1,1f))
        .SetUpdate(true)
        .Play();
    }

    /// <summary>s
    /// 페이드 아웃 : 화면 밝아짐 - 씬 전환되고 
    /// </summary>
    public void FadeOut()
    {
        DOTween.Sequence()
        .OnComplete( ()=>{fade.gameObject.SetActive(false);})
        .Append(fade.DOFade(0,1f))
        .SetUpdate(true)
        .Play();
    }
    #endregion

    #region ===== Zoom =====
    public void ZoomIn(Transform target)
    {
        zoomCamera.Follow = target;
        zoomCamera.gameObject.SetActive(true);
    }
    public void ZoomOut()
    {
        if (isZoomIn)
        {
            zoomCamera.gameObject.SetActive(false);
        }
        
    }

    #endregion


    #region ====== Volume =================

    public void SetVignette(float targetIntensity,float duration = 0)
    {             
        if (volume.profile.TryGet(out vignette))
        {
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetIntensity, duration).SetUpdate(true).Play();
        }
    }

    public void InitVignette(float duration)
    {     
        SetVignette(intensity_default,duration);
    }
    
    #endregion
}
