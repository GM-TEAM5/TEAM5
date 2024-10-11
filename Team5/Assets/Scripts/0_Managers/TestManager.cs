using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager :  Singleton<TestManager>
{
    public List<PlayerSkillSO> initSkillData;
    public Sprite initProjSprite;

    public DamageText damageText;
}
