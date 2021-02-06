using System;
using System.Collections;
using UnityEngine;
using ScriptableObjectArchitecture;

public class PlayerHealthShield : ExtendedCustomMonoBehavior, IDamageable
{
    [Serializable]
    public enum ShieldArmor
    {
        None,
        Tier_1,
        Tier_2,
        Tier_3,
        Tier_4
    }

    [Serializable]
    public enum HealthLevel
    {
        Level_1,
        Level_2,
        Level_3,
        Level_4
    }

    public const int HEALTH_AMOUNT_PER_SEGMENT = 25;
    public const int SHIELD_AMOUNT_PER_SEGMENT = 25;

    [SerializeField] private BoolGameEvent OnHealthShieldChanged;
    [SerializeField] private GameEvent OnRepair;
    [SerializeField] private GameEvent OnPlayerDie;

    public event EventHandler OnDeath;

    [SerializeField] float _shieldScaleFactor = 0.0f;
    [SerializeField] private GameObject _explosionEffect;
    private ShieldArmor _shieldArmor;
    private HealthLevel _healthLevel;

    [SerializeField] private FloatVariable _health;
    [SerializeField] private FloatVariable _shield;

    [SerializeField] protected Color _shieldColor = Color.magenta;

    private bool _isDead;

    // How many seconds to wait after being damaged before starting to recharge the shield
    [SerializeField] private float _rechargeShieldDelay = 7.0f;

    private float _rechargeShieldDelayCount;

    [SerializeField] private float _rechargeShieldSpeed = 2.0f;
    [SerializeField] private float _hitShakeDuration = 0.15f;
    [SerializeField] private float _hitShakeIntensity = 0.1f;

    private Transform _baseTransform;
    public SpriteRenderer spriteRenderer;
    private Material _tintMaterial;
    private bool _isFlashing = false;

    [SerializeField] private float _flashDelay = 0.08f;
    [SerializeField] private float _flashSpeed = 4.0f;

    private float _flashDelayTimer;
    private IEnumerator _doFlash;

    [SerializeField] private CircleCollider2D _shieldCollier;
    [SerializeField] private PolygonCollider2D _collider;

    private void Awake()
    {
        _baseTransform = transform.Find("Base");

        if (spriteRenderer != null)
        {
            _tintMaterial = spriteRenderer.material;
        }


        if (_shieldCollier != null && GetShield() > 0)
        {
            _shieldCollier.enabled = true;
        }

        if (_collider != null)
        {
            _collider.enabled = GetShield() <= 0;
        }

        _healthLevel = GameDataController.GetPlayerHealthLevel();
        _shieldArmor = GameDataController.GetPlayerShieldArmor();
        SetHealthLevel(_healthLevel);
        SetEquippedShieldArmor(_shieldArmor);
        
    }

    private void Start()
    {
        _rechargeShieldDelayCount = 0;

        OnHealthShieldChanged.Raise(false);
    }

    private void Update()
    {
        if (_rechargeShieldDelayCount < _rechargeShieldDelay)
        {
            _rechargeShieldDelayCount += GameTime.deltaTime;
        }
        else
        {
            HealShield(_rechargeShieldSpeed * GameTime.deltaTime);
        }
    }

    public void Damage(float damageAmount)
    {
        Damage(damageAmount, false);
    }

    public void Damage(float damageAmount, bool isMeleeAttack)
    {
        if (isMeleeAttack)
        {
            _health.Value -= damageAmount;

            Flash();

            StartCoroutine(Shake(_hitShakeDuration, _hitShakeIntensity));

        }
        else
        {
            if (damageAmount < _shield)
            {
                _shield.Value -= damageAmount;
            }
            else
            {
                if (_shieldCollier != null)
                {
                    _shieldCollier.enabled = false;
                }

                if (_collider != null)
                {
                    _collider.enabled = true;
                }

                _health.Value -= damageAmount - _shield;
                _shield.Value = 0;


                Flash();

                StartCoroutine(Shake(_hitShakeDuration, _hitShakeIntensity));
            }
        }

        // Reset the recharge delay for the shield
        _rechargeShieldDelayCount = 0;

        _health.Value = Mathf.Clamp(_health, 0, GetHealthMax());

        OnHealthShieldChanged.Raise(true);

        if (_health <= 0 && !_isDead)
        {
            Die();
        }

    }

    public Color GetShieldColor()
    {
        return _shieldColor;
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetShield()
    {
        return _shield;
    }


    private void Die()
    {
        _isDead = true;

        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, GetPosition(), Quaternion.identity);
        }

        OnPlayerDie?.Raise();

        OnDeath?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    public ShieldArmor GetEquippedShieldArmor()
    {
        return _shieldArmor;
    }
    public HealthLevel GetHealthLevel()
    {
        return _healthLevel;
    }

    public void SetEquippedShieldArmor(ShieldArmor shieldArmor)
    {
        this._shieldArmor = shieldArmor;

        switch (shieldArmor)
        {
            case ShieldArmor.None:
                _shield.Value = 0;
                break;
            case ShieldArmor.Tier_1:
                _shield.Value = SHIELD_AMOUNT_PER_SEGMENT;
                break;
            case ShieldArmor.Tier_2:
                _shield.Value = SHIELD_AMOUNT_PER_SEGMENT * 2;
                break;
            case ShieldArmor.Tier_3:
                _shield.Value = SHIELD_AMOUNT_PER_SEGMENT * 3;
                break;
            case ShieldArmor.Tier_4:
                _shield.Value = SHIELD_AMOUNT_PER_SEGMENT * 4;
                break;
        }
    }

    public void SetHealthLevel(HealthLevel healthLevel)
    {
        _healthLevel = healthLevel;

        switch (healthLevel)
        {
            case HealthLevel.Level_1:
                _health.Value = HEALTH_AMOUNT_PER_SEGMENT;
                break;
            case HealthLevel.Level_2:
                _health.Value = HEALTH_AMOUNT_PER_SEGMENT * 2;
                break;
            case HealthLevel.Level_3:
                _health.Value = HEALTH_AMOUNT_PER_SEGMENT * 3;
                break;
            case HealthLevel.Level_4:
                _health.Value = HEALTH_AMOUNT_PER_SEGMENT * 4;
                break;
        }
    }

    public float GetHealthMax()
    {
        switch (_healthLevel)
        {
            default:
            case HealthLevel.Level_1: return HEALTH_AMOUNT_PER_SEGMENT;
            case HealthLevel.Level_2: return HEALTH_AMOUNT_PER_SEGMENT * 2;
            case HealthLevel.Level_3: return HEALTH_AMOUNT_PER_SEGMENT * 3;
            case HealthLevel.Level_4: return HEALTH_AMOUNT_PER_SEGMENT * 4;
        }
    }

    public void HealHealth(float amount)
    {
        if (_health >= GetHealthMax())
        {
            return;
        }

        _health.Value += amount;
        _health.Value = Mathf.Clamp(_health, 0, GetHealthMax());
        OnHealthShieldChanged.Raise(false);
    }


    public float GetShieldhMax()
    {
        switch (_shieldArmor)
        {
            default:
            case ShieldArmor.None: return 0;
            case ShieldArmor.Tier_1: return SHIELD_AMOUNT_PER_SEGMENT;
            case ShieldArmor.Tier_2: return SHIELD_AMOUNT_PER_SEGMENT * 2;
            case ShieldArmor.Tier_3: return SHIELD_AMOUNT_PER_SEGMENT * 3;
            case ShieldArmor.Tier_4: return SHIELD_AMOUNT_PER_SEGMENT * 4;
        }
    }

    public void HealShield(float amount)
    {
        if (_shield >= GetShieldhMax())
        {
            return;
        }

        _shield.Value += amount;
        _shield.Value = Mathf.Clamp(_shield, 0, GetShieldhMax());


        if (_shieldCollier != null)
        {
            _shieldCollier.enabled = true;
        }

        if (_collider != null)
        {
            _collider.enabled = false;
        }

        OnHealthShieldChanged.Raise(false);
    }



    public bool HasShield()
    {
        return _shield > 0;
    }

    public bool HasShield(float damageAmount)
    {
        return damageAmount <= _shield;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = _baseTransform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration) // TODO: Consider if game is paused
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            _baseTransform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += GameTime.deltaTime;

            yield return null;
        }

        _baseTransform.localPosition = originalPos;
    }


    private void Flash()
    {
        if (_doFlash != null)
        {
            StopCoroutine(_doFlash);
        }

        _doFlash = DoFlashRoutine();

        StartCoroutine(_doFlash);
    }

    private IEnumerator DoFlashRoutine()
    {
        if (_tintMaterial == null)
        {
            Debug.LogWarning(gameObject.name + " not assigned tint material!");
            yield break;
        }

        _isFlashing = false;
        yield return new WaitForEndOfFrame();

        _isFlashing = true;

        _flashDelayTimer = _flashDelay;

        while (_isFlashing && _flashDelayTimer >= 0)
        {
            _flashDelayTimer -= GameTime.deltaTime;

            _tintMaterial.SetFloat("_FlashAmount", 1.0f);

            yield return null;
        }

        float flash = 1f;

        while (_isFlashing && flash >= 0)
        {
            flash -= GameTime.deltaTime * _flashSpeed;

            _tintMaterial.SetFloat("_FlashAmount", flash);

            yield return null;
        }

        _tintMaterial.SetFloat("_FlashAmount", 0.0f);

        _doFlash = null;

        _isFlashing = false;
    }

    public float GetShieldScaleFactor()
    {
        return _shieldScaleFactor;
    }


    public void Repair()
    {
        HealHealth(GetHealthMax());
        HealShield(GetShieldhMax());
        OnRepair.Raise();
    }
}
