using UnityEngine;
using System.Collections;

[AddComponentMenu("Game/Shot Pattern/Linear Shot")]
public class LinearShot : BaseShot
{
    [Range(0f, 360f)]
    public float angle = 180f;

    public float betweenDelay = 0.1f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {

        if (_shot != null)
        {
            StopCoroutine(_shot);
            //FinishedShot();
        }

        _shot = ShotCoroutine();

        StartCoroutine(_shot);
    }

    public override void StopShot()
    {
        if (_shot != null)
        {
            StopCoroutine(_shot);
            FinishedShot();
            _shot = null;
        }
    }

    IEnumerator ShotCoroutine()
    {
        if (bulletNum <= 0 || bulletSpeed <= 0f)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
            yield break;
        }
        if (_shooting)
        {
            yield break;
        }
        _shooting = true;

        for (int i = 0; i < bulletNum; i++)
        {
            if (0 < i && 0f < betweenDelay)
            {
                yield return new WaitForSeconds(betweenDelay);
            }



            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }
            bullet.targetTag = targetTagName;


            ShotBullet(bullet, bulletSpeed, angle);

        }
        FinishedShot();
    }
}