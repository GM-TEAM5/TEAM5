using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    Image img_icon;
   
    public void Init(PlayerSkillSO skillData)
    {
        img_icon = GetComponent<Image>();
        img_icon.sprite = skillData.icon;

        Activate();
    }


    public void Activate()
    {
        gameObject.SetActive(true);    
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
