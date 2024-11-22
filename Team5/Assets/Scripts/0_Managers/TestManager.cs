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


    public List<Sprite> boundImages=new();

    public GameObject InstantDeath;

    // void Start()
    // {
    //     SetBoundImage();
    // }

    [Header("Foot Step Sound")]
    
    public List<AudioClip> fs_clothes= new();
    public List<AudioClip> fs_foot= new();
    public List<AudioClip> fs_grass= new();


    [Header("EnemyHit")]

    public AudioClip ac_enemyHit;


    public void TestSFX_enemyHit()
    {
        Instantiate(simpleSFX).PlaySFX(ac_enemyHit);
    }



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


    public AudioClip sfx_ryoikitenkaiOn;
    public AudioClip sfx_ryoikitenkaiOff;
    public void TestSFX_RyoikiTenkai(bool isOn)
    {
        if (isOn)
        {
            Instantiate(simpleSFX).PlaySFX(sfx_ryoikitenkaiOn);
        }
        else
        {
            Instantiate(simpleSFX).PlaySFX(sfx_ryoikitenkaiOff);
        }
        
        
    }


    public void TestSFX_FootStep()
    {
        int rand = Random.Range(0,4);
        // Debug.Log(rand);
         Instantiate(simpleSFX).PlaySFX(fs_clothes[rand]);
         Instantiate(simpleSFX).PlaySFX(fs_foot[rand]);
         Instantiate(simpleSFX).PlaySFX(fs_grass[rand]);
    }




    // public void SetBoundImage()
    // {
    //     var imgs = FindObjectsOfType<TestBillboard>();

    //     foreach(var img in imgs)
    //     {
    //         int randIdx = Random.Range(0, boundImages.Count);
            
    //         img.GetComponent<SpriteRenderer>().sprite = boundImages[randIdx];

    //     }


    // }


    public void KillPlayer()
    {
        Instantiate(InstantDeath, new Vector3(0,30,0), Quaternion.identity );
    }
}
