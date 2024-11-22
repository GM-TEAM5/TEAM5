using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;



public class EnemyHpSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    
    [SerializeField] TextMeshProUGUI text_percent;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    // public void Init(Enemy enemy)
    // {
    //     slider.maxValue = enemy.data.maxHp;
    //     slider.value= enemy.hp;

    //     UpdateHp(enemy);
    // }


    public void UpdateHp(Enemy enemy)
    {
        if(gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
        
        slider.maxValue = enemy.data.maxHp;
        slider.value= enemy.hp;

        float percent = slider.value/slider.maxValue *100f;
        text_percent.SetText($" { percent:.0}%");
    }
}
