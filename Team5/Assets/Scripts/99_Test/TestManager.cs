using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager :  Singleton<TestManager>
{

    public SimpleSFX simpleSFX;


    // public List<Sprite> boundImages=new();

    public GameObject InstantDeath;

    // void Start()
    // {
    //     SetBoundImage();
    // }

    // [Header("Foot Step Sound")]
    
    // public List<AudioClip> fs_clothes= new();
    // public List<AudioClip> fs_foot= new();
    // public List<AudioClip> fs_grass= new();





    [SerializeField] SerializableDictionary<string, AudioClip> testSFXs=new(); 
    public void TestSFX_BtnClick()
    {
        TestSFX("BtnClick");
    }
    
    public void TestSFX_GameStart()
    {
        TestSFX("GameStart");
    }

    void TestSFX(string key)
    {
        // Instantiate(simpleSFX).PlaySFX(testSFXs[key]);
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
