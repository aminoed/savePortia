using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : Istate
{
    //引用状态机
    private FSM manager;
    private Parameter parameter;

    private float timer;
    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("Idle");

    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }


        if (parameter.target != null && Mathf.Abs(parameter.target.position.x - manager.transform.position.x) <= parameter.chaseRange)
        {
            manager.TransitionState(StateType.Chase);
        }

        if (timer >= parameter.time)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }
}
