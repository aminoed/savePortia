using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject explosionPrefab;
    private Rigidbody2D bulletRb;

    void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(Vector2 direction)
    {
        bulletRb.velocity = direction * bulletSpeed;
    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        GameObject explosion = ObjectPool.Instance.GetObject(explosionPrefab);
        explosion.transform.position = transform.position;

        ObjectPool.Instance.PushObject(gameObject);
    }
}
