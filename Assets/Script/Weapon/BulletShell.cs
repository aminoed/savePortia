using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{

    public float shellSpeed;
    public float shellStopT = 0.5f;
    public float shellFadeSpeed = 0.01f;
    private Rigidbody2D shellRb;
    private SpriteRenderer shellSprite;

    void Awake()
    {
        shellRb = GetComponent<Rigidbody2D>();
        shellSprite = GetComponent<SpriteRenderer>();
    }

    //shell use objectpool, won't call Awake(), only call OnEnable() when actived
    private void OnEnable()
    {
        float angel = Random.Range(-30f, 30f);
        shellRb.velocity = Quaternion.AngleAxis(angel, Vector3.forward) * Vector3.up * shellSpeed;

        //reset color&gravity
        shellSprite.color = new Color(shellSprite.color.r, shellSprite.color.g, shellSprite.color.b, 1);
        shellRb.gravityScale = 3;
        StartCoroutine(Stop());
    }

    //协程
    IEnumerator Stop()
    {
        //wait time
        yield return new WaitForSeconds(shellStopT);
        //set zero
        shellRb.velocity = Vector2.zero;
        shellRb.gravityScale = 0;

        while (shellSprite.color.a > 0)
        {
            shellSprite.color = new Color(shellSprite.color.r, shellSprite.color.g, shellSprite.color.b, shellSprite.color.a - shellFadeSpeed);
            yield return new WaitForFixedUpdate();
        }
        ObjectPool.Instance.PushObject(gameObject);
    }
}
