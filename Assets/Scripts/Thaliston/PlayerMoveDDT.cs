using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveDDT : MonoBehaviour
{
    private Rigidbody rb;

    public float speedX;
    public float speedZ;
    public float speed = 1;
    public float jumpForce;
    public bool isGrounded = false;

    //Cam
    public float speedH;
    private float pH;

    public GameObject areaAtk;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Run();
        Jump();
        CheckY();
        TurnSides();

        if (Input.GetMouseButtonDown(0))
        {
            areaAtk.SetActive(true);
            Invoke(nameof(DesactiveAtk), 0.1f);
        }
    }

    void DesactiveAtk()
    {
        areaAtk.SetActive(false);
    }

    void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * speedX * speed;
        float moveZ = Input.GetAxisRaw("Vertical") * speedZ * speed;


        Vector3 posCorrect = (transform.right * moveX) + (transform.forward * moveZ);

        rb.velocity = new Vector3(posCorrect.x, rb.velocity.y, posCorrect.z);
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            speed = 3f;
        }
        else
        {
            speed = 1;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(rb.velocity.x, jumpForce, rb.velocity.z, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void CheckY()
    {
        if (transform.position.y <= -30)
        {
            transform.position = new(0, -0.6f, -2.5f);
        }
    }

    void TurnSides()
    {
        pH += speedH * Input.GetAxis("Mouse X");
        transform.eulerAngles = new Vector3(0, pH, 0);
    }
}
