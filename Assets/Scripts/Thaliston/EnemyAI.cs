using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Animator anim;
    Transform player;//Posição player
    public LayerMask whatIsGround, whatIsPlayer;//Máscara de camadas para verificar chão e player
    //Bala inimiga
    public GameObject bullet;
    //Patrol
    public Vector3 walkPoint;//Posição que o inimigo vai patrulhar
    private bool walkPointSet;//True quando tem um ponto de patrulha setado
    //Attack
    bool alreadyAttacked;//Já atacou
    //States
    public bool playerInListeningRange;//Quando o player está no alcance de audição
    public bool playerInVisionRange;//Quando o  player está no alcance de visão
    public bool playerInAttackRange;//Quando o player está no alcance de ataque
    //
    public float playerDistance;//calculo da magnitude da distancia entre o player e o inimigo
    //
    private bool foundPlayer;//True para quando o jogador esá no alcance de visão ou audição
    //
    public GameObject bloodParticle;
    //

    public GameObject atkBox;

    //Variaveis Principais do inimigo
    [Header("Atributos")]
    public float hpMax;//influencia na vida máxima do inimigo
    [SerializeField]private float hp;//HP atual do inimigo
    public float listening;//influencia na distância que o inimigo ouve o jogador ao redor mesmo sem ver ele, só ouvindo

    //Um objeto separado com um script separado vai chamar um método aqui para reconhecer a colisão com a área de visão
    public GameObject vision;//influencia na distância que o inimigo pode enxergar o jogador

    public float persistent;//influencia em quanto tempo o inimigo continua perseguindo o jogador mesmo sem ver ou ouvir ele
    public float areaPatrol;//influencia no tamanho da área que o inimigo cobre enquanto patrulha
    public float velPatrol;//influencia na velocidade que o inimigo se locomove durante a patrulha
    public float velChase;//influencia na velocidade que o inimigo se locomove enquanto persegue o jogador
    private float velEnemy;//Aceleração e desaceleração da velocidade em que o inimigo patrulha ou corre
    public float velAtk;//influencia em quantos golpes por minuto o inimigo pode desferir (Tempo entre cada ataque)
    public float defChance;//influencia na chance que o inimigo tem (a cada ataque do jogador) de desviar
    public float defDamageIgnored;//influencia em quantos porcento do dano o inimigo consegue ignorar quando realiza a defesa
    public bool defend;//True enquanto o inimigo estiver defendendo
    [SerializeField] bool tryDefend = false;
    [SerializeField] float tryDefTimer = 0f;
    public int enemyType;//Variação de inimigo Range e Melee
    ///enemyType: 0 = melee, 1 = range
    public float attackRange;//Alcance do ataque
    public float damage;//Dano do inimigo
    private bool escaping = false;//Faz o inimigo fugir com pouco HP
    private void Awake()
    {
        velEnemy = velPatrol;
        hp = hpMax;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        if(atkBox != null)
        {
            EnemyAtk atkBoxS = atkBox.GetComponent<EnemyAtk>();
            atkBoxS.SetDamage(damage);
        }
        if(vision != null)
        {
            vision.GetComponent<EnemyVision>().SetPersistense(persistent);
        }
    }

    private void Start()
    {
        Invoke(nameof(ResearchWalkPoint), 2);
    }

    private void Update()
    {
        agent.speed = velEnemy;

        //Retorna a magnitude da distancia entre o player e o inimigo
        playerDistance = (player.transform.position - transform.position).magnitude;

        //Retorna o alcance de audição e alcance de ataque
        playerInListeningRange = Physics.CheckSphere(transform.position, listening, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Retorna se o inimigo estiver vendo ou ouvindo o inimigo
        if (playerInListeningRange || playerInVisionRange) foundPlayer = true;
        if (!playerInListeningRange && !playerInVisionRange) foundPlayer = false;

        //Movimentação inimiga
        if (!foundPlayer && !playerInVisionRange && !defend) Patroling();//Patrulha
        if (foundPlayer && !playerInAttackRange && !escaping && !defend) ChasePlayer();//Corre até o player
        if (foundPlayer && playerInAttackRange && !escaping && !defend) AttackPlayer();//Ataca o player
        AnimationsController();

        if (escaping) ToEscape();//Fugir do player
        if (hp <= 1)
        {
            escaping = true;
        }

        ActiveDef();
    }

    private void Patroling()
    {
        if(velEnemy > velPatrol)
        {
            velEnemy -= Time.deltaTime;
        }
        else
        {
            velEnemy = velPatrol;
        }

        if (!walkPointSet) SearchWalkPoint(); //Se não tiver nenhum ponto setado, procura um lugar para patrulhar

        if (walkPointSet) agent.SetDestination(walkPoint); //Se tiver algum ponto setado, vai na direção dele

        Vector3 distanceToWalkPoint = transform.position - walkPoint; //Distancia entre ele e o ponto de patrulha

        if (distanceToWalkPoint.magnitude < 3f) walkPointSet = false; //Checa se ele se aproximou do ponto setado da patrulha
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-areaPatrol, areaPatrol);
        float randomX = Random.Range(-areaPatrol, areaPatrol);

        walkPoint = new(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //Randomiza uma posição perto dele para patrulhar

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))//Checa se a distancia foi setada em lugar que tenha chão
        {
            walkPointSet = true;
        }
    }

    private void ResearchWalkPoint()
    {
        walkPointSet = false;
        Invoke(nameof(ResearchWalkPoint), 2);
    }

    private void ChasePlayer()
    {
        if (velEnemy < velChase)
        {
            velEnemy += Time.deltaTime;
        }
        else
        {
            agent.speed = velChase;
        }
        agent.SetDestination(player.position);//Seta o destino para o player
    }

    private void ToEscape()
    {
        if (velEnemy < velChase * 2f)
        {
            velEnemy += Time.deltaTime;
        }
        else
        {
            agent.speed = velChase * 2f;
        }

        if (!walkPointSet) SearchWalkPoint(); //Se não tiver nenhum ponto setado, procura um lugar para patrulhar

        if (walkPointSet) agent.SetDestination(walkPoint); //Se tiver algum ponto setado, vai na direção dele

        Vector3 distanceToWalkPoint = transform.position - walkPoint; //Distancia entre ele e o ponto de patrulha

        if (distanceToWalkPoint.magnitude < 3f) walkPointSet = false; //Checa se ele se aproximou do ponto setado da patrulha
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);//Fica parado

        ///Modificar para fazer com que um objeto vazio que ficaria o local de spawn do projétil inimigo ir na direção do inimigo
        ///E fazer com que o inimigo olhe para direção Y do player somente (ou X e Z)
        Vector3 playerPos = player.position;
        Vector3 lookAtPos = new(playerPos.x, transform.position.y, playerPos.z);
        transform.LookAt(lookAtPos); //Olha para o player (Modificar!!!)
        ///(miraArma.)transform.LookAt(player);
        ///transform.LookAt(transform.position.x,player.transform.position.x,transform.position.x);

        if (!alreadyAttacked)
        {
            ///Código de ataque aqui
            anim.SetTrigger("Atk");
            /// ---

            ///Ao realizar o ataque, chamar reset do atk
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), Random.Range(velAtk - velAtk / 2, velAtk + velAtk / 2));
            /// ---
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)//Recebe dano de algum objeto externo
    {
        if(!defend) hp -= damage;
        if (defend) hp -= (damage / defDamageIgnored);
        Vector3 bloodRandPos = new(Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f), Random.Range(transform.position.y + 1, transform.position.y + 2f), Random.Range(transform.position.z - 0.5f, transform.position.z + 0.5f));
        Instantiate(bloodParticle, bloodRandPos, Quaternion.identity);

        if(hp <= 0)
        {
            anim.SetBool("Die", true);
            //Invoke(nameof(DestroyEnemy), 0.5f);
            this.enabled = false;
            agent.enabled = false;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            if(atkBox != null)atkBox.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>().enemiesCurrent++;
            GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>().UpdateEnemCris();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void ActiveDef()
    {
        if (playerDistance <= attackRange*2 && player.GetComponent<PlayerMove>().attacking && !tryDefend && !escaping)
        {
            tryDefend = true;
            tryDefTimer = Random.Range(velAtk - velAtk / 2, velAtk + velAtk / 2);

            if (enemyType == 0 && !defend)
            {
                if (Random.Range(0, 100) <= defChance)
                {
                    defend = true;
                    StartCoroutine(DesactiveDef());
                }
                else
                {
                    defend = false;
                }
            }
            else
            {
                defend = false;
            }
        }

        if (tryDefend)
        {
            if(tryDefTimer >= 0)
            {
                tryDefTimer -= Time.deltaTime;
            }
            if(tryDefTimer < 0)
            {
                tryDefend = false;
            }
        }
    }

    IEnumerator DesactiveDef()
    {
        yield return new WaitForSeconds(0.3f);
        defend = false;/*
        yield return new WaitForSeconds(3.5f);
        tryDefend = false;*/
    }

    private void OnDrawGizmosSelected()//Mostra no editor as linhas com o raio de alcance
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, listening);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, areaPatrol);
    }
    
    void AnimationsController()
    {
        //Andando
        if (!playerInListeningRange && !playerInVisionRange && !playerInAttackRange)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        //Correndo
        if((playerInListeningRange || playerInVisionRange) && !playerInAttackRange)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        //Defendendo
        anim.SetBool("Def", defend);


        //Fugindo
        if (escaping)
        {
            anim.SetBool("Run", true);
        }
    }
    

    public void SetPlayerInVisionRange(bool isVision)
    {
        playerInVisionRange = isVision;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("atkPlayer_hit"))
        {
            TakeDamage(1);
        }
    }
}
