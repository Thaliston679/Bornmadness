using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Active", true);
        }
    }
}
