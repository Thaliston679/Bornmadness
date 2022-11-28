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

    [SerializeField] float speed = 5;
    [SerializeField] float turnSpeed = 0.1f;
    float turnVelocity;

    //Pulo
    [SerializeField] float gravity = -20f;
    [SerializeField] float jumpSpeed = 15;
    bool doJump;
    float jumpTimer = 0;
    [SerializeField] Vector3 moveVelocity;
    bool canJump;
    float canJumpTime = 0.25f;

    //Ataque e Combos
    public int comboCount = 1;
    [SerializeField] float comboTimer = 0;
    public bool attacking = false;
    [SerializeField] bool doAtk = false;

    //Canvas
    //Dano
    [SerializeField] Animator borderDamage;

    //Vida
    [SerializeField] float hp;
    [SerializeField] float hpMax = 10;

    bool dodge = false;
    float dodgeTimer = 0f;
    [SerializeField] GameObject frontVision;

    void Start()
    {
        hp = hpMax;
        cc = GetComponent<CharacterController>();
    }


    private void Update()
    {
        //Movimentação
        MoveControl();

        //Pulo
        JumpControl();

        //Ataque
        AtkControl();

        //Gravidade
        moveVelocity.y += gravity * Time.deltaTime;
        cc.Move(moveVelocity * Time.deltaTime);

        //Controlador de váriaveis
        JumpTimer();

        //Controlador de animações
        AnimationsControl();

        if (Input.GetKeyDown(KeyCode.T))
        {
            dodge = true;
            dodgeTimer = 0.2f;
        }
        if (dodge)
        {
            if(dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
            Dodge();
        }
        if(dodgeTimer < 0)
        {
            dodge = false;
        }
    }

    void MoveControl()
    {
        //Movimentação
        Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized; //.normalize serve pra evitar erro de aumentar velocidade ao ir pra frente e pra tras ao mesmo tempo
        if (direction.magnitude >= 0.1f/* && !attacking*/)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    void Dodge()
    {
        //Dodge
        Vector3 dodgeDirection = new Vector3(0f, 0f, -3.5f);

        float targetAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        cc.Move(moveDir * (speed*3) * Time.deltaTime);

        //Rotacionar
        float targetBackAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg + frontVision.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetBackAngle, ref turnVelocity, 0.05f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    void DodgeTimer()
    {

    }

    void JumpControl()
    {
        //Verificadores de Pulo
        if (cc.isGrounded)
        {
            canJump = true;
            canJumpTime = 0.25f;

            if (moveVelocity.y <= 0.5f)
            {
                moveVelocity.y = 0;
            }
        }
        if (canJump && !cc.isGrounded)
        {
            canJumpTime -= Time.deltaTime;
        }
        if (canJumpTime <= 0 && !cc.isGrounded)
        {
            canJump = false;
        }
        //Ação de pular
        if (canJump && doJump && jumpTimer > 0f)
        {
            doJump = false;
            canJump = false;
            jumpTimer = 0f;

            moveVelocity.y = jumpSpeed;
        }
    }

    void AtkControl()
    {
        if (doAtk)
        {
            doAtk = false;
            if (!attacking)
            {
                attacking = true;
                comboCount++;

                if(comboCount > 3)
                {
                    comboCount = 1;
                }

                anim.SetInteger("Atk", comboCount);
                anim.SetBool("Attacking", true);
            }
        }

        if (!attacking)
        {
            anim.SetBool("Attacking", false);

            if(comboCount > 1)
            {
                if(comboTimer >= 0f)
                {
                    comboTimer -= Time.deltaTime;
                }
                if(comboTimer < 0)
                {
                    comboCount--;
                }
            }
        }
    }

    void EnemyCollision()
    {
        borderDamage.SetTrigger("Dano");
    }

    void AnimationsControl()
    {
        if (cc.isGrounded)
        {
            if(Mathf.Abs(movement.x) >= 0.5f || Mathf.Abs(movement.y) >= 0.5f)
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Idle", false);
            }
            else
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Idle", true);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", false);
        }

        if (cc.velocity.y <= -10f)
        {
            anim.SetBool("Fall", true);
        }
        else
        {
            anim.SetBool("Fall", false);
        }

        if (cc.velocity.y >= 1f)
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }
    }

    public void PlayerOnMove(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void PlayerOnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            doJump = true;
            jumpTimer = 0.2f;
        }
    }
    void JumpTimer()
    {
        if(doJump && jumpTimer >= 0f)
        {
            jumpTimer -= Time.deltaTime;
        }
        if(jumpTimer < 0)
        {
            doJump = false;
        }
    }

    public void PlayerOnHeal(InputAction.CallbackContext value)
    {

    }

    public void PlayerOnTNT(InputAction.CallbackContext value)
    {

    }

    public void PlayerOnAtk(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            doAtk = true;
            comboTimer = 0.25f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("atkEnemy_hit"))
        {
            EnemyCollision();
        }
    }
}
