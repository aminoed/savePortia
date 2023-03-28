using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator bulletAnimator;
    private AnimatorStateInfo bulletInfo;

    void Awake()
    {
        bulletAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletInfo = bulletAnimator.GetCurrentAnimatorStateInfo(0);
        if (bulletInfo.normalizedTime >= 1)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
