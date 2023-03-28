using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum StateType
// {
//     Idle, Patrol, Chase, Attack, Damaged, Death
// }

[Serializable]
public class WormParameter
{
    public Animator enemyAnimator;
    public SpriteRenderer sprite;
    public bool getHit;
    [Header("追击")]
    //追击速度
    public float chaseSpeed;
    //视线范围(追击范围)
    public float chaseRange;
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
}
public class Fireworm : MonoBehaviour
{
    protected Transform firePos;
    protected Vector2 direction;
    public bool getHit;
    public GameObject fireballPrefab;
    public float interval;
    protected float timer;
    public bool isAttack = false;
    public int fireNum = 1;
    public float fireAngle = 0;
    public int maxLife;
    protected Istate currentState;
    protected Dictionary<StateType, Istate> states = new Dictionary<StateType, Istate>();
    public WormParameter wormPara;
    void Start()
    {

        states.Add(StateType.Idle, new WormIdleState(this));
        states.Add(StateType.Attack, new WormAttackState(this));
        states.Add(StateType.Chase, new WormChaseState(this));
        states.Add(StateType.Damaged, new WormDamageState(this));
        states.Add(StateType.Death, new WormDeathState(this));
        //设置初始状态
        TransitionState(StateType.Idle);

        wormPara.enemyAnimator = transform.GetComponent<Animator>();
        firePos = transform.Find("FirePoint");
        maxLife = wormPara.life;
    }

    void Update()
    {
        currentState.OnUpdate();
        Fire();
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
    public void Fire()
    {
        transform.right = direction;
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
        if (isAttack == true)
        {
            if (timer == 0)
            {
                timer = interval;
                FireAttack();
            }

        }
    }
    public void FireAttack()
    {
        StartCoroutine(DelayFire(0.2f));
    }

    IEnumerator DelayFire(float delayT)
    {
        yield return new WaitForSeconds(delayT);
        int median = fireNum / 2;
        for (int i = 0; i < fireNum; i++)
        {
            GameObject fireball = ObjectPool.Instance.GetObject(fireballPrefab);
            // fireball.GetComponent<Animator>().SetTrigger("attack");
            fireball.transform.position = firePos.position;

            if (fireNum % 2 == 1)
            {
                fireball.transform.right = Quaternion.AngleAxis(fireAngle * (i - median), Vector3.forward) * direction;
            }
            else
            {
                fireball.transform.right = Quaternion.AngleAxis(fireAngle * (i - median) * fireAngle / 2, Vector3.forward) * direction;
            }
            fireball.GetComponent<Fireball>().SetTarget(wormPara.target.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wormPara.target = other.transform;
        }
        if (other.CompareTag("Bullet"))
        {
            wormPara.getHit = true;
            wormPara.life--;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wormPara.target = null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wormPara.attackPoint.position, wormPara.attackArea);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wormPara.attackPoint.position, wormPara.chaseRange);
    }
}
