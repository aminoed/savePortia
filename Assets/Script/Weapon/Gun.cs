using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Gun : MonoBehaviour
{
    public float interval;
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    protected Transform muzzlePos;
    protected Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected Animator gunAnimator;
    protected float timer;
    protected float flipY;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        gunAnimator = GetComponent<Animator>();
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        flipY = transform.localScale.y;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(flipY, -flipY, 1);
        }
        else
        {
            transform.localScale = new Vector3(flipY, flipY, 1);
        }
        Shoot();
    }

    protected virtual void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }

        // if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0) || InputManager.ActiveDevice.Action1.WasPressed == true)
        {
            if (timer == 0)
            {
                timer = interval;
                GunAttack();
            }
        }
    }
    protected virtual void GunAttack()
    {

        gunAnimator.SetTrigger("shoot");
        gameObject.GetComponentInParent<PlayerController>().hunger -= 1;

        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = muzzlePos.position;

        float angel = UnityEngine.Random.Range(-5f, 5f);
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angel, Vector3.forward) * direction);

        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
