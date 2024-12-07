using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public PlayerDataSO playerData;       // 얘는 결국 별도의 로딩이 필요없음.

    private List<ITimeScaleable> timeScaleables = new List<ITimeScaleable>();
    public static bool isPaused;

    //===================================================================================

    void Awake()
    {
        DOTween.SetTweensCapacity(1000, 50);  // tweens: 1000, sequences: 50
    }

    //
    void Start()
    {
        InitGame();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            playerData.InitPlayerData();
            SceneLoadManager.Instance.Load_Lobby();
        }


    }

    //===================================================================================

    public void InitGame()
    {
        GameEventManager.Instance.onGameOver.AddListener(onGameOver);


        ResourceManager.Instance.Init();
        PlayerInputManager.Instance.Init();
        SoundManager.Instance.Init();
    }

    void onGameOver()
    {
        playerData.deathCount++;
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


    public void RegisterTimeScaleable(ITimeScaleable scaleable)
    {
        if (!timeScaleables.Contains(scaleable))
        {
            timeScaleables.Add(scaleable);
        }
    }

    public void UnregisterTimeScaleable(ITimeScaleable scaleable)
    {
        timeScaleables.Remove(scaleable);
    }

    public void PauseGamePlay(bool pause, float duration = 0f)
    {
        float targetTimeScale = pause ? 0 : 1f;
        isPaused = pause;

        // 모든 ITimeScaleable 객체의 타임스케일 설정
        foreach (var scaleable in timeScaleables)
        {
            scaleable.SetTimeScale(targetTimeScale);
        }

        if (duration == 0f)
        {
            Time.timeScale = targetTimeScale;
        }
        else
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, targetTimeScale, duration)
                   .SetUpdate(true)
                   .Play();
        }
    }
}
