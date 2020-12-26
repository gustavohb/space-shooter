using UnityEngine;
using System.Collections;

/// <summary>
/// Sin wave bullet nway shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Sin Wave Bullet nWay Shot")]
public class SinWaveBulletNwayShot : BaseShot
{
    // "Set a number of shot way."
    public int wayNum = 4;
    // "Set a center angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float centerAngle = 180f;
    // "Set a size of wave range. (0 to 360)"
    [Range(0f, 360f)]
    public float waveRangeSize = 40f;
    // "Set a speed of wave. (0 to 30)"
    [Range(0f, 30f)]
    public float waveSpeed = 10f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float betweenAngle = 10f;
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
            while (GameTime.isPaused)
            {
                yield return null;
            }

            if (wayNum <= wayIndex)
            {
                wayIndex = 0;

                if (0f < nextLineDelay)
                {
                    AudioManager.Instance.PlaySound(shotSFX, transform.position);
                    yield return new WaitForSeconds(nextLineDelay);
                }
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            float baseAngle = wayNum % 2 == 0 ? centerAngle - (betweenAngle / 2f) : centerAngle;

            float angle = Util.GetShiftedAngle(wayIndex, baseAngle, betweenAngle);

            ShotBullet(bullet, bulletSpeed, angle, false, null, 0f, true, waveSpeed, waveRangeSize);

            wayIndex++;
        }

        FinishedShot();
    }

    public override void StopShot()
    {
        if (_shot != null)
        {
            StopCoroutine(_shot);
            _shot = null;
        }
    }
}