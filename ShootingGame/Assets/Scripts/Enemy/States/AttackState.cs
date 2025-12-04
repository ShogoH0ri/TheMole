using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    private float moveTimer;      　// 一定時間ごとに移動するためのタイマー
    private float losePlayerTimer;　// プレイヤーを見失ってからの経過時間を計測するタイマー
    private float shotTimer;　　　　// 射撃の間隔を制御するタイマー
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())　　// プレイヤーが視界にいる？
        {
            losePlayerTimer = 0;　　// プレイヤーが見えている間は見失いタイマーをリセット
            moveTimer += Time.deltaTime;　　 // 移動用の時間加算
            shotTimer += Time.deltaTime;　　// 射撃間隔タイマー加算
            enemy.transform.LookAt(enemy.Player.transform);　　// プレイヤーの方向を向く
            if (shotTimer > enemy.fireRate)　　// 一定時間ごとに射撃
            {
                Shoot();
            }
            if (moveTimer > Random.Range(3, 7))　　// 3〜7秒ごとにランダム方向に移動
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnwonPos = enemy.Player.transform.position;　　// 最後に見たプレイヤーの位置を更新
        }
        else
        {
            losePlayerTimer += Time.deltaTime;　　// プレイヤーが見えない間はタイマー加算
            if (losePlayerTimer > 8)　　// 8秒以上見失ったら探索状態へ戻る
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()　　// 射撃処理
    {
        Transform gunbarrel = enemy.gunBarrel;

        // 弾生成
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);

        // プレイヤーへの方向を計算
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

        // 射撃音
        SoundManager.Instance.PlaySound3D("Shooting", gunbarrel.transform.position);

        // 弾に速度を与えて発射
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * 40;
        shotTimer = 0;
    }
}
