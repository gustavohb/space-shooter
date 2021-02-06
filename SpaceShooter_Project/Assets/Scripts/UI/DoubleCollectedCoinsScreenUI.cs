using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DoubleCollectedCoinsScreenUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _collectedCoinsTextCanvasGroup;

    [SerializeField] private TextMeshProUGUI _coinsCollectedAmountTMP;

    [SerializeField] private CanvasGroup _coinsCollectedAmountCanvasGroup;

    [SerializeField] private CanvasGroup _doubleCollectedCoinsCanvasGroup;

    [SerializeField] private CanvasGroup _watchAdButtonCanvasGroup;

    [SerializeField] private CanvasGroup _continueButtonCanvasGroup;

    [SerializeField] private CanvasGroup _coinIconCanvasGroup;

    [SerializeField] private float _textMoveDistance = 1.0f;

    [SerializeField] private float _textFadeDuration = 0.5f;

    [SerializeField] private float _textMoveDuration = 1.0f;

    [SerializeField] private float _buttonMoveDistance = 1.0f;

    [SerializeField] private float _buttonFadeDuration = 0.3f;

    [SerializeField] private float _buttonMoveDuration = 0.3f;

    [SerializeField] private GameEvent _doubleCollectedCoinsEvent = default;

    [SerializeField] private FloatGameEvent _loadStartEvent = default;

    [SerializeField] private IntVariable _collectedCoins;

    private IEnumerator collectedCoinsRoutine;

    private void OnEnable()
    {


        _collectedCoinsTextCanvasGroup.alpha = 0.0f;
        _coinsCollectedAmountCanvasGroup.alpha = 0.0f;

        _doubleCollectedCoinsCanvasGroup.alpha = 0.0f;

        _watchAdButtonCanvasGroup.alpha = 0.0f;
        _continueButtonCanvasGroup.alpha = 0.0f;
        _coinIconCanvasGroup.alpha = 0.0f;

#if PLATFORM_WEBGL
        LoadMenu();
        return;
#endif


        if (!AdsManager.Instance.isAdsReady)
        {
            LoadMenu();
            gameObject.SetActive(false);
            return;
        }

        if (_collectedCoins.Value == 0)
        {
            LoadMenu();
            gameObject.SetActive(false);
            return;
        }
        
        _doubleCollectedCoinsEvent?.AddListener(UpdateCollectedCoins);

        _coinsCollectedAmountTMP.text = _collectedCoins.Value.ToString();

        Vector2 collectedCoinsTextTargetLocation = _collectedCoinsTextCanvasGroup.transform.position;

        Vector2 coinsCollectedAmountTargetLocation = _coinsCollectedAmountCanvasGroup.transform.position;
        Vector2 coinIconTargetLocation = _coinIconCanvasGroup.transform.position;

        Vector2 doubleCollectedCoinsTargetLocation = _doubleCollectedCoinsCanvasGroup.transform.position;

        Vector2 watchAdButtonTargetLocation = _watchAdButtonCanvasGroup.transform.position;
        Vector2 continueButtonTargetLocation = _continueButtonCanvasGroup.transform.position;

        Sequence sequence = DOTween.Sequence()
            .OnStart(() =>
            {
                _collectedCoinsTextCanvasGroup.transform.position = new Vector2(collectedCoinsTextTargetLocation.x, collectedCoinsTextTargetLocation.y - _textMoveDistance);

                _coinsCollectedAmountCanvasGroup.transform.position = new Vector2(coinsCollectedAmountTargetLocation.x, coinsCollectedAmountTargetLocation.y - _textMoveDistance);

                _coinIconCanvasGroup.transform.position = new Vector2(coinIconTargetLocation.x, coinIconTargetLocation.y - _textMoveDistance);

                _doubleCollectedCoinsCanvasGroup.transform.position = new Vector2(doubleCollectedCoinsTargetLocation.x, doubleCollectedCoinsTargetLocation.y - _textMoveDistance);



                _watchAdButtonCanvasGroup.transform.position = new Vector2(watchAdButtonTargetLocation.x, watchAdButtonTargetLocation.y - _buttonMoveDistance);
                _continueButtonCanvasGroup.transform.position = new Vector2(continueButtonTargetLocation.x, continueButtonTargetLocation.y - _buttonMoveDistance);
            })


            .Append(_collectedCoinsTextCanvasGroup.DOFade(1.0f, _textFadeDuration)).SetEase(Ease.OutCubic)
            .Join(_collectedCoinsTextCanvasGroup.transform.DOMove(collectedCoinsTextTargetLocation, _textMoveDuration).SetEase(Ease.OutCubic))

             .Append(_coinsCollectedAmountCanvasGroup.DOFade(1.0f, _textFadeDuration)).SetEase(Ease.OutCubic)
            .Join(_coinsCollectedAmountCanvasGroup.transform.DOMove(coinsCollectedAmountTargetLocation, _textMoveDuration).SetEase(Ease.OutCubic))

            .Join(_coinIconCanvasGroup.DOFade(1.0f, _textFadeDuration)).SetEase(Ease.OutCubic)
            .Join(_coinIconCanvasGroup.transform.DOMove(coinIconTargetLocation, _textMoveDuration).SetEase(Ease.OutCubic))

            .Append(_doubleCollectedCoinsCanvasGroup.DOFade(1.0f, _textFadeDuration)).SetEase(Ease.OutCubic)
            .Join(_doubleCollectedCoinsCanvasGroup.transform.DOMove(doubleCollectedCoinsTargetLocation, _textMoveDuration).SetEase(Ease.OutCubic))

            .Append(_watchAdButtonCanvasGroup.DOFade(1.0f, _buttonFadeDuration)).SetEase(Ease.OutCubic)
            .Join(_watchAdButtonCanvasGroup.transform.DOMove(watchAdButtonTargetLocation, _buttonMoveDuration).SetEase(Ease.OutCubic))

            .Append(_continueButtonCanvasGroup.DOFade(1.0f, _buttonFadeDuration)).SetEase(Ease.OutCubic)
            .Join(_continueButtonCanvasGroup.transform.DOMove(continueButtonTargetLocation, _buttonMoveDuration).SetEase(Ease.OutCubic));

    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        _loadStartEvent?.Raise(0.0f);
    }

    public void UpdateCollectedCoins()
    {
        if (collectedCoinsRoutine != null)
        {
            StopCoroutine(collectedCoinsRoutine);
        }
        collectedCoinsRoutine = UpdateCollectedCoinsCoroutine();

        StartCoroutine(collectedCoinsRoutine);
    }

    private IEnumerator UpdateCollectedCoinsCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        float totalTimeToAddCoins = 0.5f;
        float timeBetweenCoinsIncrement = 0.1f;

        int coisCountIncrement = (int)((_collectedCoins.Value * timeBetweenCoinsIncrement) / (float)totalTimeToAddCoins);
        if (coisCountIncrement <= 0)
        {
            coisCountIncrement = 1;
        }
        for (int currentCollectedCoins = _collectedCoins.Value; currentCollectedCoins <= _collectedCoins.Value * 2; currentCollectedCoins += coisCountIncrement)
        {
            _coinsCollectedAmountTMP.text = currentCollectedCoins.ToString();
            AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.UIPickupCoin);
            yield return new WaitForSeconds(timeBetweenCoinsIncrement);
        }
        _coinsCollectedAmountTMP.text = (2 * _collectedCoins.Value).ToString();
    }
}
