using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float lerp;
    public float speed=15;
    public GameObject explosionPrefab;
    
    protected Rigidbody2D rocketRb;
    protected Vector3 targetPos;
    protected Vector3 direction;
    protected bool isArrived;

    protected virtual void Awake() {
        rocketRb=GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Vector2 _target){
        isArrived=false;
        targetPos=_target;
    }

    protected virtual void FixedUpdate() {
        direction = (targetPos-transform.position).normalized;

        if(!isArrived){
            transform.right=Vector3.Slerp(
transform.right, direction,lerp/Vector2.Distance(transform.position,targetPos));
            rocketRb.velocity=transform.right*speed;
        }
        if(Vector2.Distance(transform.position,targetPos)<1f &&!isArrived){
            isArrived=true;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        GameObject exposion=ObjectPool.Instance.GetObject(explosionPrefab);
        exposion.transform.position=transform.position;

        rocketRb.velocity=Vector2.zero;
        StartCoroutine(Push(gameObject,0.3f));
    }

    IEnumerator Push(GameObject _object,float time){
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.PushObject(_object);
    }
}
