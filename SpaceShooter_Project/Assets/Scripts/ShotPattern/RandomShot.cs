using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Random shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Random Shot")]
public class RandomShot : BaseShot
{
    // "Center angle of random range."
    [Range(0f, 360f)]
    public float randomCenterAngle = 180f;
    // "Set a angle size of random range. (0 to 360)"
    [Range(0f, 360f)]
    public float randomRangeSize = 360f;
    // "Set a minimum bullet speed of shot."
    // "BulletSpeed is ignored."
    public float randomSpeedMin = 1f;
    // "Set a maximum bullet speed of shot."
    // "BulletSpeed is ignored."
    public float randomSpeedMax = 3f;
    // "Set a minimum delay time between bullet and next bullet. (sec)"
    public float randomDelayMin = 0.01f;
    // "Set a maximum delay time between bullet and next bullet. (sec)"
    public float randomDelayMax = 0.1f;
    // "Evenly distribute of all bullet angle."
    public bool evenlyDistribute = true;

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
        if (bulletNum <= 0 || randomSpeedMin <= 0f || randomSpeedMax <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or RandomSpeedMin or RandomSpeedMax is not set.");
            yield break;
        }
        if (_shooting)
        {
            yield break;
        }
        _shooting = true;

        List<int> numList = new List<int>();

        for (int i = 0; i < bulletNum; i++)
        {
            numList.Add(i);
        }

        while (0 < numList.Count)
        {
            int index = Random.Range(0, numList.Count);
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            while (GameTime.isPaused)
            {
                yield return null;
            }

            float bulletSpeed = Random.Range(randomSpeedMin, randomSpeedMax);

            float minAngle = randomCenterAngle - (randomRangeSize / 2f);
            float maxAngle = randomCenterAngle + (randomRangeSize / 2f);
            float angle = 0f;

            if (evenlyDistribute)
            {
                float oneDirectionNum = Mathf.Floor((float)bulletNum / 4f);
                float quarterIndex = Mathf.Floor((float)numList[index] / oneDirectionNum);
                float quarterAngle = Mathf.Abs(maxAngle - minAngle) / 4f;
                angle = Random.Range(minAngle + (quarterAngle * quarterIndex), minAngle + (quarterAngle * (quarterIndex + 1f)));

            }
            else
            {
                angle = Random.Range(minAngle, maxAngle);
            }

            AudioManager.Instance.PlaySound(shotSFX, transform.position);
            ShotBullet(bullet, bulletSpeed, angle);

            numList.RemoveAt(index);

            if (0 < numList.Count && 0f <= randomDelayMin && 0f < randomDelayMax)
            {
                float waitTime = Random.Range(randomDelayMin, randomDelayMax);
                yield return new WaitForSeconds(waitTime);
            }
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