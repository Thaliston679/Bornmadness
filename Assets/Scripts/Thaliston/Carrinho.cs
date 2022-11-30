using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrinho : MonoBehaviour
{
    public GameObject car;
    public Transform a;
    public Transform b;
    public float speed;
    public Vector3 nextPos;
    [SerializeField]bool move;
    [SerializeField] bool playerIn;

    private void Start()
    {
        nextPos = a.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetCarPos();
            move = true;
        }
    }

    private void Update()
    {
        if (car.transform.localPosition.z >= b.localPosition.z && nextPos == b.localPosition)
        {
            move = false;
            speed *= -1;
        }
        if (car.transform.localPosition.z <= a.localPosition.z && nextPos == a.localPosition)
        {
            move = false;
            speed *= -1;
        }
    }
    private void FixedUpdate()
    {
        MoveCar();
    }

    void MoveCar()
    {
        if (move)
        {
            //GetComponent<Rigidbody>().velocity = Vector3.forward * speed * Time.deltaTime;
            car.transform.position = Vector3.MoveTowards(car.transform.position, nextPos, speed * Time.deltaTime);
            //transform.Translate(transform.forward*speed*Time.deltaTime, Space.World);
        }
    }

    void GetCarPos()
    {
        if (car.transform.localPosition.z > 0 && nextPos != a.localPosition)
        {
            nextPos = a.position;
        }
        if (car.transform.localPosition.z < 0 && nextPos != b.localPosition)
        {
            nextPos = b.position;
        }
    }
}
