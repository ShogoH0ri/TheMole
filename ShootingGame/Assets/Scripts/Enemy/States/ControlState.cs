using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState : BaseState
{

    public int waypointIndex;　　// 現在向かっているウェイポイントの番号
    public float waitTimer;　　　// ウェイポイントに到達した際に停止する時間
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        Animator anim = enemy.GetComponent<Animator>();　　// Animatorを取得し、移動速度に応じてアニメの速度パラメータを更新

        if (anim != null)
        {
            float speed = enemy.Agent.velocity.magnitude;　　// NavMeshAgentの移動スピード
            anim.SetFloat("Speed", speed);
        }
        PatrolCycle();　　// 巡回処理
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)　　// 目的地にある程度近づいたかどうか
        {
            waitTimer += Time.deltaTime;　　// 待機時間を加算
            if (waitTimer > 3)　　// 3秒以上経過 → 次のポイントへ
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)　　// 最後のポイントなら0番にループ
                    waypointIndex++;
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);　　// 次のポイントへ移動
                waitTimer = 0;
            }
        }
    }
}
