using UnityEngine;
using DG.Tweening;

public class BasePageUIAnimation : MonoBehaviour
{
    [Header("Title Animation Settings")]
    [SerializeField] private float _titleMoveDelay = 0.15f;
    [SerializeField] private float _titleMoveXDitance = 250f;
    [SerializeField] private Ease _titleMoveEase = Ease.OutCubic;
    [SerializeField] private float _titleMoveDuration = 0.7f;
    [SerializeField] private float _titleFadeDuration = 1f;

    [Header("Close Button Animation Settings")]
    [SerializeField] private float _closeButtonMoveDelay = 1.0f;
    [SerializeField] private float _closeButtonMoveYDistance = 40f;
    [SerializeField] private Ease _closeButtonEase = Ease.Linear;
    [SerializeField] private float _closeButtonMoveDuration = 0.35f;
    [SerializeField] private float _closeButtonFadeDuration = 0.5f;



    private Vector3 _titleEndPos;
    private Vector3 _closeButtonEndPos;

    private RectTransform _titleRectTransform;
    private RectTransform _closeButtonRectTransform;

    private CanvasGroup _titleCanvasGroup;
    private CanvasGroup _closeButtonCanvasGroup;

    private Sequence _titleAnimationSequence;
    private Sequence _closeButtonAnimationSequence;

    private void Awake()
    {
        _titleRectTransform = transform.Find("Title")?.GetComponent<RectTransform>();
        _closeButtonRectTransform = transform.Find("CloseButton")?.GetComponent<RectTransform>();

        _titleEndPos = _titleRectTransform.localPosition;
        _closeButtonEndPos = _closeButtonRectTransform.localPosition;
    }

    private void OnEnable()
    {
        if (_titleRectTransform != null)
        {
            _titleAnimationSequence = DOTween.Sequence();
            
            _titleRectTransform.localPosition = new Vector3(_titleRectTransform.localPosition.x - _titleMoveXDitance, _titleRectTransform.localPosition.y, _titleRectTransform.localPosition.z);
            _titleAnimationSequence.AppendInterval(_titleMoveDelay)
                .Append(_titleRectTransform.DOLocalMoveX(_titleEndPos.x, _titleMoveDuration).SetEase(_titleMoveEase));
            

            _titleCanvasGroup = _titleRectTransform.GetComponent<CanvasGroup>();

            if (_titleCanvasGroup != null)
            {
                _titleCanvasGroup.alpha = 0f;
                _titleAnimationSequence.Join(_titleCanvasGroup.DOFade(1f, _titleFadeDuration));
            }

            _titleAnimationSequence.Play();
        }
        else
        {
            Debug.Log("_titleRectTransform == null");
        }

        if (_closeButtonRectTransform != null)
        {
            _closeButtonAnimationSequence = DOTween.Sequence();
           

            _closeButtonRectTransform.localPosition = new Vector3(_closeButtonRectTransform.localPosition.x, _closeButtonRectTransform.localPosition.y - _closeButtonMoveYDistance, _closeButtonRectTransform.localPosition.z);
            _closeButtonCanvasGroup = _closeButtonRectTransform.GetComponent<CanvasGroup>();
            _closeButtonAnimationSequence.AppendInterval(_closeButtonMoveDelay)
                .Append(_closeButtonRectTransform.DOLocalMoveY(_closeButtonEndPos.y, _closeButtonMoveDuration).SetEase(_closeButtonEase));

            _closeButtonCanvasGroup = _closeButtonRectTransform.GetComponent<CanvasGroup>();

            if (_closeButtonCanvasGroup != null)
            {
                _closeButtonCanvasGroup.alpha = 0f;
                _closeButtonAnimationSequence.Join(_closeButtonCanvasGroup.DOFade(1, _closeButtonFadeDuration));
            }

            _closeButtonAnimationSequence.Play();
        }
        else
        {
            Debug.Log("_closeButtonRectTransform != null");
        }
    }

    private void OnDisable()
    {
        _titleAnimationSequence?.Kill();
        _titleRectTransform.localPosition = _titleEndPos;
        _closeButtonRectTransform.localPosition = _closeButtonEndPos;
        
    }

}
