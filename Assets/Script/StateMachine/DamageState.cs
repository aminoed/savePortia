using System;
using UnityEngine;

public class DamageState : Istate
{
    //引用状态机
    private FSM manager;
    private Parameter parameter;

    private AnimatorStateInfo info;

    public DamageState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("Damaged");
        parameter.life--;
    }

    public void OnUpdate()
    {
        info = parameter.enemyAnimator.GetCurrentAnimatorStateInfo(0);

        if (parameter.life <= 0)
        {
            manager.TransitionState(StateType.Death);
        }
        else if(info.normalizedTime >= 0.95f)
        {
            parameter.target = GameObject.FindWithTag("Player").transform;
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        parameter.getHit = false;
    }
}