using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

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
    [SerializeField] float continuosAtkTimer = 0f;

    //Canvas
    //Dano
    [SerializeField] Animator borderDamage;
    [SerializeField] Animator borderToxic;
    public GameObject bloodParticle;
    [SerializeField] bool doDamage = false;
    public GameObject gameOverPanel;
    [SerializeField] bool toxic = false;
    [SerializeField] bool spike = false;

    //Vida
    [SerializeField] float hp;
    [SerializeField] float hpMax = 10;
    public Slider hpBarSlider;

    [SerializeField] Image hpFill;

    bool dodge = false;
    float dodgeTimer = 0f;
    [SerializeField] GameObject frontVision;
    bool canDodge;
    float canDodgeTime;
    bool dodgeSide;//true = front; false = back;
    [SerializeField] float dodgeSpeed;

    //FPS
    [SerializeField]TextMeshProUGUI fpsCont;

    //Descanso
    public GameObject descansoPanel;
    [SerializeField] bool inDescanso;
    [SerializeField] bool inDescansoArea;
    GameObject descansoObj;

    //Recuperar HP
    [SerializeField] bool healing;
    [SerializeField]float healCount = 5;
    [SerializeField]Image healImg;

    //Next Level
    public GameObject nextLevelPanel;
    [SerializeField] bool inNextLevelArea;
    GameObject nextLevelObj;
    public GameObject nextLevelPanelDelete;

    //Back Level
    public GameObject backLevelPanel;
    [SerializeField] bool inBackLevelArea;
    GameObject backLevelObj;
    public GameObject backLevelPanelDelete;

    //Cameras Switch
    public GameObject[] cams;

    //Cena atual
    [SerializeField]int sceneA;

    void Start()
    {
        hp = hpMax;
        cc = GetComponent<CharacterController>();
        hpBarSlider.maxValue = hpMax;
        InvokeRepeating(nameof(ContadorFPS), 0, .5f);
        sceneA = SceneManager.GetActiveScene().buildIndex;
        healImg.fillAmount = healCount / 5;
        hpFill.fillAmount = hp / hpMax;
    }

    void ContadorFPS()
    {
        fpsCont.text = $"FPS: {(int)(1/Time.deltaTime)}";
    }

    private void Update()
    {
        if(hp > 0 && !inDescanso)
        {
            //Movimenta??o
            MoveControl();

            //Pulo
            JumpControl();

            //Ataque
            AtkControl();

            //Dodge
            DodgeControl();
        }
        

        //Gravidade
        moveVelocity.y += gravity * Time.deltaTime;
        cc.Move(moveVelocity * Time.deltaTime);

        //Controlador de v?riaveis
        JumpTimer();

        //Controlador de anima??es
        AnimationsControl();

        PlayerBarHP();
    }

    void PlayerBarHP()
    {
        hpBarSlider.value = GetHP();
    }
    public float GetHP()
    {
        return hp;
    }

    void MoveControl()
    {
        //Movimenta??o
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

    void DodgeControl()
    {
        if (dodge)
        {
            if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
            Dodge();
        }
        if (dodgeTimer < 0)
        {
            dodge = false;
        }

        //Verificador de Dodge
        if (cc.isGrounded)
        {
            canDodge = true;
            canDodgeTime = 0.15f;
        }
        if (canDodge && !cc.isGrounded)
        {
            canDodgeTime -= Time.deltaTime;
        }
        if (canDodgeTime <= 0 && !cc.isGrounded)
        {
            canDodge = false;
        }
    }
    void Dodge()
    {
        if (dodgeSide) dodgeSpeed -= Time.deltaTime*0.5f;
        else dodgeSpeed -= Time.deltaTime * 6;

        float dodgeOrRoll;
        if (dodgeSide) dodgeOrRoll = dodgeSpeed;
        else dodgeOrRoll = -dodgeSpeed;

        GameObject dodgeOrRollVision;
        if (dodgeSide) dodgeOrRollVision = cam.gameObject;
        else dodgeOrRollVision = frontVision;

        GameObject dodgeOrRollDirection;
        if (dodgeSide) dodgeOrRollDirection = gameObject;
        else dodgeOrRollDirection = cam.gameObject;

        //Dodge

        Vector3 dodgeDirection = new Vector3(movement.x, 0f, dodgeOrRoll);

        float targetAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg + dodgeOrRollDirection.transform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        cc.Move(moveDir * (speed*3) * Time.deltaTime);

        //Rotacionar
        
        float targetBackAngle = Mathf.Atan2(dodgeDirection.x, dodgeDirection.z) * Mathf.Rad2Deg + dodgeOrRollVision.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetBackAngle, ref turnVelocity, 0.05f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
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
        //A??o de pular
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

    void EnemyCollision(float d)
    {
        hp -= d;
        hpFill.fillAmount = hp / hpMax;
        borderDamage.SetTrigger("Dano");
        Vector3 randDamagePos = new(Random.Range(transform.position.x + 0.5f, transform.position.x - 0.5f), Random.Range(transform.position.y + 0.5f, transform.position.y - 0.5f), Random.Range(transform.position.z + 0.5f, transform.position.z - 0.5f));
        Instantiate(bloodParticle, randDamagePos, Quaternion.identity);
    }

    void EnemyContinuosDamage(float takeDamageI)
    {
        if(continuosAtkTimer >= 0 && doDamage)
        {
            continuosAtkTimer -= Time.deltaTime;
        }
        if(continuosAtkTimer < 0 && doDamage)
        {
            continuosAtkTimer = 0.2f;
            hp -= takeDamageI;
            hpFill.fillAmount = hp / hpMax;
            if (spike)borderDamage.SetTrigger("Dano");
            Vector3 randDamagePos = new(Random.Range(transform.position.x + 0.5f, transform.position.x - 0.5f), Random.Range(transform.position.y + 0.5f, transform.position.y - 0.5f), Random.Range(transform.position.z + 0.5f, transform.position.z - 0.5f));
            Instantiate(bloodParticle, randDamagePos, Quaternion.identity);
        }
    }

    public void GetNewXp(float xpValue)
    {
        
    }

    void AnimationsControl()
    {
        //Walk/Idle
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


        //Fall
        if (cc.velocity.y <= -10f)
        {
            anim.SetBool("Fall", true);
        }
        else
        {
            anim.SetBool("Fall", false);
        }

        //Jump
        if (cc.velocity.y >= 1f)
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }

        //Dodge
        anim.SetBool("Dodge", dodge);
        if (dodge)
        {
            if (dodgeSide) anim.SetBool("Roll", dodge);
            else anim.SetBool("Roll", !dodge);
        }
        else
        {
            anim.SetBool("Roll", dodge);
        }

        //Damage
        borderToxic.SetBool("Dano", toxic);


        //Death
        if(hp <= 0)
        {
            anim.SetTrigger("Death");
            gameOverPanel.SetActive(true);
        }

        //Healing
        anim.SetBool("Healing", healing);
    }

    public void PlayerOnMove(InputAction.CallbackContext value)
    {
        if(!inDescanso) movement = value.ReadValue<Vector2>();
        if (movement != Vector2.zero) healing = false;
    }

    public void PlayerOnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            doJump = true;
            jumpTimer = 0.2f;
            healing = false;
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
        if (value.started && !healing && healCount > 0)
        {
            healing = true;
        }
    }

    public void PlayerOnTNT(InputAction.CallbackContext value)
    {

    }

    public void PlayerOnAtk(InputAction.CallbackContext value)
    {
        //Atacar
        if (value.started && !inDescanso)
        {
            doAtk = true;
            comboTimer = 0.25f;
            healing = false;
        }

        //Descansar
        if(value.started && inDescansoArea)
        {
            anim.SetBool("Resting", true);
            descansoPanel.SetActive(false);
            inDescanso = true;
            //gameObject.transform.parent = descansoObj.transform;
            //transform.localPosition = new Vector3(0, 0, 0);
            transform.SetParent(descansoObj.transform);
            Invoke(nameof(PositionDescanso), 0.01f);
            healing = false;
            hp = hpMax;
            hpFill.fillAmount = hp / hpMax;
            healCount = 5;
            healImg.fillAmount = healCount / 5;
        }

        //Prosseguir para pr?xima fase
        if(value.started && inNextLevelArea)
        {
            SceneManager.LoadScene(sceneA + 1);
            //nextLevelPanelDelete.SetActive(true);
            cc.enabled = false;        
        }

        //Voltar para fase anterior
        if (value.started && inBackLevelArea)
        {
            SceneManager.LoadScene(sceneA - 1);
            //backLevelPanelDelete.SetActive(true);
            cc.enabled = false;
        }
    }

    void PositionDescanso()
    {
        transform.localPosition = new(1.22f, 1.3f, -1.31f);
        transform.rotation = Quaternion.identity;
    }

    public void PlayerOnDodge(InputAction.CallbackContext value)
    {
        if (value.started && canDodge && !inDescanso && !dodge)
        {
            if (movement.y > 0)
            {
                dodgeSide = true;
                /*dodgeTimer = 0.45f;
                dodgeSpeed = 1.5f;*/
                dodgeTimer = 0.35f;
                dodgeSpeed = 2.25f;
            }
            else
            {
                dodgeSide = false;
                dodgeTimer = 0.35f;
                dodgeSpeed = 2.25f;
            }
            dodge = true;
            healing = false;
        }

        if(value.started && inDescansoArea)
        {
            gameObject.transform.parent = null;
            anim.SetBool("Resting", false);
            inDescanso = false;
            if (inDescansoArea) descansoPanel.SetActive(true);
            healing = false;
        }
    }

    public void AddHP()
    {
        Debug.Log("Recuperou HP");
        hp += 10;
        hpFill.fillAmount = hp / hpMax;
        if (hp > hpMax) hp = hpMax;
    }
    public void EndHeal()
    {
        healing = false;
    }
    public void UseHeal()
    {
        healCount--;
        healImg.fillAmount = (healCount / 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("atkEnemy_hit") && hp > 0)
        {
            float d = 1;
            EnemyAI enemy = other.GetComponentInParent<EnemyAI>();
            if(enemy != null) d = enemy.damage;
            EnemyCollision(d);
        }

        if (other.gameObject.CompareTag("checkpoint") && hp > 0)
        {
            descansoObj = other.gameObject;
            inDescansoArea = true;
            descansoPanel.SetActive(true);
        }

        if (other.gameObject.CompareTag("nextLevel") && hp > 0)
        {
            inNextLevelArea = other.gameObject;
            inNextLevelArea = true;
            nextLevelPanel.SetActive(true);
        }

        if (other.gameObject.CompareTag("backLevel") && hp > 0)
        {
            inBackLevelArea = other.gameObject;
            inBackLevelArea = true;
            backLevelPanel.SetActive(true);
        }

        if (other.gameObject.CompareTag("cam") && hp > 0)
        {
            cams[0].SetActive(false);
            cams[1].SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("atkEnemy_continuos") && hp > 0)
        {
            doDamage = true;
            if (other.gameObject.transform.parent.CompareTag("trap"))//Espinhos
            {
                spike = true;
                EnemyContinuosDamage(5);
            }
            if (other.gameObject.transform.CompareTag("atkEnemy_continuos") && !spike)//Nevoa
            {
                toxic = true;
                EnemyContinuosDamage(2);
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("atkEnemy_continuos") && hp>0)
        {
            doDamage = false;
            toxic = false;
            spike = false;
        }

        if (other.gameObject.CompareTag("checkpoint"))
        {
            descansoPanel.SetActive(false);
            inDescansoArea = false;
        }

        if (other.gameObject.CompareTag("nextLevel"))
        {
            nextLevelPanel.SetActive(false);
            inNextLevelArea = false;
        }

        if (other.gameObject.CompareTag("backLevel"))
        {
            backLevelPanel.SetActive(false);
            inBackLevelArea = false;
        }

        if (other.gameObject.CompareTag("cam") && hp > 0)
        {
            cams[0].SetActive(true);
            cams[1].SetActive(false);
        }
    }
}
