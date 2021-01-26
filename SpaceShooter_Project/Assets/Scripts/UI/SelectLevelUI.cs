using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectLevelUI : ExtendedCustomMonoBehavior
{
    [SerializeField] private Button[] _levelButtons;

    [SerializeField] private LevelLoader _levelLoader;

    [SerializeField] private GameObject _levelSelectWindow;

    [SerializeField] private GameObject _levelSelectPricePopup;

    private void Awake()
    {
        Refresh();
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
                text.text = (i + 1).ToString();
                _levelButtons[i].onClick.RemoveAllListeners();
                _levelButtons[i].onClick.AddListener(() =>
                {
                    PlayerPrefs.SetInt("levelToLoad", level);
                    PlayerPrefs.Save();

                    if (level == 0)
                    {
                        _levelLoader.LoadArcade();

                        if (_levelSelectWindow != null)
                        {
                            _levelSelectWindow.SetActive(false);
                        }
                    }
                    else
                    {
                        _levelSelectPricePopup.SetActive(true);
                    }
                });

            }
        }
    }
}
