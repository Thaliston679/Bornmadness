using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallAtk : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPoint;
    public GameObject atkBox;
    public void MeleeAtkActive()
    {
        atkBox.SetActive(true);
    }

    public void MeleeAtkDesactive()
    {
        atkBox.SetActive(false);
    }

    public void RangeAtk()
    {
        Rigidbody rb = Instantiate(bullet, bulletPoint.position, bulletPoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 48, ForceMode.Impulse);
        rb.AddForce(transform.up * 2, ForceMode.Impulse);
    }
}
