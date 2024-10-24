using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager :  Singleton<TestManager>
{
    public List<PlayerSkillSO> initSkillData;
    public Sprite initProjSprite;

    public DamageText damageText;

    public EnemyDataSO enemyData;
    public StageDataSO testStageData;

    public AudioClip testEnemyDeathSFX_ghost;
    public AudioClip testEnemyDeathSFX_beast;
    public AudioClip testNormalAttackSFX;
    public AudioClip testEnhancedAttackSFX;

    public SimpleSFX simpleSFX;


    //
    public void TestSFX_enemyDeath(EnemyType enemyType)
    {
        if( enemyType == EnemyType.Beast)
        {
            Instantiate(simpleSFX).PlaySFX(testEnemyDeathSFX_beast);
        }
        else if (enemyType == EnemyType.Ghost)
        {
            Instantiate(simpleSFX).PlaySFX(testEnemyDeathSFX_ghost);
        }
    }

    //
    public void TestSFX_NormalAttack()
    {
        Instantiate(simpleSFX).PlaySFX(testNormalAttackSFX);
    }
    public void TestSFX_EnhancedAttack()
    {
        Instantiate(simpleSFX).PlaySFX(testEnhancedAttackSFX);
    }
}
