using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalActivation : MonoBehaviour
{
    public GameObject cristal;
    public bool cristalVisivel;

    float timer;

    private void Start()
    {
        Invoke(nameof(DesactiveCristals),0.1f);
    }

    private void Update()
    {
        if (cristalVisivel) timer += Time.deltaTime;
        if(timer > 1)
        {
            timer = 0;
            if(cristal != null) GetComponent<BoxCollider>().center = cristal.transform.localPosition;
        }
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
            if (cristal != null) GetComponent<BoxCollider>().center = cristal.transform.localPosition;
            cristalVisivel = false;
            if (cristal != null) cristal.SetActive(false);
        }
    }
}
