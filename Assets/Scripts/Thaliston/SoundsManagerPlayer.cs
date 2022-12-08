using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManagerPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource efeitosSonoros;
    [SerializeField] private AudioSource trilhaSonora;
    public AudioClip soundTrackGame;
    public AudioClip[] playerWalk;
    public AudioClip[] playerAtk;
    public AudioClip playerDeath;
    public AudioClip playerHealing;
    public AudioClip playerMine;

    void Start()
    {
        efeitosSonoros.ignoreListenerPause = true;
        //trilhaSonora.ignoreListenerPause = true;
    }

    //Para chamar os áudios
    //GameObject.FindGameObjectWithTag("Player").GetComponent<SoundsManagerPlayer>().MetodoSom();

    public void PlayerWalk()
    {
        efeitosSonoros.PlayOneShot(playerWalk[Random.Range(0, playerWalk.Length)]);
    }

    public void PlayerAtk()
    {
        efeitosSonoros.PlayOneShot(playerAtk[Random.Range(0, playerAtk.Length)]);
    }

    public void TrilhaSonoraGame()
    {
        trilhaSonora.loop = true;
        trilhaSonora.clip = soundTrackGame;
        trilhaSonora.Play();
    }

    public void PlayerDeath()
    {
        efeitosSonoros.PlayOneShot(playerDeath);
    }

    public void PlayerHealing()
    {
        efeitosSonoros.PlayOneShot(playerHealing);
    }

    public void PlayerMine()
    {
        efeitosSonoros.PlayOneShot(playerMine);
    }
}
