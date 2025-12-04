using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    private StateMachine stateMachine;　　// ステート管理用
    private NavMeshAgent agent;　　　　　// NavMeshAgent（移動の制御）
    private GameObject player; 　　　　　// プレイヤーへの参照
    private Vector3 lastKnownPos;　　　　// 最後に見たプレイヤーの位置

    // プロパティ
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnwonPos { get => lastKnownPos; set => lastKnownPos = value; }

    [SerializeField]
    public float EnemyHealth;

    [SerializeField]
    public Pathway path;　　// 巡回ルート

    [Header("Sight Values")]
    public float sightDistance = 20f;　　// 視界距離
    public float fieldOfView = 85f;　　　// 視野角
    public float eyeHeight;　　　　　　　// 視点の高さ

    [Header("Weapon Values")]
    public Transform gunBarrel;　　// 弾を撃つ位置

    [Range(0.1f, 10f)]
    public float fireRate;　　// 射撃の間隔

    private PlayerMotor motor;
    void Start()
    {
        GameOver();　　// 健康チェック
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();　　// 初期ステートを設定
        player = GameObject.FindGameObjectWithTag("Player");　　// プレイヤー参照
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();　　// キルカウントのために PlayerMotor を取得
    }
    void Update()
    {
        GameOver();　　// 体力を常に確認
        CanSeePlayer();　　// 現在の視界状態を更新
    }

    public void TakeDamage(float damage)
    {
        SoundManager.Instance.PlaySound3D("DamagedPlayer", transform.position);  // ダメージを食らう声
        EnemyHealth -= damage;
    }

    public void GameOver()　　// HPが0以下になったら自身を破壊し、KillCountを加算
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
            // プレイヤーがしゃがんでいるかどうかで視界を弱体化
            PlayerMotor playerMotor = player.GetComponent<PlayerMotor>();
            bool isPlayerCrouching = playerMotor != null && playerMotor.IsCrouching;

            float effectiveSightDistance = isPlayerCrouching ? sightDistance * 0.5f : sightDistance;
            float effectiveFieldOfView = isPlayerCrouching ? fieldOfView * 0.6f : fieldOfView;

            // プレイヤーへの方向、そして目の高さから照射
            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            // 距離チェック
            if (Vector3.Distance(transform.position, player.transform.position) < effectiveSightDistance)
            {
                // 視野角チェック
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= effectiveFieldOfView)
                {
                    // Raycast がプレイヤーに当たった場合のみ見えている扱い
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
