using System.Net.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle, Patrol, Chase, Attack, Damaged, Death, empty
}

//Enemies's Parameter
[Serializable]
public class Parameter
{
    public Animator enemyAnimator;
    public SpriteRenderer sprite;
    public bool getHit;
    public float time;

    [Header("追击")]
    //追击速度
    public float chaseSpeed;
    //视线范围(追击范围)
    public float chaseRange;
    [Header("巡逻")]
    //巡逻速度
    public float moveSpeed;
    //巡逻范围
    public float patrolRange;
    //巡逻点
    // public Transform patrolPoint;
    public Vector2 patrolPoint;
    // public GameObject Area;
    [Header("攻击")]
    //攻击目标
    public Transform target;
    //目标所在图层
    public LayerMask targetLayer;
    //目标点transform
    public Transform attackPoint;
    public int life;

    //攻击范围
    public float attackArea;
    //敌人出生点
    public Vector2 bornPoint;
}

public class FSM : MonoBehaviour
{
    //多态
    protected Istate currentState;
    protected Dictionary<StateType, Istate> states = new Dictionary<StateType, Istate>();
    public Parameter parameter;
    public GameObject enemyGen;
    public GameObject[] bonus;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //注册所有状态，将引用传给状态
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Damaged, new DamageState(this));
        states.Add(StateType.Death, new DeathState(this));
        states.Add(StateType.empty, new EmptyState(this));
        //设置初始状态
        TransitionState(StateType.Idle);

        parameter.enemyAnimator = transform.GetComponent<Animator>();
        parameter.bornPoint = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentState.OnUpdate();
        Debug.DrawLine(transform.position, (Vector2)transform.position + parameter.patrolPoint * 0.8f, Color.red);
        if (Physics2D.Raycast(transform.position, parameter.patrolPoint, 0.8f, LayerMask.GetMask("Wall")))
        {
            if (transform.tag == "Enemy")
                GetNewWayPoint();
        }
    }
    public void TransitionState(StateType type)
    {
        //转移状态，退出前一状态
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    //更改朝向
    public void FlipTo(Vector2 target)
    {
        if (target != null)
        {
            if (transform.position.x > target.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else if (transform.position.x < target.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }
    //在Patrol范围内，也就是视线范围，追击范围内
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
        if (other.CompareTag("Bullet"))
        {
            parameter.getHit = true;
            parameter.life--;
        }
        if (other.CompareTag("RoomArea"))
        {
            enemyGen = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }

    //找到巡逻点
    public void GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-parameter.patrolRange, parameter.patrolRange);
        float randomY = UnityEngine.Random.Range(-parameter.patrolRange, parameter.patrolRange);

        Vector2 randomPoint = new Vector2(parameter.bornPoint.x + randomX, parameter.bornPoint.y + randomY);
        parameter.patrolPoint = randomPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(parameter.bornPoint, parameter.patrolRange);
    }
}
