using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private bool playerInVisionRange;
    [SerializeField] private bool playerInVisionRay;
    [SerializeField] private bool callEnemy;
    [SerializeField] private float persistenseTime;
    [SerializeField] private float persistenseTimer;

    public Transform pointVision;
    public LayerMask layers;

    [SerializeField]EnemyAI ai;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            persistenseTimer = 0;
            playerInVisionRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            persistenseTimer = persistenseTime;
            playerInVisionRange = false;
        }
    }

    private void Update()
    {
        if(playerInVisionRange) Raycaster();

        if(playerInVisionRange && playerInVisionRay && !callEnemy)
        {
            callEnemy = true;
            ai.SetPlayerInVisionRange(true);
        }

        if (persistenseTimer > 0)
        {
            persistenseTimer -= Time.deltaTime;
        }

        if(persistenseTimer <= 0 && !playerInVisionRange)
        {
            playerInVisionRay = false;
            ai.SetPlayerInVisionRange(false);
            callEnemy = false;
        }
    }

    public void SetPersistense(float persist)
    {
        persistenseTime = persist;
    }

    public void Raycaster()
    {
        pointVision.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        if(Physics.Raycast(pointVision.position, pointVision.forward, out RaycastHit hit, 100, layers, QueryTriggerInteraction.Ignore))
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
