using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player; 
    private Vector3 lastKnownPos;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }

    public Vector3 LastKnwonPos { get => lastKnownPos; set => lastKnownPos = value; }

    [SerializeField]
    public float EnemyHealth;

    [SerializeField]
    private string currentState;
    public Pathway path;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;

    [Range(0.1f, 10f)]
    public float fireRate;

    private PlayerMotor motor;
    void Start()
    {
        GameOver();
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
    }
    void Update()
    {
        GameOver();
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public void TakeDamage(float damage)
    {
        //fix later
        SoundManager.Instance.PlaySound3D("DamagedPlayer", transform.position);
        EnemyHealth -= damage;
    }

    public void GameOver()
    {
        if (EnemyHealth <= 0f)
        {
            Destroy(gameObject);

            if (motor != null)
            {
                motor.KillCount();
            }
        }
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            PlayerMotor playerMotor = player.GetComponent<PlayerMotor>();
            bool isPlayerCrouching = playerMotor != null && playerMotor.IsCrouching;

            float effectiveSightDistance = isPlayerCrouching ? sightDistance * 0.5f : sightDistance;
            float effectiveFieldOfView = isPlayerCrouching ? fieldOfView * 0.6f : fieldOfView;

            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (Vector3.Distance(transform.position, player.transform.position) < effectiveSightDistance)
            {
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= effectiveFieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();

                    if (Physics.Raycast(ray, out hitInfo, effectiveSightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * effectiveSightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
