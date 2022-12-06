using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalActivation : MonoBehaviour
{
    public GameObject cristal;
    public bool cristalVisivel;

    private void Start()
    {
        Invoke(nameof(DesactiveCristals),0.1f);
    }

    void DesactiveCristals()
    {
        if(!cristalVisivel) cristal.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cristalVisivel = true;
            if (cristal != null) cristal.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cristalVisivel = false;
            if (cristal != null) cristal.SetActive(false);
        }
    }
}
