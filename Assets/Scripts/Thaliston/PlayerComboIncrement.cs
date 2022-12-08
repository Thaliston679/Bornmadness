using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboIncrement : MonoBehaviour
{
    Animator animator;
    SoundsManagerPlayer playerSounds;

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerSounds = GameObject.FindGameObjectWithTag("Player").GetComponent<SoundsManagerPlayer>();
    }

    void Increment()
    {
        animator.GetComponentInParent<PlayerMove>().attacking = false;
    }

    void AddHP()
    {
        GetComponentInParent<PlayerMove>().AddHP();
    }

    void EndHeal()
    {
        GetComponentInParent<PlayerMove>().EndHeal();
    }

    //SoundsManager
    public void PlayerDeath()
    {
        playerSounds.PlayerDeath();
    }

    public void PlayerHealing()
    {
        playerSounds.PlayerHealing();
    }

    public void PlayerWalk()
    {
        playerSounds.PlayerWalk();
    }

    public void PlayerAtk()
    {
        playerSounds.PlayerAtk();
    }
}
