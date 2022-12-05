using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class TargetLock : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [Space]
    [Header("UI")]
    [SerializeField] private Image aimIcon;
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string enemyTag;
    [SerializeField] private KeyCode _Input;
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    public bool isTargeting;

    private float maxAngle;
    private Transform currentTarget;
    private float mouseX;
    private float mouseY;

    void Start()
    {
        maxAngle = 90f;
        cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
    }

    void Update()
    {
        if (!isTargeting)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            NewInputTarget(currentTarget);
        }

        if (aimIcon)
            aimIcon.gameObject.SetActive(isTargeting);

        cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;

        if (Input.GetKeyDown(_Input))
        {
            AssignTarget();
        }

        CheckTargetDeath();
    }

    void CheckTargetDeath()
    {
        if (!currentTarget) return;

        if (currentTarget.gameObject.GetComponent<EnemyAI>().enabled) return;

        AssignTarget();
        Invoke(nameof(AssignTarget), 0.01f);
    }

    public void ActDesLockTrgt()//ActiveDesactiveLockTarget
    {
        AssignTarget();
    }

    private void AssignTarget()
    {
        if (isTargeting)
        {
            isTargeting = false;
            currentTarget = null;
            return;

        }

        if (ClosestTarget())
        {
            currentTarget = ClosestTarget().transform;
            isTargeting = true;
            cinemachineFreeLook.gameObject.GetComponent<CinemachineInputProvider>().enabled = false;
        }
    }

    private void NewInputTarget(Transform target)
    {
        if (!currentTarget) return;

        Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);

        if (aimIcon)
            aimIcon.transform.position = mainCamera.WorldToScreenPoint(target.position);

        if ((target.position - transform.position).magnitude < minDistance) return;
        mouseX = (viewPos.x - 0.5f + targetLockOffset.x) * 3f;
        mouseY = (viewPos.y - 0.5f + targetLockOffset.y) * 3f;
    }


    private GameObject ClosestTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        return closest;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}