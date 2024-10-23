using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public PlayerDataSO playerData;       // 얘는 결국 별도의 로딩이 필요없음.

    public static bool isPaused;
    
    //===================================================================================

    //
    void Start()
    {
        InitGame();
    }

    //===================================================================================

    public void InitGame()
    {
        GameEventManager.Instance.onGameOver.AddListener(onGameOver);
        
        
        ResourceManager.Instance.Init();
    }
    
    void onGameOver()
    {
        playerData.deathCount ++;
    }



    /// <summary>
    /// 게임 종료 - 현재 01_Lobby 의 Quit 버튼에서 호출됨.
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    public void PauseGamePlay()
    {
        
    }

    public void PauseGamePlay(bool pause, float duration = 0f)
    {
        float targetTimeScale= pause? 0: 1f;
        isPaused = pause;
        if (duration == 0f)
        {
            Time.timeScale = targetTimeScale;
        }
        else
        {
            DOTween.To( ()=> Time.timeScale, x=> Time.timeScale = x ,targetTimeScale, duration ).SetUpdate(true).Play();
        }
       
        
    }
}
