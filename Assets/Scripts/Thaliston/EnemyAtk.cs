using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtk : MonoBehaviour
{
    public bool damageContinuos;
    public float delayDamage;
    private float timerDamage;

    private float damage;

    private void Update()
    {
        if(timerDamage > 0)
        {
            timerDamage -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !damageContinuos)
        {
            //Método para tirar HP do player
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && damageContinuos && timerDamage <= 0)
        {
            timerDamage = delayDamage;
            //Método para tirar HP do player
        }
    }

    public void SetDamage(float passDamage)
    {
        damage = passDamage;
    }
}
