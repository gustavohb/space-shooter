using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShot : ExtendedCustomMonoBehavior
{
    public GameObject bulletPrefab;
    public int bulletNum = 10;
    public float bulletSpeed = 8f;
    public float accelerationSpeed = 0f;
    public float accelerationTurn = 0f;
    public bool initialPooling = false;

    public bool setTargetFromTag = true;
    public string targetTagName = "Player";

    public SoundLibrary.Sound shotSFX = SoundLibrary.Sound.Shot;

    public Transform targetTransform;

    public bool aiming;

    public Color bulletColor = Color.magenta;

    protected ShotController _shotController
    {
        get
        {
            if (_shotCtrl == null)
            {
                _shotCtrl = transform.GetComponentInParent<ShotController>();
            }
            return _shotCtrl;
        }
    }
    ShotController _shotCtrl;

    protected bool _shooting;

    protected IEnumerator _shot;

    protected virtual void Awake()
    {
        if (initialPooling)
        {
            var goBulletList = new List<GameObject>();
            for (int i = 0; i < bulletNum; i++)
            {
                var bullet = GetBullet(Vector3.zero, Quaternion.identity, true);
                if (bullet != null)
                {
                    goBulletList.Add(bullet.gameObject);
                }
            }
            for (int i = 0; i < goBulletList.Count; i++)
            {
                ObjectPool.Instance.ReleaseGameObject(goBulletList[i]);
            }
        }
    }

    protected virtual void OnDisable()
    {
        _shooting = false;
    }

    public abstract void Shot();

    public abstract void StopShot();


    public void SetShotCtrl(ShotController shotCtrl)
    {
        _shotCtrl = shotCtrl;
    }

    protected void FinishedShot()
    { 
        if (_shot != null)
        {
            StopCoroutine(_shot);
            _shot = null;
        }

        _shooting = false;
    }

    protected Bullet GetBullet(Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Cannot generate a bullet because BulletPrefab is not set.");
            return null;
        }

        var goBullet = ObjectPool.Instance.GetGameObject(bulletPrefab, position, rotation, forceInstantiate);
        if (goBullet == null)
        {
            return null;
        }
        var bullet = goBullet.GetComponent<Bullet>();
        if (bullet == null)
        {
            bullet = goBullet.AddComponent<Bullet>();
        }

        bullet.SetColor(bulletColor);
        bullet.SetTargetTag(targetTagName);
        return bullet;
    }

    protected void ShotBullet(Bullet bullet, float speed, float angle,
                               bool homing = false, Transform homingTarget = null, float homingAngleSpeed = 0f,
                               bool wave = false, float waveSpeed = 0f, float waveRangeSize = 0f)
    {
        if (bullet == null)
        {
            return;
        }
        bullet.Shot(speed, angle, accelerationSpeed, accelerationTurn,
                    homing, homingTarget, homingAngleSpeed,
                    wave, waveSpeed, waveRangeSize);
    }

}
