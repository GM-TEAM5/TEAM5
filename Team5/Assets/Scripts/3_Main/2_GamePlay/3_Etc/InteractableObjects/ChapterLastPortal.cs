using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ChapterLastPortal :InteractiveObject
{
    [SerializeField] TextMeshPro text;

    // public override bool hasSecondaryInteraction => false;

    public int nextChapterNo;

    public void Init(int nextChapterNo)
    {
        this.nextChapterNo = nextChapterNo;
    }


    protected override void OnInspect_Custom(bool isOn)
    {
        text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract_Custom()
    {
        Debug.Log("챕터 마지막 포탈 진입");

        //
        GoToNextChapter();    
    }


    public void OnStageClear()
    {
        gameObject.SetActive(true);
        Activate();
    }

    
    void GoToNextChapter()
    {
        GameManager.Instance.userData.OnChapterClear(Player.Instance, nextChapterNo);  // 데이터 저장.
        
        if(GameManager.Instance.IsGameClear())
        {
  
            GameManager.Instance.userData.SetInitializationWaitingState();   // 데이터 초기화.
            SceneLoadManager.Instance.Load_Credit();     
        }
        else
        {
            Debug.Log("진짜 다음 챕터로~");
        }
    }

    // protected override void OnSecondaryInteract_Custom()
    // {
        
    // }
}
