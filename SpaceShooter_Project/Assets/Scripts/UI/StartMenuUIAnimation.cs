using UnityEngine;
using DG.Tweening;

public class StartMenuUIAnimation : MonoBehaviour
{
    [SerializeField] private float _buttonsFadeDuration = 0.3f;
    [SerializeField] private float _buttonsMoveDuration = 0.3f;
    [SerializeField] private float _buttonsAppearDelay = 2.0f;
    [SerializeField] private float _moveDistance = 1.0f;

    private CanvasGroup _playGameButtonCanvasGroup;
    private CanvasGroup _settingsButtonCanvasGroup;
    private CanvasGroup _shopButtonCanvasGroup;
    private CanvasGroup _creditsButtonCanvasGroup;

    private RectTransform _playGameButtonRectTransform;
    private RectTransform _settingsButtonRectTransform;
    private RectTransform _shopButtonRectTransform;
    private RectTransform _creditsButtonRectTransform;

    private Vector3 _playGameButtonTargetLocation;
    private Vector3 _settingsButtonTargetLocation;
    private Vector3 _shopButtonTargetLocation;
    private Vector3 _creditsButtonTargetLocation;

    private void Awake()
    {
        _playGameButtonRectTransform = transform.Find("PlayButton")?.GetComponent<RectTransform>();
        _settingsButtonRectTransform = transform.Find("SettingsButton")?.GetComponent<RectTransform>();
        _shopButtonRectTransform = transform.Find("ShopButton")?.GetComponent<RectTransform>();
        _creditsButtonRectTransform = transform.Find("CreditsButton")?.GetComponent<RectTransform>();

        if (_playGameButtonRectTransform != null)
        {
            _playGameButtonTargetLocation = _playGameButtonRectTransform.localPosition;
            _playGameButtonCanvasGroup = _playGameButtonRectTransform.GetComponent<CanvasGroup>();
        }
        else
        {
            Debug.Log("_playGameButtonRectTransform == null");
        }

        if (_settingsButtonRectTransform != null)
        {
            _settingsButtonTargetLocation = _settingsButtonRectTransform.localPosition;
            _settingsButtonCanvasGroup = _settingsButtonRectTransform.GetComponent<CanvasGroup>();
        }
        else
        {
            Debug.Log("_settingsButtonRectTransform == null");
        }

        if (_shopButtonRectTransform != null)
        {
            _shopButtonTargetLocation = _shopButtonRectTransform.localPosition;
            _shopButtonCanvasGroup = _shopButtonRectTransform.GetComponent<CanvasGroup>();
        }
        else
        {
            Debug.Log("_shopButtonRectTransform == null");
        }

        if (_creditsButtonRectTransform != null)
        {
            _creditsButtonTargetLocation = _creditsButtonRectTransform.localPosition;
            _creditsButtonCanvasGroup = _creditsButtonRectTransform.GetComponent<CanvasGroup>();
        }
        else
        {
            Debug.Log("_creditsButtonRectTransform == null");
        }

    }

    private void Start()
    {
       

        Sequence sequence = DOTween.Sequence().SetDelay(_buttonsAppearDelay)
               .OnStart(() =>
               {
                   _playGameButtonCanvasGroup.alpha = 0.0f;
                   _playGameButtonCanvasGroup.transform.localPosition = new Vector2(_playGameButtonTargetLocation.x, _playGameButtonTargetLocation.y - _moveDistance);
                   _settingsButtonCanvasGroup.alpha = 0.0f;
                   _settingsButtonCanvasGroup.transform.localPosition = new Vector2(_settingsButtonTargetLocation.x, _settingsButtonTargetLocation.y - _moveDistance);
                   _shopButtonCanvasGroup.alpha = 0.0f;
                   _shopButtonCanvasGroup.transform.localPosition = new Vector2(_shopButtonTargetLocation.x, _shopButtonTargetLocation.y - _moveDistance);
                   _creditsButtonCanvasGroup.alpha = 0.0f;
                   _creditsButtonCanvasGroup.transform.localPosition = new Vector2(_creditsButtonTargetLocation.x, _creditsButtonTargetLocation.y - _moveDistance);

               })
               .Append(_playGameButtonCanvasGroup.DOFade(1.0f, _buttonsFadeDuration).SetEase(Ease.OutCubic))
               .Join(_playGameButtonCanvasGroup.transform.DOLocalMove(_playGameButtonTargetLocation, _buttonsMoveDuration).SetEase(Ease.OutCubic))
               .Append(_settingsButtonCanvasGroup.DOFade(1.0f, _buttonsFadeDuration).SetEase(Ease.OutCubic))
               .Join(_settingsButtonCanvasGroup.transform.DOLocalMove(_settingsButtonTargetLocation, _buttonsMoveDuration).SetEase(Ease.OutCubic))
               .Append(_shopButtonCanvasGroup.DOFade(1.0f, _buttonsFadeDuration).SetEase(Ease.OutCubic))
               .Join(_shopButtonCanvasGroup.transform.DOLocalMove(_shopButtonTargetLocation, _buttonsMoveDuration).SetEase(Ease.OutCubic))
               .Append(_creditsButtonCanvasGroup.DOFade(1.0f, _buttonsFadeDuration).SetEase(Ease.OutCubic))
               .Join(_creditsButtonCanvasGroup.transform.DOLocalMove(_creditsButtonTargetLocation, _buttonsMoveDuration).SetEase(Ease.OutCubic));

        sequence.Play();
    }

}
