using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManagerEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource efeitosSonoros;
    public AudioClip[] enemyWalk;
    public AudioClip[] enemyOgroWalk;
    public AudioClip clava;
    public AudioClip faca;
    public AudioClip goblinDamage;
    public AudioClip ogroDamage;
    public AudioClip ogroDeath;
    public AudioClip goblinDeath;
    public AudioClip zarabatana;

    void Start()
    {
        efeitosSonoros.ignoreListenerPause = true;
    }

    //Para chamar os áudios
    //GameObject.FindGameObjectWithTag("Enemy").GetComponent<SoundsManagerEnemy>().MetodoSom();

    public void OgroWalk()
    {
        efeitosSonoros.PlayOneShot(enemyOgroWalk[Random.Range(0, enemyOgroWalk.Length)]);
    }

    public void GoblinWalk()
    {
        efeitosSonoros.PlayOneShot(enemyWalk[Random.Range(0, enemyWalk.Length)]);
    }

    public void Clava()
    {
        efeitosSonoros.PlayOneShot(clava);
    }

    public void Faca()
    {
        efeitosSonoros.PlayOneShot(faca);
    }

    public void GoblindDamage()
    {
        efeitosSonoros.PlayOneShot(goblinDamage);
    }

    public void OgroDamage()
    {
        efeitosSonoros.PlayOneShot(ogroDamage);
    }

    public void GoblinDeath()
    {
        efeitosSonoros.PlayOneShot(goblinDeath);
    }

    public void OgroDeath()
    {
        efeitosSonoros.PlayOneShot(ogroDeath);
    }

    public void Zarabatana()
    {
        efeitosSonoros.PlayOneShot(zarabatana);
    }
}
