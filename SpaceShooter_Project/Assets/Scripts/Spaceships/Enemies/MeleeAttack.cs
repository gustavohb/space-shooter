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

    private float _meleeAttackDelayTimer;

    private float _nextAttackTime;

    private IMoveVelocity _moveVelocity;

    private Transform _target;

    private bool _isAttacking = false;

    private bool _isMeleeAttackEnable = true;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(_targetTag)?.transform;

        _moveVelocity = GetComponent<IMoveVelocity>();

    }

    private void Update()
    {

        _meleeAttackDelayTimer -= Time.deltaTime;

        if (_target != null && Time.time > _nextAttackTime && _isMeleeAttackEnable)
        {
            float sqrDistToTarget = (_target.position - transform.position).sqrMagnitude;

            if (sqrDistToTarget < Mathf.Pow(_attackDistanceThreshold, 2))
            {
                _nextAttackTime = Time.time + _timeBetweenAttacks;
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

        while (percent <= 1)
        {
            if (_target == null)
            {
                break;
            }
            percent += Time.deltaTime * _attackSpeed;

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
