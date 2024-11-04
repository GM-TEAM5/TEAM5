using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class StagePortal : InteractableObject
{
    [SerializeField] TextMeshPro text;
    
    public void Init()
    {
        gameObject.SetActive(false);

    }


    protected override void OnEnter(bool isOn)
    {
        text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract()
    {
        Debug.Log("포탈 진입");

        //
        GoToNextStage();
    }

    public void OnStageClear()
    {
        gameObject.SetActive(true);
        Activate();
    }

    
    void GoToNextStage()
    {
        SceneLoadManager.Instance.Load_MainScene();     
    }



    // public Sequence GetSeq_GeneratePortal(float playTime)
    // {
    //     var sr = GetComponent<SpriteRenderer>();
    //     sr.color = new Color(1,1,1,0);

    //     return DOTween.Sequence()
    //     .Append( sr.DOFade(1f, playTime));
    // }

    // public Sequence PlaySeq_DestroyPortal(float playTime)
    // {
    //     var sr = GetComponent<SpriteRenderer>();
    //     sr.color = Color.white;

    //     return DOTween.Sequence()
    //     .OnComplete( ()=>{Destroy(gameObject);})
    //     .Append( sr.DOFade(0f, playTime))
    //     .Play();
    // }
}
