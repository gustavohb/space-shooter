using UnityEngine;

public class PlayerWeapon : BaseShot
{

    [SerializeField] private Transform[] _muzzles;

    [SerializeField] private float _burstDelay = 0f;

    [SerializeField] private int _burstShots = 1;

    [SerializeField] private float _shotRate = 0.5f;

    [SerializeField] private float betweenDelay = 0.1f;

    public string targetTag = "Enemy";

    [HideInInspector]
    public bool isShooting { get; private set; } = false;

    private Transform _currentTarget;

    private float _burstDelayCount = 0f;

    private int _burstShotsCount = 0;

    private float _shotRateCount = 0f;

    private float angle;

    public void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (_currentTarget == null)
        {
            return;
        }

        Vector3 newEulerAngles = transform.eulerAngles;

        newEulerAngles.z = Mathf.Atan2(_currentTarget.position.y - transform.position.y, _currentTarget.position.x - transform.position.x) * (180 / Mathf.PI);

        transform.eulerAngles = newEulerAngles;

        _burstDelayCount += GameTime.deltaTime;

        if (_burstDelayCount >= _burstDelay)
        {
            if (_burstShotsCount < _burstShots)
            {
                _shotRateCount += GameTime.deltaTime;

                if (_shotRateCount >= _shotRate)
                {
                    _burstShotsCount++;

                    foreach (Transform muzzleTransform in _muzzles)
                    {
                        Bullet bullet = GetBullet(muzzleTransform.position, muzzleTransform.rotation);
                        bullet.targetTag = targetTag;

                        angle = Util.GetAngleFromTwoPosition(transform, _currentTarget);

                        AudioManager.Instance.PlaySound(shotSFX, transform.position);

                        ShotBullet(bullet, bulletSpeed, angle);
                    }

                    _shotRateCount = 0;
                }
            }
            else
            {
                _burstShotsCount = 0;
                _burstDelayCount = 0;
            }
        }
    }

    public void StartShooting()
    {
        if (isShooting || _currentTarget == null)
        {
            return;
        }
        isShooting = true;
        _burstShotsCount = 0;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
    }

    public void OnDrawGizmos()
    {
        if (_muzzles == null || _muzzles.Length == 0)
        {
            return;
        }

        foreach (Transform muzzleTranform in _muzzles)
        {
            Gizmos.color = Color.red;

            Vector3 direction = muzzleTranform.TransformDirection(transform.right) * 5;

            Gizmos.DrawRay(muzzleTranform.position, direction);
        }
    }

    public override void Shot()
    {
        
    }

    public override void StopShot()
    {
        
    }
}

