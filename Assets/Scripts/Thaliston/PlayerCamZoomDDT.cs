using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamZoomDDT : MonoBehaviour
{
    public bool zoom;
    public Rigidbody rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zoom && transform.position.z > -1.5f)
        {
            rb.MovePosition(new(0,0,-1.5f));
        }

        if (!zoom && transform.position.z < 5)
        {
            rb.MovePosition(new(0, 0, 5));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            zoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            zoom = false;
        }
    }
}
