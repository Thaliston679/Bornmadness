using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboIncrement : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Increment()
    {
        PlayerMove pm = animator.gameObject.GetComponentInParent<PlayerMove>();
        pm.comboCount++;
        pm.attacking = false;
        animator.SetInteger("Atk", animator.GetInteger("Atk") + 1);
        animator.SetBool("DoAtk", false);

        if(pm.comboCount > 2)
        {
            pm.comboCount = 1;
            animator.SetInteger("Atk", 1);
            animator.SetBool("DoAtk", false);
        }
    }
}
