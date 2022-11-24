using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighRayCast : MonoBehaviour
{
    [SerializeField] private bool playerInVisionRay;

    public Transform pointVision;
    public LayerMask layers;

    public GameObject lights;


    private void Update()
    {
        Raycaster();

        if (playerInVisionRay)
        {
            if (!lights.activeSelf) lights.SetActive(true);
        }
        else
        {
            if (lights.activeSelf) lights.SetActive(false);
        }
    }

    public void Raycaster()
    {
        pointVision.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        if (Physics.Raycast(pointVision.position, pointVision.forward, out RaycastHit hit, 100, layers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(pointVision.position, hit.point, Color.red);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                playerInVisionRay = true;
            }
            else
            {
                playerInVisionRay = false;
            }
        }
        else
        {
            playerInVisionRay = false;
        }
    }
}
