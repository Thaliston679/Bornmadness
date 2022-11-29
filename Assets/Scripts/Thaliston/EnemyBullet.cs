using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject,0.02f);
        }
    }
}
