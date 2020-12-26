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
        GameObject shotHitEffectGO;
        GameObject forceFieldShieldGO;

        // Fix bug when colliding with more than one collider at the same time
        if (bullet.transform.position == Vector3.zero)
        {
            return;
        }

        if (e.hasShield)
        {
            Vector3 direction = bullet.transform.position - e.objectTransform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            shotHitEffectGO = ObjectPool.Instance.GetGameObject(GameAssets.Instance.shieldHitEffectPrefab, bullet.transform.position, bullet.transform.rotation); 
            forceFieldShieldGO = ObjectPool.Instance.GetGameObject(GameAssets.Instance.shieldEffectPrefab, e.objectTransform.position, Quaternion.identity); 
            forceFieldShieldGO.transform.parent = e.objectTransform;
            forceFieldShieldGO.transform.eulerAngles = new Vector3(0, 0, angle);
            forceFieldShieldGO.transform.localScale = new Vector3(e.shieldScaleFactor, e.shieldScaleFactor, e.shieldScaleFactor);
            IShotEffect forceFieldShieldEffect = forceFieldShieldGO.GetComponent<IShotEffect>();
            forceFieldShieldEffect.Setup(e.shieldColor);
        }
        else
        {
            shotHitEffectGO = ObjectPool.Instance.GetGameObject(GameAssets.Instance.shotHitEffectPrefab, bullet.transform.position, bullet.transform.rotation);  
        }

        IShotEffect shotHitEffect = shotHitEffectGO.GetComponent<IShotEffect>();
        shotHitEffect.Setup(e.bulletColor);
    }


}

