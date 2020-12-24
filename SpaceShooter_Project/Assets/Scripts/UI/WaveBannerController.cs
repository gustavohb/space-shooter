using System.Collections;
using UnityEngine;
using TMPro;

public class WaveBannerController : MonoBehaviour
{

    [Header("References")]

    [SerializeField] private TextMeshProUGUI _waveTitle;

    [SerializeField] private EnemySpawner _enemySpawner;

    [SerializeField] private RectTransform _newWaveBannerRect;

    [SerializeField] private CanvasGroup _newWaveBannerCanvasGroup;

    [Header("SFX")]

    [SerializeField] private SoundLibrary.Sound _newWaveBannerAppearSfx = SoundLibrary.Sound.WaveBannerAppear;

    [SerializeField] private SoundLibrary.Sound _newWaveBannerDisappearSfx = SoundLibrary.Sound.WaveBannerDisappear;

    [Header("Settings")]

    [SerializeField] private float _newWaveBannerDelayTime = 2.0f;

    [SerializeField] private float _newWaveBannerSpeed = 3.0f;

    [SerializeField] private float _newWaveBannerStartYPos = -613;

    [SerializeField] private float _newWaveBannerEndYPos = 0;

    [SerializeField] private float _newWaveBannerStartDelay = 2.0f;

    private IEnumerator _animateNewWaveBanner;

    private void Awake()
    {
        if (_enemySpawner == null)
        {
            _enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        if (_enemySpawner)
        {
            _enemySpawner.OnNewWave += OnNewWave;
        }

        if (_newWaveBannerRect == null)
        {
            _newWaveBannerRect = GetComponent<RectTransform>();
        }

        if (_newWaveBannerCanvasGroup == null)
        {
            _newWaveBannerCanvasGroup = GetComponent<CanvasGroup>();
            if (_newWaveBannerCanvasGroup != null)
            {
                _newWaveBannerCanvasGroup.alpha = 0.0f;
            }
        }
    }

    private IEnumerator AnimateNewWaveBannerRoutine()
    {
        yield return new WaitForSeconds(_newWaveBannerStartDelay);

        float animatePercent = 0;
        int dir = 1;

        float endDelayTime = Time.time + 1 / _newWaveBannerSpeed + _newWaveBannerDelayTime;

        AudioManager.Instance.PlaySound2D(_newWaveBannerAppearSfx);

        while (animatePercent >= 0)
        {
            animatePercent += Time.unscaledDeltaTime * _newWaveBannerSpeed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;

                    AudioManager.Instance.PlaySound2D(_newWaveBannerDisappearSfx);
                }
            }

            if (_newWaveBannerRect)
            {
                _newWaveBannerRect.anchoredPosition = Vector2.up * Mathf.Lerp(_newWaveBannerStartYPos, _newWaveBannerEndYPos, animatePercent);
            }

            if (_newWaveBannerCanvasGroup)
            {
                _newWaveBannerCanvasGroup.alpha = Mathf.Clamp(animatePercent - 0.10f, 0.0f, 0.9f);
            }

            yield return null;
        }
    }

    void OnNewWave(string waveTitle)
    {
        if (_waveTitle)
        {
            _waveTitle.text = waveTitle;

            if (_animateNewWaveBanner != null)
            {
                StopCoroutine(_animateNewWaveBanner);
            }

            _animateNewWaveBanner = AnimateNewWaveBannerRoutine();

            StartCoroutine(_animateNewWaveBanner);
        }
    }

    private void OnDestroy()
    {
        if (_enemySpawner)
        {
            _enemySpawner.OnNewWave -= OnNewWave;
        }
    }
}
