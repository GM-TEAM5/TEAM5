using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyStateUI : MonoBehaviour
{
    [SerializeField] Canvas enemyCanvas;
    
    [SerializeField] Slider hpBar;

    public void Init(Enemy enemy)
    {
        enemyCanvas.gameObject.SetActive( enemy.data.rank == EnemyRank.Elite );
        
        UpdateMaxHp(enemy.data.maxHp);
        UpdateCurrHp(enemy.hp);
    }

    public void OnDie()
    {
        if (enemyCanvas.gameObject.activeSelf)
        {
            enemyCanvas.gameObject.SetActive( false );
        }
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
}
