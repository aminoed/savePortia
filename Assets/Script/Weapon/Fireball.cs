using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Rocket
{
    public Animator fireBallAnim;

    protected override void Awake()
    {
        base.Awake();
        fireBallAnim = GetComponent<Animator>();
    }
    protected override void FixedUpdate()
    {
        direction = (targetPos - transform.position).normalized;

        if (!isArrived)
        {
            transform.right = Vector3.Slerp(
transform.right, direction, lerp / Vector2.Distance(transform.position, targetPos));
            rocketRb.velocity = transform.right * speed;
        }
        if (Vector2.Distance(transform.position, targetPos) < 1f && !isArrived)
        {
            isArrived = true;
        }
    }
}
