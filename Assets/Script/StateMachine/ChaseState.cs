using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : Istate
{
    //引用状态机
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("Move");
    }

    public void OnUpdate()
    {

        if (parameter.target)
        {
            manager.FlipTo(parameter.target.position);
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                        parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
        }
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }

        if (parameter.target == null || Mathf.Abs(parameter.target.position.x - manager.transform.position.x) > parameter.chaseRange)
        {
            // Debug.Log("chase准备idle" + parameter.target);
            manager.TransitionState(StateType.Idle);
        }
        // Debug.Log(parameter.targetLayer);
        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            // Debug.Log("chase准备attack");
            manager.TransitionState(StateType.Attack);
        }


    }
    public void OnExit()
    {

    }
}
