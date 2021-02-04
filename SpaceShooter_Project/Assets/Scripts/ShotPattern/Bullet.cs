using System;
using System.Collections;
using UnityEngine;

public class Bullet : ExtendedCustomMonoBehavior
{
    public static event EventHandler<OnBulletHitObjectEventArgs> OnBulletHitObject;

    public class OnBulletHitObjectEventArgs : EventArgs
    {
        public Color bulletColor;
        public Transform objectTransform;
        public bool hasShield;
        public float shieldScaleFactor = 0.0f;
        public Color shieldColor;
    }

    public string targetTag = "Player";

    public int damage = 3;

    public bool shooting
    {
        get;
        private set;
    }

    [ColorUsage(true, true)]
    private Color _color;

    public Color color => _color;

    private SpriteRenderer _glow;

    public void SetColor(Color color)
    {
        _color = color;
        _glow = transform.Find("bullet").Find("glow").GetComponent<SpriteRenderer>();
        if (_glow != null)
        {
            _glow.color = color;
        }
    }



    private void Awake()
    {
        _glow = transform.Find("bullet").Find("glow").GetComponent<SpriteRenderer>();
        _glow.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            IDamageable damageableObject = collision.GetComponent<IDamageable>();

            if (damageableObject != null)
            {
                OnBulletHitObject?.Invoke(this, new OnBulletHitObjectEventArgs
                {
                    bulletColor = color,
                    objectTransform = collision.transform,
                    hasShield = damageableObject.HasShield(damage),
                    shieldScaleFactor = damageableObject.GetShieldScaleFactor(),
                    shieldColor = damageableObject.GetShieldColor()
                });
                damageableObject.Damage(damage);
            }
            ObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }


    public void Shot(float speed, float angle, float accelSpeed, float accelTurn,
                      bool homing, Transform homingTarget, float homingAngleSpeed,
                      bool wave, float waveSpeed, float waveRangeSize)
    {
        if (shooting)
        {
            return;
        }
        shooting = true;

        StartCoroutine(MoveCoroutine(speed, angle, accelSpeed, accelTurn,
                                     homing, homingTarget, homingAngleSpeed,
                                     wave, waveSpeed, waveRangeSize));
    }

    IEnumerator MoveCoroutine(float speed, float angle, float accelSpeed, float accelTurn,
                               bool homing, Transform homingTarget, float homingAngleSpeed,
                               bool wave, float waveSpeed, float waveRangeSize)
    {
        transform.SetEulerAnglesZ(angle);

        float selfFrameCnt = 0f;

        while (true)
        {
            if (homing)
            {
                // homing target.
                if (homingTarget != null && 0f < homingAngleSpeed)
                {
                    float rotAngle = Util.GetAngleFromTwoPosition(transform, homingTarget);
                    float myAngle = 0f;

                    myAngle = transform.eulerAngles.z;

                    float toAngle = Mathf.MoveTowardsAngle(myAngle, rotAngle, GameTime.deltaTime * homingAngleSpeed);

                    transform.SetEulerAnglesZ(toAngle);
                }

            }
            else if (wave)
            {
                // acceleration turning.
                angle += (accelTurn * GameTime.deltaTime);
                // wave.
                if (0f < waveSpeed && 0f < waveRangeSize)
                {
                    float waveAngle = angle + (waveRangeSize / 2f * Mathf.Sin(selfFrameCnt * waveSpeed / 100f));

                    transform.SetEulerAnglesZ(waveAngle);
                }
                selfFrameCnt++;

            }
            else
            {
                // acceleration turning.
                float addAngle = accelTurn * GameTime.deltaTime;

                transform.AddEulerAnglesZ(addAngle);
            }

            // acceleration speed.
            speed += (accelSpeed * GameTime.deltaTime);


            transform.position += transform.up.normalized * speed * GameTime.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    public void SetTargetTag(string targetTag)
    {
        this.targetTag = targetTag;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        transform.ResetPosition();
        transform.ResetRotation();
        shooting = false;
    }

}
