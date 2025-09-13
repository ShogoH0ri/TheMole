using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnwonPos);
    }

    public override void Perform()
    {

        Animator anim = enemy.GetComponent<Animator>();

        if (anim != null)
        {
            float speed = enemy.Agent.velocity.magnitude;
            anim.SetFloat("Speed", speed);
        }

        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(new AttackState());


        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }
            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new ControlState());
            }
        }
    }

    public override void Exit()
    {

    }
}
