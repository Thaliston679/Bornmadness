using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public int HP;

    //Bullet
    public GameObject bullet;

    //Patrol
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerIsSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //verifica a visão e alcance de ataque
        playerIsSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerIsSightRange && !playerInAttackRange) Patroling();
        if (playerIsSightRange && !playerInAttackRange) ChasePlayer();
        if (playerIsSightRange && playerInAttackRange) AttackPlayer();

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint(); //Se não tiver nenhum ponto setado, procura um lugar para patrulhar

        if (walkPointSet) agent.SetDestination(walkPoint); //Se tiver algum ponto setado, vai na direção dele

        Vector3 distanceToWalkPoint = transform.position - walkPoint; //Distancia entre ele e o ponto de patrulha

        if (distanceToWalkPoint.magnitude < 3f) walkPointSet = false; //Checa se ele se aproximou do ponto setado da patrulha
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //Randomiza uma posição perto dele para patrulhar

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))//Checa se a distancia foi setada em lugar que tenha chão
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

        ///Modificar para fazer com que um objeto vazio que ficaria o local de spawn do projétil inimigo ir na direção do inimigo
        ///E fazer com que o inimigo olhe para direção Y do player somente (ou X e Z)
        transform.LookAt(player); //Olha para o player (Modificar!!!)
        ///(miraArma.)transform.LookAt(player);
        ///transform.LookAt(transform.position.x,player.transform.position.x,transform.position.x);

        if (!alreadyAttacked)
        {
            ///Código de ataque aqui
            Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32, ForceMode.Impulse);
            rb.AddForce(transform.up * 8, ForceMode.Impulse);
            /// ---

            ///Ao realizar o ataque, chamar reset do atk
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            /// ---
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)//Recebe dano de algum objeto externo
    {
        HP -= damage;

        if(HP <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()//Mostra no editor as linhas com o raio de alcance
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
