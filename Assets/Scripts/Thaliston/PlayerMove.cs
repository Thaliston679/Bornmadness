using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]Animator anim;
    //[SerializeField] Rigidbody rb;
    [SerializeField] CharacterController cc;
    [SerializeField] Transform cam;

    //Input Receive Values
    Vector2 movement;

    float speed = 5;
    float turnSpeed = 0.1f;
    float turnVelocity;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        /*My Code
        Vector3 newDirection = new Vector3(movement.x, 0, movement.y);
        rb.velocity = new(movement.x * speed, rb.velocity.y, movement.y * speed);

        Vector3 newRotate = Vector3.RotateTowards(transform.forward, newDirection, turnSpeed * Time.deltaTime, 0);
        rb.MoveRotation(Quaternion.LookRotation(newRotate));
        */

        /*Tutorial
        Vector3 direction = new Vector3(movement.x, 0, movement.y);
        rb.MovePosition(rb.position + direction * Time.deltaTime * speed);

        Vector3 newRotation = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.deltaTime, 0);
        rb.MoveRotation(Quaternion.LookRotation(newRotation));
        */
    }

    private void Update()
    {
        Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized; //.normalize serve pra evitar erro de aumentar velocidade ao ir pra frente e pra tras ao mesmo tempo

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    public void PlayerOnMove(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void PlayerOnJump(InputAction.CallbackContext value)
    {

    }

    public void PlayerOnHeal(InputAction.CallbackContext value)
    {

    }

    public void PlayerOnTNT(InputAction.CallbackContext value)
    {

    }

    public void PlayerOnAtk(InputAction.CallbackContext value)
    {

    }
}
