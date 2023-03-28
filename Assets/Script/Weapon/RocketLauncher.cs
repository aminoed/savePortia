using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    public int rocketNum = 4;
    public float rocketAngle = 15;

    protected override void GunAttack()
    {
        gunAnimator.SetTrigger("shoot");
        AudioController.instance.RocketAudio();
        gameObject.GetComponentInParent<PlayerController>().hunger -= rocketNum;
        StartCoroutine(DelayFire(0.2f));
    }

    IEnumerator DelayFire(float delayT)
    {
        yield return new WaitForSeconds(delayT);
        int median = rocketNum / 2;
        for (int i = 0; i < rocketNum; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePos.position;

            if (rocketNum % 2 == 1)
            {
                bullet.transform.right = Quaternion.AngleAxis(rocketAngle * (i - median), Vector3.forward) * direction;
            }
            else
            {
                bullet.transform.right = Quaternion.AngleAxis(rocketAngle * (i - median) * rocketAngle / 2, Vector3.forward) * direction;
            }
            bullet.GetComponent<Rocket>().SetTarget(mousePos);
        }

    }
}
