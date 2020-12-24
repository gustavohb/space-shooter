using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IMoveVelocity))]
public class MeleeAttack : ExtendedCustomMonoBehavior
{

    [SerializeField] private float _attackDamage = 5f;

    [SerializeField] private float _meleeAttackDelay = 1.0f;

    [SerializeField] private string _targetTag = "Player";

    [SerializeField] private float _attackSpeed = 2.5f;

    [SerializeField] private float _attackDistanceThreshold = 2.2f;

    [SerializeField] private float _timeBetweenAttacks = 1f;

    [SerializeField] private SoundLibrary.Sound _attackSFX;

    private float _meleeAttackDelayTimer;

    private float _meleeAttackTimer;

    private Transform _target;

    private bool _isAttacking = false;

    private bool _isMeleeAttackEnable = true;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(_targetTag)?.transform;
    }

    private void Update()
    {

        _meleeAttackDelayTimer -= GameTime.deltaTime;

        _meleeAttackTimer -= GameTime.deltaTime;

        if (_target != null && _meleeAttackTimer <= 0 && _isMeleeAttackEnable)
        {
            float sqrDistToTarget = (_target.position - transform.position).sqrMagnitude;

            if (sqrDistToTarget < Mathf.Pow(_attackDistanceThreshold, 2))
            {
                _meleeAttackTimer = _timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }


    private IEnumerator Attack()
    {
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = _target.position;

        float percent = 0f;

        _isAttacking = true;

        AudioManager.Instance.PlaySound(_attackSFX, transform.position);

        while (percent <= 1)
        {
            if (_target == null)
            {
                break;
            }
            percent += GameTime.deltaTime * _attackSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            if (Vector3.Distance(transform.position, _target.position) <= 0.3 && _meleeAttackDelayTimer <= 0)
            {
                _meleeAttackDelayTimer = _meleeAttackDelay;

                IDamageable damageable = _target.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(_attackDamage, true);
                }
            }

            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        _isAttacking = false;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == _targetTag && _isMeleeAttackEnable && _isAttacking && _meleeAttackDelayTimer <= 0)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                _meleeAttackDelayTimer = _meleeAttackDelay;

                damageable.Damage(_attackDamage, true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _targetTag && _isMeleeAttackEnable && _isAttacking && _meleeAttackDelayTimer <= 0)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                _meleeAttackDelayTimer = _meleeAttackDelay;

                damageable.Damage(_attackDamage, true);
            }
        }
    }

    public void DisableMeleeAttack(float time)
    {
        _isMeleeAttackEnable = false;
        Invoke("EnableMeleeAttack", time);
    }
}
