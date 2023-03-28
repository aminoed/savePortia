using System;
using UnityEngine;
using System.Collections;

public class DeathState : Istate
{
    //引用状态机
    private FSM manager;
    private Parameter parameter;

    private AnimatorStateInfo info;
    private float timer = 0;
    public DeathState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("Dead");
    }

    public void OnUpdate()
    {
        info = parameter.enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            parameter.sprite.color = new Color(0.53f, 0.53f, 0.53f, 1);
            manager.GetComponent<Collider2D>().enabled = false;            
            manager.GetComponent<SpriteRenderer>().sortingOrder = -1;
            manager.TransitionState(StateType.empty);
        }
        timer += Time.deltaTime;
    }

    public void OnExit()
    {
        manager.enemyGen.GetComponent<EnemyManager>().RemoveFromList(manager.gameObject);
        float percent = UnityEngine.Random.Range(0f,1f);
        if(manager.bonus!=null && percent>=0.5){
            GameObject bottle =ObjectPool.Instance.GetObject(manager.bonus[UnityEngine.Random.Range(0,1)]);
            bottle.transform.position = manager.gameObject.transform.position;
        }
        // Debug.Log("死了");
    }
}
