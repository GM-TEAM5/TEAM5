using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slash
{
    public GameObject slashPrefab;
    public float delay;
}

public class PlayerBasicAttack : MonoBehaviour
{
    PlayerInputManager playerInput;

    public List<Slash> slashes;
    private bool attacking = false;

    void Start()
    {
        playerInput = PlayerInputManager.Instance;
        DisableSlashes();
    }

    void Update()
    {
        if (playerInput.isMouseLeftButtonOn && !attacking)
        {
            attacking = true;
            StartCoroutine(BasicAttack());
        }
    }

    IEnumerator BasicAttack()
    {
        for (int i = 0; i < slashes.Count; i++)
        {
            yield return new WaitForSeconds(slashes[i].delay);
            slashes[i].slashPrefab.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
        DisableSlashes();
        attacking = false;
    }

    void DisableSlashes()
    {
        for (int i = 0; i < slashes.Count; i++)
        {
            slashes[i].slashPrefab.SetActive(false);
        }
    }
}