using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class SelectLevelUI : ExtendedCustomMonoBehavior
{
    [SerializeField] private Button[] _levelButtons;

    [SerializeField] private GameObject _levelSelectPricePopup;

    [SerializeField] private FloatGameEvent _loadArcadeEvent = default;

    [SerializeField] private IntVariable _levelToLoad;

    private PageController _pageController;


    private void Awake()
    {
        _pageController = FindObjectOfType<PageController>();
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void GameDataController_OnSaveDataChanged(object sender, System.EventArgs e)
    {
        Refresh();
    }

    public void Refresh()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 0);

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            TextMeshProUGUI text = _levelButtons[i].transform.Find("Text").GetComponent<TextMeshProUGUI>();
            if (i > levelReached)
            {
                _levelButtons[i].interactable = false;
                text.gameObject.SetActive(false);
                _levelButtons[i].transform.Find("LockIcon").gameObject.SetActive(true);
            }
            else
            {
                int level = i;
                if (i + 1 == 5)
                {
                    text.text = "BOSS 1";
                }
                else if (i + 1 == 10)
                {
                    text.text = "BOSS 2";
                }
                else
                {
                    text.text = (i + 1).ToString();
                }
                _levelButtons[i].onClick.RemoveAllListeners();
                _levelButtons[i].onClick.AddListener(() =>
                {
                    _levelToLoad.Value = level;

                    if (level == 0)
                    {
                        if (_pageController != null)
                        {
                            _pageController.CloseOpenPage();
                        }
                        _loadArcadeEvent?.Raise(0.5f);
                    }
                    else
                    {
                        _levelSelectPricePopup.transform.localPosition = Vector3.zero;
                        _levelSelectPricePopup.SetActive(true);
                    }

                    AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ClickButton01);

                });

            }
        }
    }
}
