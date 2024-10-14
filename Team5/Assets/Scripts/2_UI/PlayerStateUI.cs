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
    public void Init(Player player)
    {
        UpdateMaxHp(player.status.maxHp);
        UpdateCurrHp(player.status.hp);

        UpdateMaxExp(player.status.maxExp);
        UpdateCurrExp(player.status.currExp);
        UpdateLevelText(player.status.level);

        UpdateMaxInk(player.status.maxInk);
        UpdateCurrInk(player.status.currInk);
    }



    #region ====== HP =======

    /// <summary>
    /// hp bar 의 최댓값을 플레이어 능력치 값에 맞춘다. (주로 체력 증가 이벤트 발생시 호출됨 )
    /// </summary>
    public void UpdateMaxHp(float maxHp)
    {
        hpBar.maxValue = maxHp;
    }

    /// <summary>
    /// hp bar의 현재 값을 플레이어 능력치 값에 맞춘다. (주로 회복, 피해 발생시 호출됨)
    /// </summary>
    public void UpdateCurrHp(float hp)
    {
        hpBar.value = hp;
    }

    #endregion
    #region ====== Level =======

    /// <summary>
    /// hp bar 의 최댓값을 플레이어 능력치 값에 맞춘다. (주로 체력 증가 이벤트 발생시 호출됨 )
    /// </summary>
    public void UpdateMaxExp(float maxExp)
    {
        expBar.maxValue = maxExp;
    }

    /// <summary>
    /// hp bar의 현재 값을 플레이어 능력치 값에 맞춘다. (주로 회복, 피해 발생시 호출됨)
    /// </summary>
    public void UpdateCurrExp(float currExp)
    {
        expBar.value = currExp;
    }

    /// <summary>
    ///  레벨 텍스트 업데이트
    /// </summary>
    public void UpdateLevelText(int level)
    {
        levelText.SetText( level.ToString());
    }
    #endregion

    #region ====== Ink =======

    /// <summary>
    /// ink bar 의 최댓값을 플레이어 능력치 값에 맞춘다.
    /// </summary>
    public void UpdateMaxInk(float maxInk)
    {
        inkBar.maxValue = maxInk;
    }

    /// <summary>
    /// ink bar의 현재 값을 맞춘다. (붓칠 시 호출됨)
    /// </summary>
    public void UpdateCurrInk(float currInk)
    {
        inkBar.value = currInk;
    }
    #endregion
}
