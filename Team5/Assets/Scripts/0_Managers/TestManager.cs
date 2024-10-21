using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager :  Singleton<TestManager>
{
    public List<PlayerSkillSO> initSkillData;
    public Sprite initProjSprite;

    public DamageText damageText;

    public EnemySO enemyData;
    public StageDataSO testStageData;

    public AudioClip testEnemyDeathSFX;
    public SimpleSFX simpleSFX;

    public void TestSFX_enemyDeath()
    {
        Instantiate(simpleSFX).PlaySFX(testEnemyDeathSFX);
    }
}
