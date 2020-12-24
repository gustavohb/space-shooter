using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class EnemyHealthShieldBars : MonoBehaviour
{
    [SerializeField] private Image _shieldBarImage;

    [SerializeField] private Image _shieldBackgroundImage;

    [SerializeField] private Image _healthBarImage;

    [SerializeField] private float _displayTime = 4f;

    private float _displayTimer;

    [SerializeField] private float _updateSpeedSeconds = 0.5f;

    [SerializeField] private float _positionOffset = 1.5f;

    private CanvasGroup _canvasGroup;

    private bool _isFadingOut = false;

    private EnemyHealthShield _healthShield;

    private IEnumerator _changeShieldToPct;
    private IEnumerator _changeHealthToPct;
    private IEnumerator _fadeCanvasGroup;

    private Camera _targetCamera;

    private void Awake()
    {
        _targetCamera = Camera.main;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetHealthShield(EnemyHealthShield healthShield)
    {
        _healthShield = healthShield;
        _healthShield.OnHealthPctChanged += HandleHealthChanged;
        _healthShield.OnShieldPctChanged += HandleShieldChanged;

        _healthBarImage.fillAmount = healthShield.GetHealthPct();
        _shieldBarImage.fillAmount = healthShield.GetShieldPct();

        if (!healthShield.HasShield())
        {
            _shieldBackgroundImage.gameObject.SetActive(false);
        }
    }

    private void HandleShieldChanged(float pct)
    {
        if (_changeShieldToPct != null)
        {
            StopCoroutine(_changeShieldToPct);
        }

        if (_healthShield != null)
        {
            _changeShieldToPct = ChangeShieldToPctRoutine(pct);
            StartCoroutine(_changeShieldToPct);
        }
    }

    private void HandleHealthChanged(float pct)
    {
        if (_changeHealthToPct != null)
        {
            StopCoroutine(_changeHealthToPct);
        }

        if (_healthShield != null)
        {
            _changeHealthToPct = ChangeHealthToPctRoutine(pct);

            StartCoroutine(_changeHealthToPct);
        }
    }

    private IEnumerator ChangeShieldToPctRoutine(float pct)
    {
        float preChangePct = _shieldBarImage.fillAmount;
        float elapsed = 0f;
        _displayTimer = 0f;

        if (_fadeCanvasGroup != null)
        {
            StopCoroutine(_fadeCanvasGroup);
        }

        _fadeCanvasGroup = FadeCanvasGroupRoutine(_canvasGroup, _canvasGroup.alpha, 1);

        StartCoroutine(_fadeCanvasGroup);

        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += GameTime.deltaTime;
            _shieldBarImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / _updateSpeedSeconds);
            yield return null;
        }

        _shieldBarImage.fillAmount = pct;

        if (_shieldBarImage != null)
        {
            if (_shieldBarImage.fillAmount <= 0 && _shieldBackgroundImage.gameObject.activeSelf)
            {
                _shieldBackgroundImage.gameObject.SetActive(false);
            }
            else if (_shieldBarImage.fillAmount > 0 && !_shieldBackgroundImage.gameObject.activeSelf)
            {
                _shieldBackgroundImage.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator ChangeHealthToPctRoutine(float pct)
    {
        float preChangePct = _healthBarImage.fillAmount;
        float elapsed = 0f;
        _displayTimer = 0f;

        if (_fadeCanvasGroup != null)
        {
            StopCoroutine(_fadeCanvasGroup);
        }

        _fadeCanvasGroup = FadeCanvasGroupRoutine(_canvasGroup, _canvasGroup.alpha, 1);

        StartCoroutine(_fadeCanvasGroup);


        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += GameTime.deltaTime;
            _healthBarImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / _updateSpeedSeconds);
            yield return null;
        }

        _healthBarImage.fillAmount = pct;
    }

    private void Update()
    {
        _displayTimer += GameTime.deltaTime;
        if (_displayTimer >= _displayTime && !_isFadingOut)
        {
            _isFadingOut = true;

            if (_fadeCanvasGroup != null)
            {
                StopCoroutine(_fadeCanvasGroup);
            }

            _fadeCanvasGroup = (FadeCanvasGroupRoutine(_canvasGroup, _canvasGroup.alpha, 0));

            StartCoroutine(_fadeCanvasGroup);
        }
    }

    private void LateUpdate()
    {
        if (_targetCamera != null && _healthShield != null)
        {
            transform.position = _targetCamera.WorldToScreenPoint(_healthShield.transform.position + Vector3.up * _positionOffset);
        }
    }

    private void OnDestroy()
    {
        if (_changeShieldToPct != null)
        {
            StopCoroutine(_changeShieldToPct);
            _changeShieldToPct = null;
        }

        if (_changeHealthToPct != null)
        {
            StopCoroutine(_changeHealthToPct);
            _changeHealthToPct = null;
        }

        if (_fadeCanvasGroup != null)
        {
            StopCoroutine(_fadeCanvasGroup);
            _fadeCanvasGroup = null;
        }

        StopAllCoroutines();

        _healthShield.OnHealthPctChanged -= HandleHealthChanged;
        _healthShield.OnShieldPctChanged -= HandleShieldChanged;
    }



    public IEnumerator FadeCanvasGroupRoutine(CanvasGroup canvasGroup, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartLerping = Time.time;
        float timeSinceStarted;
        float percentageComplete;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            canvasGroup.alpha = currentValue;

            if (percentageComplete >= 1)
            {
                _isFadingOut = false;

                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

}

