using UnityEngine;

public static class BulletForParticles
{
    public static void Init()
    {
        Bullet.OnBulletHitObject += Bullet_OnBulletHitObject;
    }

    private static void Bullet_OnBulletHitObject(object sender, Bullet.OnBulletHitObjectEventArgs e)
    {
        Bullet bullet = sender as Bullet;
        Transform shotHitEffectTransform;
        Transform forceFieldShieldTransform;

        // Fix bug when colliding with more than one collider at the same time
        if (bullet.transform.position == Vector3.zero)
        {
            return;
        }

        if (e.hasShield)
        {
            Vector3 direction = bullet.transform.position - e.objectTransform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            shotHitEffectTransform = Object.Instantiate(GameAssets.Instance.shieldHitEffectPrefab, bullet.transform.position, bullet.transform.rotation);
            forceFieldShieldTransform = Object.Instantiate(GameAssets.Instance.shieldEffectPrefab, e.objectTransform.position, Quaternion.identity);
            forceFieldShieldTransform.transform.parent = e.objectTransform;
            forceFieldShieldTransform.eulerAngles = new Vector3(0, 0, angle);
            forceFieldShieldTransform.localScale = new Vector3(e.shieldScaleFactor, e.shieldScaleFactor, e.shieldScaleFactor);
            IShotEffect forceFieldShieldEffect = forceFieldShieldTransform.GetComponent<IShotEffect>();
            forceFieldShieldEffect.Setup(e.shieldColor);
        }
        else
        {
            shotHitEffectTransform = Object.Instantiate(GameAssets.Instance.shotHitEffectPrefab, bullet.transform.position, bullet.transform.rotation);
        }

        IShotEffect shotHitEffect = shotHitEffectTransform.GetComponent<IShotEffect>();
        shotHitEffect.Setup(e.bulletColor);
    }


}

