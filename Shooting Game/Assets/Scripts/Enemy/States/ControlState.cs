using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState : BaseState
{

    public int waypointIndex;
    public float waitTimer;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        Animator anim = enemy.GetComponent<Animator>();

        if (anim != null )
        {
            float speed = enemy.Agent.velocity.magnitude;
            anim.SetFloat("Speed", speed);
        }
        PatrolCycle();
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }
}
