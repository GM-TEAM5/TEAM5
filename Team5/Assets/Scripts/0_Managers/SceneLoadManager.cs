using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Loading;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    
    public static readonly string lobbySceneName = "1_Lobby";
    public static readonly string cutSceneName = "2_CutScene";
    public static readonly string mainSceneName = "3_Main";
    public static readonly string UnderWorldSceneName = "4_UnderWorld";



    bool isLoading;

    [SerializeField] bool isCompleted_sceneLoaded;

    bool canSwtichScene => DirectingManager.Instance.isCompleted_fade && isCompleted_sceneLoaded;     // 


    //====================
    // 비동기 씬 호출 : sceneName에 해당하는 씬을 비동기적으로 로드한다. 
    //===================
    public void LoadScene(string sceneName)
    {
        if( isLoading )
        {
            return;
        }
        isLoading = true;


        StartCoroutine( DirectingManager.Instance.FadeSequene(()=>canSwtichScene) );              // 페이드 인/아웃 진행
        StartCoroutine(LoadScene_async(sceneName)); // 씬 전환 작업
    }

    IEnumerator LoadScene_async(string sceneName)
    {       
        // 비동기 씬호출
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        isCompleted_sceneLoaded = false;

        // 씬 로드 시 작업 
        while (asyncLoad.allowSceneActivation == false )
        {
            //
            yield return new WaitUntil(()=>asyncLoad.progress >= 0.9f) ;      // 메모리에 해당 씬 리소스 모두 올릴 때까지 대기  

            isCompleted_sceneLoaded = true;

            //
            yield return new WaitUntil(()=>canSwtichScene) ; // 페이드 인 될 떄까지 기다림.

            asyncLoad.allowSceneActivation = true;      // 이거 하면 씬 넘어감
        }

        isLoading = false;
        /* 정보 : 
        asyncLoad.isDone 이라는 프로퍼티가 있는데,
        이건 asyncLoad.progress 가 1이 되어야 true가 된다. 
        그런데 asyncLoad.allowSceneActivation가 false 이면, 
        true가 될 때까지 asyncLoad.progress 가0.9로 고정되어있어서 isDone 이 true가 될 수 없다. 
        그래서 위처럼  씬 로드가 완료되었다는 플래그를 asyncLoad.progress >= 0.9f 로 사용한다. 
        */

        OnSceneChanged();
    }

    void OnSceneChanged()
    {

    }

    //================================
    public void Load_UnderWorld()
    {
        LoadScene(UnderWorldSceneName);
    }

    public void Load_MainScene()
    {
        LoadScene(mainSceneName);
    }



}
