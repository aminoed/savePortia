using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormIdleState : Istate
{
    private Fireworm manager;
    private WormParameter wormPara;

    public WormIdleState(Fireworm manager)
    {
        this.manager = manager;
        this.wormPara = manager.wormPara;
    }
    public void OnEnter()
    {
        wormPara.enemyAnimator.Play("Idle");

    }

    public void OnUpdate()
    {
        if (wormPara.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }

        if (wormPara.target != null && Physics2D.OverlapCircle(wormPara.attackPoint.position, wormPara.attackArea, wormPara.targetLayer))
        {
            manager.TransitionState(StateType.Attack);
        }
    }

    public void OnExit()
    {

    }
}


public class WormAttackState : Istate
{
    private Fireworm manager;
    private WormParameter wormPara;
    private AnimatorStateInfo info;
    public WormAttackState(Fireworm manager)
    {
        this.manager = manager;
        this.wormPara = manager.wormPara;
    }

    public void OnEnter()
    {
        manager.isAttack = true;
        wormPara.enemyAnimator.Play("Attack");
        manager.Fire();
    }

    public void OnUpdate()
    {
        if (wormPara.target)
            manager.FlipTo(wormPara.target.position);
        info = wormPara.enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (wormPara.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }
        if (info.normalizedTime >= 0.95f && wormPara.life > 0)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        if (wormPara.target)
        {
            wormPara.target.gameObject.GetComponent<PlayerController>().getHit();
            Debug.Log(wormPara.target.gameObject.GetComponent<PlayerController>().life);
        }
    }
}

public class WormChaseState : Istate
{
    private Fireworm manager;
    private WormParameter wormPara;

    public WormChaseState(Fireworm manager)
    {
        this.manager = manager;
        this.wormPara = manager.wormPara;
    }
    public void OnEnter()
    {
        wormPara.enemyAnimator.Play("Move");
    }

    public void OnUpdate()
    {

        if (wormPara.target)
        {
            manager.FlipTo(wormPara.target.position);
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                        wormPara.target.position, wormPara.chaseSpeed * Time.deltaTime);
            Debug.DrawLine(manager.transform.position, manager.transform.position + wormPara.target.position * 1.8f, Color.red);

            if (Physics2D.Raycast(manager.transform.position, wormPara.target.position, 1.8f, LayerMask.GetMask("Player")))
            {
                // Debug.Log("chase准备attack");
                manager.TransitionState(StateType.Attack);

            }
        }
        if (wormPara.getHit)
        {
            manager.TransitionState(StateType.Damaged);
        }

        if (wormPara.target == null || Mathf.Abs(wormPara.target.position.x - manager.transform.position.x) > wormPara.chaseRange)
        {
            // Debug.Log("chase准备idle" + wormPara.target);
            manager.TransitionState(StateType.Idle);
        }

    }
    public void OnExit()
    {

    }
}

public class WormDamageState : Istate
{
    private Fireworm manager;
    private WormParameter wormPara;

    private AnimatorStateInfo info;

    public WormDamageState(Fireworm manager)
    {
        this.manager = manager;
        this.wormPara = manager.wormPara;
    }
    public void OnEnter()
    {
        wormPara.enemyAnimator.Play("Damaged");
        wormPara.life--;
    }

    public void OnUpdate()
    {
        info = wormPara.enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (wormPara.life <= 0.5 * manager.maxLife)
        {
            manager.TransitionState(StateType.Idle);
        }
        if (wormPara.life <= 0)
        {
            manager.TransitionState(StateType.Death);
        }
        else if (info.normalizedTime >= 0.95f)
        {
            wormPara.target = GameObject.FindWithTag("Player").transform;
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        wormPara.getHit = false;
    }

}

public class WormDeathState : Istate
{
    private Fireworm manager;
    private WormParameter wormPara;

    private AnimatorStateInfo info;
    private float timer = 0;
    public WormDeathState(Fireworm manager)
    {
        this.manager = manager;
        this.wormPara = manager.wormPara;
    }
    public void OnEnter()
    {
        wormPara.enemyAnimator.Play("Dead");
    }

    public void OnUpdate()
    {
        info = wormPara.enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            wormPara.sprite.color = new Color(0.53f, 0.53f, 0.53f, 1);
            manager.GetComponent<Collider2D>().enabled = false;
            manager.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        timer += Time.deltaTime;
    }

    public void OnExit()
    {

    }
}
