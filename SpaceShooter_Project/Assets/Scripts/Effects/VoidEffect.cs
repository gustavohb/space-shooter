using UnityEngine;

public class VoidEffect : MonoBehaviour
{
    [SerializeField] private float _attractionRange = 7.56f;
    [SerializeField] private float _attractionSpeed = 14f;
    [SerializeField] private string _attractionTag = "Enemy";
    [SerializeField] private float _colliderEnableDelay = 0.5f;
    [SerializeField] private float _attractionDisableTime = 4.5f;
    [SerializeField] private float _lifeTime = 5.5f;
    [SerializeField] private SoundLibrary.Sound _sfx = SoundLibrary.Sound.None;

    private float _voidDisableTimer;
    private CircleCollider2D _collider;
    private float _lifeTimer;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = _attractionRange;
        _voidDisableTimer = 0;
        _lifeTimer = _lifeTime;
        Invoke("EnableCollider", _colliderEnableDelay);
        AudioManager.Instance.PlaySound2D(_sfx);
    }


    private void Update()
    {
        _voidDisableTimer += Time.deltaTime;
        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Bullet")
        {
            ObjectPool.Instance.ReleaseGameObject(collision.gameObject);
        }
        else if (collision.tag == _attractionTag)
        {
            IMoveVelocity moveVelocity = collision.GetComponent<IMoveVelocity>();
            if (moveVelocity != null)
            {
                moveVelocity.DisableMovement(_lifeTimer);
            }

            MeleeAttack meleeAttack = collision.GetComponent<MeleeAttack>();
            if (meleeAttack != null)
            {
                meleeAttack.DisableMeleeAttack(_lifeTimer);
            }
        }

    }

    void EnableCollider()
    {
        _collider.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject attractive = collision.gameObject;
        if (attractive.tag == _attractionTag)
        {

            if (_voidDisableTimer < _lifeTime)
            {
                float rotationTarget = Mathf.Atan2(transform.position.y - attractive.transform.position.y, transform.position.x - attractive.transform.position.x) * (180 / Mathf.PI);
                Vector3 newPosition = attractive.transform.position;

                newPosition.x += Mathf.Cos(rotationTarget * Mathf.PI / 180) * _attractionSpeed * Time.deltaTime;
                newPosition.y += Mathf.Sin(rotationTarget * Mathf.PI / 180) * _attractionSpeed * Time.deltaTime;

                attractive.transform.position = newPosition;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _attractionRange);
    }
}
