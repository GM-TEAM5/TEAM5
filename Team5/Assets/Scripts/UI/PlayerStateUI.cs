using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [SerializeField] Slider expBar;
    [SerializeField] Slider inkBar;

    [SerializeField] TextMeshProUGUI levelText;



    //==========================================
    public void Init()
    {
        UpdateMaxHp();
        UpdateCurrHp();

        UpdateMaxExp();
        UpdateCurrExp();
        UpdateLevelText();

    }



    #region ====== HP =======

    /// <summary>
    /// hp bar 의 최댓값을 플레이어 능력치 값에 맞춘다. (주로 체력 증가 이벤트 발생시 호출됨 )
    /// </summary>
    public void UpdateMaxHp()
    {
        hpBar.maxValue = Player.Instance.status.maxHp;
    }

    /// <summary>
    /// hp bar의 현재 값을 플레이어 능력치 값에 맞춘다. (주로 회복, 피해 발생시 호출됨)
    /// </summary>
    public void UpdateCurrHp()
    {
        hpBar.value = Player.Instance.status.hp;
    }

    #endregion
    #region ====== Level =======

    /// <summary>
    /// hp bar 의 최댓값을 플레이어 능력치 값에 맞춘다. (주로 체력 증가 이벤트 발생시 호출됨 )
    /// </summary>
    public void UpdateMaxExp()
    {
        expBar.maxValue = Player.Instance.status.maxExp;
    }

    /// <summary>
    /// hp bar의 현재 값을 플레이어 능력치 값에 맞춘다. (주로 회복, 피해 발생시 호출됨)
    /// </summary>
    public void UpdateCurrExp()
    {
        expBar.value = Player.Instance.status.currExp;
    }

    /// <summary>
    ///  레벨 텍스트 업데이트
    /// </summary>
    public void UpdateLevelText()
    {
        levelText.SetText( Player.Instance.status.level.ToString() );
    }
    #endregion
}
