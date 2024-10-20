using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerDataSO playerData;       // 얘는 결국 별도의 로딩이 필요없음.


    //===================================================================================

    //
    void Start()
    {
        InitGame();
    }

    //===================================================================================

    public void InitGame()
    {
        ResourceManager.Instance.Init();
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
}
