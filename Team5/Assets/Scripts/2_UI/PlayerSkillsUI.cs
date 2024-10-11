using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsUI : MonoBehaviour
{
    [SerializeField] List<PlayerSkillUI> skillUIs = new();


    public void Init(List<PlayerSkill> skills)
    {
        skillUIs = new( GetComponentsInChildren<PlayerSkillUI>() );
        
        for(int i=0;i< skillUIs.Count;i++)
        {
            if (i< skills.Count)
            {
                skillUIs[i].Init(skills[i].skillData);
            }
            else
            {
                skillUIs[i].Deactivate();
            }
        }
    }
}
