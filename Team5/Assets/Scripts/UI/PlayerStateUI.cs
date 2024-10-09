using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [SerializeField] Slider expBar;
    [SerializeField] Slider inkBar;


    //==========================================
    public void Init()
    {
        UpdateMaxHp();
        UpdateCurrHp();
    }


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



}
