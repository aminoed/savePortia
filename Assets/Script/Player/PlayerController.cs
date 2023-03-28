using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class PlayerController : MonoBehaviour
{
    public GameObject[] guns;
    public Rigidbody2D rb;
    public Animator anim;
    public Vector2 move;
    public Vector2 mousePos;
    public Vector2 input;
    private float deadTime = 1f;
    private float fadeSpeed = 0.01f;
    private SpriteRenderer sprite;
    [Header("属性")]
    public float speed;
    public int life, hunger, key, maxLife, maxHunger;
    public bool isHurt;
    // public Parameter parameter;
    public int gunNum;
    private GameObject pickObject;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        maxLife = life;
        maxHunger = hunger;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchGun();
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        rb.velocity = input.normalized * speed;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
        // Vector2 dir = InputManager.ActiveDevice.LeftStick.Vector;
        // transform.Translate(dir * speed * Time.fixedDeltaTime);
    }

    void SwitchAnim()
    {
        anim.SetFloat("speed", move.magnitude);
        if (hunger <= 0)
        {
            anim.Play("Dead");
            StartCoroutine(Stop());
        }
    }
    // if (Input.GetKeyDown(KeyCode.E) || InputManager.ActiveDevice.Action2.WasPressed == true)
    void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            guns[gunNum].SetActive(false);
            AudioController.instance.ExchangeAudio();
            if (--gunNum < 0)
            {
                gunNum = guns.Length - 1;
            }
            guns[gunNum].SetActive(true);
        }
    }

    public void getHit()
    {
        if (life <= 0 || hunger <= 0)
        {
            anim.SetTrigger("isDead");
            StartCoroutine(Stop());
        }
        else
        {
            life--;
            anim.SetTrigger("isHurt");
        }
    }

    IEnumerator Stop()
    {
        //wait time
        yield return new WaitForSeconds(deadTime);
        //set zero
        rb.velocity = Vector2.zero;

        while (sprite.color.a > 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - fadeSpeed);
            yield return new WaitForFixedUpdate();
        }
        // Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("key"))
        {
            pickObject = other.collider.gameObject;
            ObjectPool.Instance.PushObject(pickObject);
            AudioController.instance.KeyAudio();
            key++;
        }
        else if (other.collider.CompareTag("hungery"))
        {
            pickObject = other.collider.gameObject;
            AudioController.instance.DrinkAudio();
            ObjectPool.Instance.PushObject(pickObject);
            hunger += UnityEngine.Random.Range(1, maxHunger - hunger);
        }
        else if (other.collider.CompareTag("blood"))
        {
            pickObject = other.collider.gameObject;
            AudioController.instance.DrinkAudio();
            ObjectPool.Instance.PushObject(pickObject);
            life += UnityEngine.Random.Range(1, maxLife - life);
        }
        if (other.collider.tag == "Princess")
        {
            GameObject.Find("Attri").GetComponent<playerAttri>().win();
            ObjectPool.Instance.PushObject(other.collider.gameObject);
            ObjectPool.Instance.PushObject(transform.gameObject);
        }
    }

}


