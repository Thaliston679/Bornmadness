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
        Vector3 playerRef = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 playerCenter = new(playerRef.x, playerRef.y+0.5f, playerRef.z);
        Rigidbody rb = Instantiate(bullet, bulletPoint.position, bulletPoint.rotation).GetComponent<Rigidbody>();
        rb.gameObject.transform.LookAt(playerCenter);
        rb.AddForce(rb.gameObject.transform.forward * 32, ForceMode.Impulse);
        //rb.AddForce(transform.up * 2, ForceMode.Impulse);
        Destroy(rb.gameObject, 3f);
    }
}
