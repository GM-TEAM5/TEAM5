using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSkillsUI : MonoBehaviour
{
    [SerializeField] GameObject prefab_skillUI;
    [SerializeField] List<PlayerSkillUI> skillUIs;

    void Awake()
    {
        GameEventManager.Instance.onInitPlayer.AddListener(() =>
        {
            var playerSkills = Player.Instance.skills;
            if (playerSkills.actives != null)
            {
                Init(playerSkills.actives);
            }
        });
    }

    public void Init(SerializableDictionary<SkillType, PlayerSkill> actives)
    {
        // 미리 만들어져 있던 거 파괴
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        skillUIs = new();

        // 생성 
        foreach (var kv in actives)
        {
            PlayerSkillUI skillUI = Instantiate(prefab_skillUI, transform).GetComponent<PlayerSkillUI>();
            skillUI.Init(kv.Key, kv.Value);
            skillUIs.Add(skillUI);
        }
    }
}
