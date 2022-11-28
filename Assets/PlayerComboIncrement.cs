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
        animator.GetComponentInParent<PlayerMove>().attacking = false;
    }
}
