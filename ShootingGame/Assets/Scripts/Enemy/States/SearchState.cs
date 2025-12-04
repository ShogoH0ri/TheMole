using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;　　// 探索している時間を計測
    private float moveTimer;　　// 周辺を移動するためのタイマー
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnwonPos);　　// 探索開始：最後に確認したプレイヤー位置へ移動
    }

    public override void Perform()
    {

        Animator anim = enemy.GetComponent<Animator>();　　// アニメーション処理: 移動速度に応じてSpeedパラメータ更新

        if (anim != null)
        {
            float speed = enemy.Agent.velocity.magnitude;
            anim.SetFloat("Speed", speed);
        }

        if (enemy.CanSeePlayer())  // プレイヤーが見えた瞬間に攻撃へ戻る
            stateMachine.ChangeState(new AttackState());


        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)  // NavMeshAgentが移動完了したかどうか判定
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 5))  // ランダムに3〜5ごとに周囲を歩き回る
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }
            if (searchTimer > 10)　　// 10秒探索しても見つからなかったらパトロール状態に戻る
            {
                stateMachine.ChangeState(new ControlState());
            }
        }
    }

    public override void Exit()
    {

    }
}
