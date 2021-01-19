using UnityEngine;
using System.Collections;

/// <summary>
/// Spiral multi shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Spiral Multi Shot")]
public class SpiralMultiShot : BaseShot
{
    // "Set a number of shot spiral way."
    public int spiralWayNum = 4;
    // "Set a starting angle of shot. (0 to 360)"
    [Range(0f, 360f)]
    public float startAngle = 180f;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f)]
    public float shiftAngle = 5f;
    // "Set a delay time between bullet and next bullet. (sec)"
    public float betweenDelay = 0.2f;


    private IEnumerator m_Shot;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        //Completed = false;

        if (m_Shot != null)
        {
            StopCoroutine(m_Shot);
            //FinishedShot();
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
        if (bulletNum <= 0 || bulletSpeed <= 0f || spiralWayNum <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or SpiralWayNum is not set.");
            yield break;
        }
        if (_shooting)
        {
            yield break;
        }
        _shooting = true;

        float spiralWayShiftAngle = 360f / spiralWayNum;

        int spiralWayIndex = 0;

        for (int i = 0; i < bulletNum; i++)
        {
            if (spiralWayNum <= spiralWayIndex)
            {
                spiralWayIndex = 0;
                if (0f < betweenDelay)
                {
                    AudioManager.Instance.PlaySound(shotSFX, transform.position);
                    yield return new WaitForSeconds(betweenDelay);
                }
            }

            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            float angle = startAngle + (spiralWayShiftAngle * spiralWayIndex) + (shiftAngle * Mathf.Floor(i / spiralWayNum));

            while (GameTime.isPaused)
            {
                yield return null;
            }

            ShotBullet(bullet, bulletSpeed, angle);

            bullet.targetTag = targetTagName;
            spiralWayIndex++;
        }

        FinishedShot();
    }
}