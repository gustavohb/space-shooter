using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] private float _radius = 8.5f;

    [SerializeField] private float _force = 2000.0f;

    [SerializeField] private float _damage = 200.0f;

    private void Start()
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (Collider2D nearbyObject in collider2Ds)
        {
            if (nearbyObject.tag == "Enemy")
            {

                IMoveVelocity moveVelocity = nearbyObject.GetComponent<IMoveVelocity>();
                if (moveVelocity != null)
                {
                    moveVelocity.DisableMovement(0.5f);
                }

                Rigidbody2D rb2D = nearbyObject.GetComponent<Rigidbody2D>();
                if (rb2D != null)
                {
                    rb2D.AddExplosionForce(_force, transform.position, _radius);
                }

                IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(_damage, true);
                }
            }

            Bullet bullet = nearbyObject.GetComponent<Bullet>();

            if (bullet != null)
            {
                ObjectPool.Instance.ReleaseGameObject(bullet.gameObject);
            }

        }

    }

}
