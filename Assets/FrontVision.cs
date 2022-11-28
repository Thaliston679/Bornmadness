using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontVision : MonoBehaviour
{
    [SerializeField] Transform player; 
    void Update()
    {
        transform.LookAt(player);
    }
}
