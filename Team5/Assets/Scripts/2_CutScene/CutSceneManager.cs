using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using DG.Tweening;
using TMPro;

public class CutSceneManager : MonoBehaviour 
{
    [SerializeField] bool initialized;
    [SerializeField] bool isCutSceneFinished;
    bool isSceneLoading;
    Sequence cutSceneSeq;


    [SerializeField] TextMeshProUGUI skipText;

    [SerializeField] TextMeshProUGUI testText;

    void Start()
    {
        StartCutScene();        //
    }


    void Update()
    {
        if(initialized ==false)
        {
            return;
        }
        
        // 스페이스 누르면 컷씬 바로 끝내기. 끝난상태에서 한번 더 스페이스 누르면 게임 시작
        if (Input.GetKeyDown(KeyCode.Space) )
        { 
            if (isCutSceneFinished ==false && cutSceneSeq.IsActive())
            {
                
                cutSceneSeq.Kill();
            }
            else 
            {
                
                StartGame();
            }
        }

    }

    //=================================================

    void StartCutScene()
    {
        initialized = true;
        isCutSceneFinished = false;


        skipText.gameObject.SetActive(false);
        PlaySequence();
    }

    void FinishCutScene()
    {
        skipText.gameObject.SetActive(true);
        
        isCutSceneFinished = true;
    }

    void StartGame()
    {
        SceneLoadManager.Instance.Load_UnderWorld();
    }

    //==========================================
    void PlaySequence()
    {
        cutSceneSeq = DOTween.Sequence()
        .OnKill( ()=>{
            FinishCutScene();
            testText.text = 10.ToString("0.0");
        })
        .Append( DOTween.To(() => 0, x => testText.text = x.ToString("0.0"), 10, 10f ))
        .Play();
    }



}
