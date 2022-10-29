using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]Animator anim;
    Transform player;//Posi��o player
    public LayerMask whatIsGround, whatIsPlayer;//M�scara de camadas para verificar ch�o e player
    //Bala inimiga
    public GameObject bullet;
    //Patrol
    public Vector3 walkPoint;//Posi��o que o inimigo vai patrulhar
    private bool walkPointSet;//True quando tem um ponto de patrulha setado
    //Attack
    bool alreadyAttacked;//J� atacou
    //States
    public bool playerInListeningRange;//Quando o player est� no alcance de audi��o
    public bool playerInVisionRange;//Quando o  player est� no alcance de vis�o
    public bool playerInAttackRange;//Quando o player est� no alcance de ataque
    //
    public float playerDistance;//calculo da magnitude da distancia entre o player e o inimigo
    //
    private bool foundPlayer;//True para quando o jogador es� no alcance de vis�o ou audi��o
    //
    public GameObject bloodParticle;
    //

    //Variaveis Principais do inimigo
    [Header("Atributos")]
    public float hpMax;//influencia na vida m�xima do inimigo
    private float hp;//HP atual do inimigo
    public float listening;//influencia na dist�ncia que o inimigo ouve o jogador ao redor mesmo sem ver ele, s� ouvindo

    //Um objeto separado com um script separado vai chamar um m�todo aqui para reconhecer a colis�o com a �rea de vis�o
    public float vision;//influencia na dist�ncia que o inimigo pode enxergar o jogador

    public float persistent;//influencia em quanto tempo o inimigo continua perseguindo o jogador mesmo sem ver ou ouvir ele
    private float persistentTimer;//Valor atual do contador de persistencia
    public float areaPatrol;//influencia no tamanho da �rea que o inimigo cobre enquanto patrulha
    public float velPatrol;//influencia na velocidade que o inimigo se locomove durante a patrulha
    public float velChase;//influencia na velocidade que o inimigo se locomove enquanto persegue o jogador
    public float velAtk;//influencia em quantos golpes por minuto o inimigo pode desferir (Tempo entre cada ataque)
    public float defChance;//influencia na chance que o inimigo tem (a cada ataque do jogador) de desviar
    public float defDamageIgnored;//influencia em quantos porcento do dano o inimigo consegue ignorar quando realiza a defesa
    public bool defend;//True enquanto o inimigo estiver defendendo
    public int enemyType;//Varia��o de inimigo Range e Melee
    ///enemyType: 0 = melee, 1 = range
    public float attackRange;//Alcance do ataque

    private void Awake()
    {
        hp = hpMax;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Retorna a magnitude da distancia entre o player e o inimigo
        playerDistance = (player.transform.position - transform.position).magnitude;

        //Retorna o alcance de audi��o e alcance de ataque
        playerInListeningRange = Physics.CheckSphere(transform.position, listening, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Retorna se o inimigo estiver vendo ou ouvindo o inimigo
        if (playerInListeningRange || playerInVisionRange) foundPlayer = true;
        if (!playerInListeningRange && !playerInVisionRange) foundPlayer = false;

        //Movimenta��o inimiga
        if (!foundPlayer && !playerInVisionRange) Patroling();//Patrulha
        if (foundPlayer && !playerInAttackRange) ChasePlayer();//Corre at� o player
        if (foundPlayer && playerInAttackRange) AttackPlayer();//Ataca o player

        PersistenTimer();
        AnimationsController();
    }

    void PersistenTimer()//Tempo que continua seguindo ap�s perder de vista
    {

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint(); //Se n�o tiver nenhum ponto setado, procura um lugar para patrulhar

        if (walkPointSet) agent.SetDestination(walkPoint); //Se tiver algum ponto setado, vai na dire��o dele

        Vector3 distanceToWalkPoint = transform.position - walkPoint; //Distancia entre ele e o ponto de patrulha

        if (distanceToWalkPoint.magnitude < 3f) walkPointSet = false; //Checa se ele se aproximou do ponto setado da patrulha
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-areaPatrol, areaPatrol);
        float randomX = Random.Range(-areaPatrol, areaPatrol);

        walkPoint = new(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //Randomiza uma posi��o perto dele para patrulhar

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))//Checa se a distancia foi setada em lugar que tenha ch�o
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);//Seta o destino para o player
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);//Fica parado

        ///Modificar para fazer com que um objeto vazio que ficaria o local de spawn do proj�til inimigo ir na dire��o do inimigo
        ///E fazer com que o inimigo olhe para dire��o Y do player somente (ou X e Z)
        transform.LookAt(player); //Olha para o player (Modificar!!!)
        ///(miraArma.)transform.LookAt(player);
        ///transform.LookAt(transform.position.x,player.transform.position.x,transform.position.x);

        if (!alreadyAttacked)
        {
            ///C�digo de ataque aqui
            anim.SetTrigger("Atk");
            /// ---

            ///Ao realizar o ataque, chamar reset do atk
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), velAtk);
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

        if(hp <= 0)
        {
            anim.SetBool("Die", true);
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void ActiveDef()
    {
        if(playerDistance < attackRange && enemyType == 0)
        {
            if (Random.Range(0, 100) <= defChance)
            {
                defend = false;

            }
            else
            {
                defend = true;
            }
        }
        else
        {
            defend = false;
        }
        Invoke(nameof(ActiveDef), 1);
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
        
    }

    public void MeleeAtkActive()
    {

    }

    public void MeleeAtkDesactive()
    {

    }

    public void RangeAtk()
    {
        Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32, ForceMode.Impulse);
        rb.AddForce(transform.up * 8, ForceMode.Impulse);
    }
}
