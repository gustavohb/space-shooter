using UnityEngine;
using DG.Tweening;

public class SelectGameModeUIAnimation : MonoBehaviour
{
    [SerializeField] private float _animationDelay = 1f;
    [SerializeField] private float _moveXDistace = 50f;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private float _fadeDuration = 0.8f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;

    private RectTransform _selectLevelButtonRectTransform;
    private RectTransform _endlessButtonRectTransform;

    private CanvasGroup _selectLevelButtonCanvasGroup;
    private CanvasGroup _endlessButtonCanvasGroup;

    private Vector3 _selectLevelButtonEndPos;
    private Vector3 _endlessButtonEndPos;

    private Sequence _animationSequence;

    private void Awake()
    {
        _selectLevelButtonRectTransform = transform.Find("SelectLevelButton")?.GetComponent<RectTransform>();
        _endlessButtonRectTransform = transform.Find("EndlessButton")?.GetComponent<RectTransform>();

        if (_selectLevelButtonRectTransform != null)
        {
            _selectLevelButtonEndPos = _selectLevelButtonRectTransform.localPosition;
        }

        if (_endlessButtonRectTransform != null)
        {
            _endlessButtonEndPos = _endlessButtonRectTransform.localPosition;
        }


    }

    private void OnEnable()
    {
        _animationSequence = DOTween.Sequence();
        _animationSequence.AppendInterval(_animationDelay);

        if (_selectLevelButtonRectTransform != null)
        {
            

            _animationSequence.Append(_selectLevelButtonRectTransform.DOLocalMoveX(_selectLevelButtonEndPos.x, _moveDuration).SetEase(_moveEase));

            _selectLevelButtonCanvasGroup = _selectLevelButtonRectTransform.GetComponent<CanvasGroup>();

            if (_selectLevelButtonCanvasGroup != null)
            {
                _selectLevelButtonCanvasGroup.alpha = 0.0f;
                _animationSequence.Join(_selectLevelButtonCanvasGroup.DOFade(1.0f, _fadeDuration));
            }
            else
            {
                Debug.Log("_selectLevelButtonCanvasGroup == null");
            }
        }
        else
        {
            Debug.Log("_selectLevelButtonRectTransform = null");
        }

        if (_endlessButtonRectTransform != null)
        {
            
            _endlessButtonCanvasGroup = _endlessButtonRectTransform.GetComponent<CanvasGroup>();

            if (_endlessButtonCanvasGroup != null)
            {
                _endlessButtonCanvasGroup.alpha = 0.0f;
                _animationSequence.Join(_endlessButtonCanvasGroup.DOFade(1.0f, _fadeDuration));
            }
            else
            {
                Debug.Log("_endlessButtonCanvasGroup == null");
            }

            _animationSequence.Join(_endlessButtonRectTransform.DOLocalMoveX(_endlessButtonEndPos.x, _moveDuration).SetEase(_moveEase));
        }
        else
        {
            Debug.Log("_endlessButtonRectTransform == null");
        }


        if (_selectLevelButtonRectTransform != null)
        {
            _selectLevelButtonRectTransform.localPosition = new Vector3(_selectLevelButtonEndPos.x - _moveXDistace, _selectLevelButtonEndPos.y, _selectLevelButtonEndPos.z);
        }

        if (_endlessButtonRectTransform != null)
        {
            _endlessButtonRectTransform.localPosition = new Vector3(_endlessButtonEndPos.x + _moveXDistace, _endlessButtonEndPos.y, _endlessButtonEndPos.z);
        }

        _animationSequence?.Play();

    }

    private void OnDisable()
    {
        _animationSequence?.Kill();
        if (_selectLevelButtonRectTransform != null)
        {
            _selectLevelButtonRectTransform.localPosition = new Vector3(_selectLevelButtonEndPos.x - _moveXDistace, _selectLevelButtonEndPos.y, _selectLevelButtonEndPos.z);
            if (_selectLevelButtonCanvasGroup != null)
            {
                _selectLevelButtonCanvasGroup.alpha = 0.0f;
            }
        }

        if (_endlessButtonRectTransform != null)
        {
            _endlessButtonRectTransform.localPosition = new Vector3(_endlessButtonEndPos.x + _moveXDistace, _endlessButtonEndPos.y, _endlessButtonEndPos.z);
            if (_endlessButtonCanvasGroup != null)
            {
                _endlessButtonCanvasGroup.alpha = 0.0f;
            }
        }
        
    }

}
