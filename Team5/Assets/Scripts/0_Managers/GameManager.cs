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
        GameEventManager.Instance.onStageLoad.AddListener( SetStage );


        InitGame();
    }

    //===================================================================================

    public void InitGame()
    {
        ResourceManager.Instance.Init();

    }



    /// <summary>
    /// 
    /// </summary>
    public void SetStage()
    {
        // 스테이지에 사용되는 리소스 확인하고,
        // 불러와놓고, (poolManager 세팅)
        // 스테이지 초기화.
        StageManager.Instance.Init(TestManager.Instance.testStageData);
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
}
