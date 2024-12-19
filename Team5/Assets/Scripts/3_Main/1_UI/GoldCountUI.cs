using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GoldCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_goldCount;

    //===================================================================

    void Start()
    {
        Init();
    }

    void OnDestroy()
    {
        GameEventManager.Instance.onChangePlayerGold.RemoveListener(OnChangePlayerGold);
    }

    //================================================================

    void Init()
    {
        GameEventManager.Instance.onChangePlayerGold.AddListener(OnChangePlayerGold);
        
        
        text_goldCount.SetText($"{Player.Instance.status.gold}");
    }

    void OnChangePlayerGold(int amount, int gold)
    {
        text_goldCount.SetText($"{gold}");
    } 
}
