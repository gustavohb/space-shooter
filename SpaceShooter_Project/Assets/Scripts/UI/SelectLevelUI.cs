using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class SelectLevelUI : ExtendedCustomMonoBehavior
{
    [SerializeField] private Button[] _levelButtons;

    [SerializeField] private LevelLoader _levelLoader;

    [SerializeField] private GameObject _levelSelectWindow;

    [SerializeField] private GameObject _levelSelectPricePopup;

    [SerializeField] private IntVariable _levelToLoad;

    private PageController _pageController;


    private void Awake()
    {
        Refresh();

        _pageController = FindObjectOfType<PageController>();
    }

    private void GameDataController_OnSaveDataChanged(object sender, System.EventArgs e)
    {
        Refresh();
    }

    public void Refresh()
    {
        if (_levelLoader == null)
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
        }


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
                        _levelLoader.LoadArcade(0.5f);

                        if (_levelSelectWindow != null)
                        {
                            _levelSelectWindow.SetActive(false);
                        }
                    }
                    else
                    {
                        _levelSelectPricePopup.transform.localPosition = Vector3.zero;
                        _levelSelectPricePopup.SetActive(true);
                    }
                });

            }
        }
    }
}
