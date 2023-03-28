using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PatrolState : Istate
{
    //引用状态机
    private FSM manager;
    private Parameter parameter;

    private int patrolPos;
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("Move");
        manager.GetNewWayPoint();
    }

    public void OnUpdate()
    {
        manager.FlipTo(parameter.patrolPoint);

        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.patrolPoint, parameter.moveSpeed * Time.deltaTime);

        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }

        if (parameter.target != null && Mathf.Abs(parameter.target.position.x - manager.transform.position.x) <= parameter.chaseRange)
        {
            manager.TransitionState(StateType.Chase);
        }

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoint) < 0.1f)
        {
            manager.TransitionState(StateType.Idle);
        }
    }
    public void OnExit()
    {
        // manager.GetNewWayPoint();
    }
}
