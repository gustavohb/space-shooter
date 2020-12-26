using System.Collections;
using UnityEngine;

/// <summary>
/// Waving nway shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Waving nWay Shot")]
public class WavingNwayShot : BaseShot
{
    // "Set a number of shot way."
    public int wayNum = 5;
    // "Set a center angle of wave range. (0 to 360)"
    [Range(0f, 360f)]
    public float waveCenterAngle = 180f;
    // "Set a size of wave range. (0 to 360)"
    [Range(0f, 360f)]
    public float waveRangeSize = 40f;
    // "Set a speed of wave. (0 to 10)"
    [Range(0f, 10f)]
    public float waveSpeed = 5f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float betweenAngle = 5f;
    // "Set a delay time between shot and next line shot. (sec)"
    public float nextLineDelay = 0.1f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        if (_shot != null)
        {
            StopCoroutine(_shot);
        }

        _shot = ShotCoroutine();

        StartCoroutine(_shot);
    }

    public override void StopShot()
    {
        if (_shot != null)
        {
            StopCoroutine(_shot);

            _shot = null;
        }

        FinishedShot();
    }

    IEnumerator ShotCoroutine()
    {
        if (bulletNum <= 0 || bulletSpeed <= 0f || wayNum <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or WayNum is not set.");
            yield break;
        }
        if (_shooting)
        {
            yield break;
        }
        _shooting = true;

        int wayIndex = 0;

        for (int i = 0; i < bulletNum; i++)
        {
            if (wayNum <= wayIndex)
            {
                wayIndex = 0;
                if (0f < nextLineDelay)
                {

                    yield return new WaitForSeconds(nextLineDelay);
                }
            }
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            float centerAngle = waveCenterAngle + (waveRangeSize / 2f * Mathf.Sin(TimeManager.Instance.frameCount * waveSpeed / 100f));

            float baseAngle = wayNum % 2 == 0 ? centerAngle - (betweenAngle / 2f) : centerAngle;

            float angle = Util.GetShiftedAngle(wayIndex, baseAngle, betweenAngle);

            while (GameTime.isPaused)
            {
                yield return null;
            }

            ShotBullet(bullet, bulletSpeed, angle);

            AudioManager.Instance.PlaySound(shotSFX, transform.position);

            AutoReleaseBulletGameObject(bullet.gameObject);

            wayIndex++;
        }

        FinishedShot();
    }
}