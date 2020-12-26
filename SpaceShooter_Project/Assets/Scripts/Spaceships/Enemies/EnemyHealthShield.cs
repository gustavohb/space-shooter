using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyHealthShield : ExtendedCustomMonoBehavior, IDamageable
{
    public event EventHandler OnDamaged;

    [Header("Health")]

    [SerializeField] private int _startHealth = 100;

    [SerializeField] private float _maxHealth = 100;

    [SerializeField] private PolygonCollider2D _collider;

    public float maxHealth => _maxHealth;

    public float currentHealth { get; private set; }

    public bool isAlive { get; private set; }

    [Header("Shield")]

    [SerializeField] private float _startShield = 0;

    [SerializeField] private float _maxShield = 100;

    public float maxShield => _maxShield;

    public float currentShield { get; private set; }

    [SerializeField] private Color _shieldColor = Color.magenta;

    [SerializeField, Range(0, 1)] private float _lowHealthPct = 0.20f;

    [SerializeField] private GameObject _lowHealthEffect;

    [SerializeField] private CircleCollider2D _shieldCollider;

    [SerializeField] float _shieldScaleFactor = 1.0f;

    [Header("Death")]

    [SerializeField] private GameObject _explosionEffectPrefab;

    public static event Action<EnemyHealthShield> OnHealthShieldAdded = delegate { };

    public static event Action<EnemyHealthShield> OnHealthShieldRemoved = delegate { };

    public event Action<float> OnHealthPctChanged;

    public event Action<float> OnShieldPctChanged;

    public event EventHandler OnDeath;

    [Header("Damage Settings")]

    [SerializeField] private float _hitShakeDuration = 0.25f;

    [SerializeField] private float _hitShakeIntensity = 0.3f;

    [SerializeField] private float _flashSpeed = 4.0f;

    [SerializeField] private float _flashDelay = 0.08f;

    private float _flashDelayTimer = 0.0f;

    private Material _material;

    private bool _isFlashing = false;

    private Transform _baseTransform;

    [SerializeField] private SpriteRenderer[] _spriteRenderers;

    private Material[] _tintMaterials;

    private IEnumerator _flash;

    private bool _isShaking = false;

    [SerializeField] private Transform _lowHealthEffectTransform;

    private void Awake()
    {
        currentHealth = _startHealth;
        currentShield = _startShield;
        isAlive = true;

        if (_lowHealthEffectTransform != null && _lowHealthEffectTransform.gameObject.activeSelf)
        {
            _lowHealthEffectTransform.gameObject.SetActive(false);
        }

        OnHealthShieldAdded(this);

        if (currentShield > 0 && _collider != null && _shieldCollider != null)
        {
            _collider.enabled = false;
            _shieldCollider.enabled = true;

        }

        if (_spriteRenderers != null)
        {
            int numSpriteRenderers = _spriteRenderers.Length;
            _tintMaterials = new Material[numSpriteRenderers];
            for (int i = 0; i < numSpriteRenderers; i++)
            {
                _tintMaterials[i] = _spriteRenderers[i].material;
            }
        }

    }

    private void Start()
    {
        _baseTransform = transform.Find("Base");

        if (_lowHealthEffect != null)
        {
            GameObject newLowHealthEffect = Instantiate(_lowHealthEffect, transform.position, Quaternion.identity);

            _lowHealthEffect = newLowHealthEffect;
            _lowHealthEffect.transform.parent = transform;
            _lowHealthEffect.gameObject.SetActive(false);
        }

    }

    private void ModifyHealth(float amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);

        float currentHealthPct = GetHealthPct();

        OnHealthPctChanged?.Invoke(currentHealthPct);
    }

    private void Modifyshield(float amount)
    {
        currentShield += amount;

        currentShield = Mathf.Clamp(currentShield, 0, _maxShield);

        float currentShieldPct = GetShieldPct();

        OnShieldPctChanged?.Invoke(currentShieldPct);
    }

    public bool HasShield()
    {
        return currentShield > 0;
    }

    public bool HasShield(float damageAmount)
    {
        return damageAmount <= currentShield;
    }

    public Color GetShieldColor()
    {
        return _shieldColor;
    }

    public void Damage(float damageAmount, bool isMeleeAttack = false)
    {
        if (isMeleeAttack)
        {
            ModifyHealth(-damageAmount);

            if (!_isShaking)
            {
                _baseTransform.DOShakePosition(_hitShakeDuration, _hitShakeIntensity)
                .OnStart(() =>
                {
                    _isShaking = true;
                }
                )
                .OnComplete(() =>
                {
                    _baseTransform.localPosition = Vector3.zero;
                    _isShaking = false;
                });
            }

            Flash();
        }
        else if (damageAmount > currentShield)
        {
            ModifyHealth(-(damageAmount - currentShield));
            Modifyshield(-damageAmount);

            if (!_isShaking)
            {
                _baseTransform.DOShakePosition(_hitShakeDuration, _hitShakeIntensity)
                .OnStart(() =>
                {
                    _isShaking = true;
                }
                )
                .OnComplete(() =>
                {
                    _baseTransform.localPosition = Vector3.zero;
                    _isShaking = false;
                });
            }


            if (_shieldCollider != null)
            {
                _shieldCollider.enabled = false;
            }


            if (_collider != null)
            {
                _collider.enabled = true;
            }

            Flash();

        }
        else
        {
            Modifyshield(-damageAmount);
        }

        if (_lowHealthEffectTransform != null)
        {
            _lowHealthEffectTransform.gameObject.SetActive(GetHealthPct() <= _lowHealthPct);
        }
        else if (_lowHealthEffect != null)
        {
            _lowHealthEffect.gameObject.SetActive(GetHealthPct() <= _lowHealthPct);
        }



        if (currentHealth <= 0)
        {
            Die();
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthPct()
    {
        return currentHealth / _maxHealth;
    }

    public float GetShieldPct()
    {
        return currentShield / _maxShield;
    }

    private void OnDisable()
    {
        OnHealthShieldRemoved(this);
    }

    public void KillEnemy()
    {
        if (isAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isAlive)
        {
            isAlive = false;

            OnDeath?.Invoke(this, EventArgs.Empty);

            if (_explosionEffectPrefab != null)
            {
                ObjectPool.Instance.GetGameObject(_explosionEffectPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator Shake(float duration, float magnitude)
    {


        Vector3 originalPos = _baseTransform.localPosition;


        float elapsed = 0.0f;

        while (elapsed < duration) //TODO: Check if game is not paused
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
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        if (_tintMaterials == null || _tintMaterials.Length == 0)
        {
            yield break;
        }

        if (_isFlashing)
        {
            yield break;
        }

        _isFlashing = true;

        _flashDelayTimer = _flashDelay;


        while (_isFlashing && _flashDelayTimer >= 0)
        {
            _flashDelayTimer -= GameTime.deltaTime;


            foreach (Material material in _tintMaterials)
            {
                material.SetFloat("_FlashAmount", 1.0f);
            }

            yield return null;
        }

        float flash = 1f;

        while (_isFlashing && flash >= 0)
        {
            flash -= GameTime.deltaTime * _flashSpeed;
 
            foreach (Material material in _tintMaterials)
            {
                material.SetFloat("_FlashAmount", flash);
            }

            yield return null;
        }

        foreach (Material material in _tintMaterials)
        {
            material.SetFloat("_FlashAmount", 0.0f);
        }

        _isFlashing = false;
    }

    public float GetShieldScaleFactor()
    {
        return _shieldScaleFactor;
    }

}