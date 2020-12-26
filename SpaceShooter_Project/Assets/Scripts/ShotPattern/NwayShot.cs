using UnityEngine;
using System.Collections;

/// <summary>
/// nWay shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/nWay Shot")]
public class NwayShot : BaseShot
{
    // "Set a number of shot way."
    public int wayNum = 5;
    // "Set a center angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float centerAngle = 180f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float betweenAngle = 10f;
    // "Set a delay time between shot and next line shot. (sec)"
    public float nextLineDelay = 0.1f;


    private IEnumerator m_Shot;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        if (m_Shot != null)
        {
            StopCoroutine(m_Shot);
        }

        m_Shot = ShotCoroutine();

        StartCoroutine(m_Shot);
    }

    public override void StopShot()
    {
        if (m_Shot != null)
        {
            StopCoroutine(m_Shot);

            m_Shot = null;
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
                    AudioManager.Instance.PlaySound(shotSFX, transform.position);
                    yield return new WaitForSeconds(nextLineDelay);
                }
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            bullet.targetTag = targetTagName;

            float baseAngle = wayNum % 2 == 0 ? centerAngle - (betweenAngle / 2f) : centerAngle;

            float angle = Util.GetShiftedAngle(wayIndex, baseAngle, betweenAngle);

            ShotBullet(bullet, bulletSpeed, angle);

            wayIndex++;
        }

        FinishedShot();
    }
}