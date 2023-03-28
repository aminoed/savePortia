using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : Istate
{
    //引用状态机
    private FSM manager;
    private Parameter parameter;
    private AnimatorStateInfo info;
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("Attack");

    }
    public void OnUpdate()
    {
        info = parameter.enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (parameter.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }
        if (info.normalizedTime >= 0.95f && parameter.life > 0)
        {
            manager.TransitionState(StateType.Chase);
        }

    }
    public void OnExit()
    {
        if (parameter.target)
        {
            parameter.target.gameObject.GetComponent<PlayerController>().getHit();
            Debug.Log(parameter.target.gameObject.GetComponent<PlayerController>().life);
        }
    }
}