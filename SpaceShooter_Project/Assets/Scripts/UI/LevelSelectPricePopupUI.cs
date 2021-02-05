using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class LevelSelectPricePopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private Button _buyButton;

    [SerializeField] private Button _watchAdButton;

    [SerializeField] private TextMeshProUGUI _priceText;

    [SerializeField] private Material _priceTextNoFundMaterial;

    [SerializeField] private Material _priceTextMaterial;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private RectTransform _windowRectTransform;

    [SerializeField] private CanvasGroup _windowCanvasGroup;

    [SerializeField] private CanvasGroup _contentCanvasGroup;

    [SerializeField] private float _openAndCloseDuration = 0.35f;

    [SerializeField] private IntVariable _coinsAmount = default;

    [SerializeField] private IntVariable _levelToLoad = default;

    [SerializeField] private GameEvent _closeWindowEvent = default;

    [SerializeField] private FloatGameEvent _loadArcadeEvent = default;

    [SerializeField] private float _loadArcadeDelay = 0.8f;

    //Remove:
    private const string PLAYER_PREFS_KEY_COINS = "COINS"; //string key used to load/save the coins value from/to PlayerPrefs

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _closeWindowEvent?.AddListener(Close);
    }

    private void OnEnable()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        

        _buyButton.onClick.RemoveAllListeners();

        //int levelToLoad = PlayerPrefs.GetInt("levelToLoad", 0);

        _levelText.text = "FAST TRAVEL TO LEVEL " + (_levelToLoad.Value + 1);


        switch (_levelToLoad.Value)
        {
            case 4:
                _levelText.text = "FAST TRAVEL TO BOSS 1";
                break;
            case 9:
                _levelText.text = "FAST TRAVEL TO BOSS 2";
                break;

        }

        int levelPrice = GetLevelPrice(_levelToLoad.Value);

        _priceText.text = levelPrice.ToString();


        if (levelPrice > _coinsAmount.Value)
        {
            _priceText.fontMaterial = _priceTextNoFundMaterial;
            _buyButton.interactable = false;
        }
        else
        {
            _priceText.fontMaterial = _priceTextMaterial;
            _buyButton.interactable = true;
            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() =>
            {
                _coinsAmount.Value -= levelPrice;

                PlayerPrefs.SetInt(PLAYER_PREFS_KEY_COINS, _coinsAmount.Value);

                _loadArcadeEvent.Raise(_loadArcadeDelay);
                Close();
                _closeWindowEvent?.Raise();
            });
        }

        Open();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Close()
    {
        _contentCanvasGroup.alpha = 0;
        _windowCanvasGroup.DOFade(0, .4f);
        AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ClickButton01);
        _windowRectTransform.DOScale(0, _openAndCloseDuration).SetEase(Ease.InBack).OnComplete(() => {
            transform.localPosition = _startPosition;
            gameObject.SetActive(false);
        });

    }

    public void Open()
    {
        _windowRectTransform.DOScale(0, 0);
        _windowCanvasGroup.alpha = 0;
        _contentCanvasGroup.alpha = 0;
        _windowRectTransform.DOScale(1, _openAndCloseDuration).SetEase(Ease.OutBack);
        _windowCanvasGroup.DOFade(1, .4f);
        _contentCanvasGroup.DOFade(1, .4f);
    }


    private int GetLevelPrice(int level)
    {
        switch (level)
        {
            case 0:
                return 0;
            case 1:
                return 25;
            case 2:
                return 50;
            case 3:
                return 75;
            case 4:
                return 100;
            case 5:
                return 125;
            case 6:
                return 150;
            case 7:
                return 175;
            case 8:
                return 200;
            case 9:
                return 225;
            default:
                return 250;
        }
    }

    private void OnDestroy()
    {
        _closeWindowEvent?.RemoveListener(Close);
    }
}
