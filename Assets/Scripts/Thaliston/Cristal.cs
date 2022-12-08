using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristal : MonoBehaviour
{
    public float xp;
    public GameObject cristalDrop;
    public Transform localDrop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("atkPlayer_hit"))
        {
            other.GetComponentInParent<PlayerMove>().GetNewXp(xp);
            GameObject drop = Instantiate(cristalDrop, localDrop.position, Quaternion.identity);
            drop.GetComponent<ParticleSystemRenderer>().material = GetComponentInChildren<MeshRenderer>().material;
            GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>().crystalsCurrent++;
            GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>().UpdateEnemCris();
            GameObject.FindGameObjectWithTag("Player").GetComponent<SoundsManagerPlayer>().PlayerMine();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
