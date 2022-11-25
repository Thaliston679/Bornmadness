using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighRayCast : MonoBehaviour
{
    [SerializeField] private bool playerInVisionRay;

    public Transform pointVision;
    public LayerMask layers;

    public GameObject lights;

    public Animator lighAnim;

    [SerializeField] private bool lighActived = false;
    [SerializeField] private float TimeToDesactive = 0;
    [SerializeField] private bool counter = false;


    private void Update()
    {
        Raycaster();

        //
        if (playerInVisionRay)
        {
            counter = false;
            TimeToDesactive = 0;
            if (!lights.activeSelf) lights.SetActive(true);
            if (!lighActived)
            {
                lighActived = true;
            }
            lighAnim.SetBool("Active", true);
        }
        else
        {
            if (!counter && lighActived) counter = true;
            lighAnim.SetBool("Active", false);
        }
        //


        //
        if (counter) TimeToDesactive += Time.deltaTime;
        if (TimeToDesactive > 6)
        {
            counter = false;
            TimeToDesactive = 0;
            lighActived = false;
            if (lights.activeSelf) lights.SetActive(false);
        }
        //
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
