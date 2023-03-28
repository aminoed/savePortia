using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Gun
{
    private bool isShooting;
    private LineRenderer laser;

    protected override void Start()
    {
        base.Start();
        laser=muzzlePos.GetComponent<LineRenderer>();
    }

    protected override void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // if(Input.GetButtonDown("Fire1")){
            isShooting = true;
            laser.enabled=true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // if(Input.GetButtonUp("Fire1")){            
            isShooting = false;
            laser.enabled=false;
        }
        gunAnimator.SetBool("shoot", isShooting);

        if (isShooting)
        {
            GunAttack();
        }
    }

    protected override void GunAttack()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position,Vector2.zero, 30);
        // Debug.Log(muzzlePos.position+"---"+direction);

        // Debug.Log(mousePos+"---"+hit2D.point);
        if(hit2D){Debug.Log(hit2D.collider.name);}
        Debug.DrawLine(muzzlePos.position, hit2D.point, UnityEngine.Color.red);
        // laser.SetPosition(0,muzzlePos.position);
        // laser.SetPosition(1,hit2D.point);
    }
}
