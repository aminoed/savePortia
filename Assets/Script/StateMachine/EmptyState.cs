using System;
using UnityEngine;
using System.Collections;

public class EmptyState : Istate
{
        //引用状态机
    private FSM manager;
    private Parameter parameter;
    public EmptyState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {

    }
}
