using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDDT : MonoBehaviour
{
    public Transform alvo;
    RaycastHit hit = new RaycastHit();
    public float distCam;
    public LayerMask whatIsGround;
    public float ajustCam;


    void Update()
    {
        transform.position = alvo.position - transform.forward * distCam;
        if(Physics.Linecast(alvo.position, transform.position, out hit, whatIsGround))
        {
            transform.position = hit.point + transform.forward * ajustCam;
        }
    }
}
