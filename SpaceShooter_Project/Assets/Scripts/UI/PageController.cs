using UnityEngine;
using DG.Tweening;

public class PageController : MonoBehaviour
{

    [SerializeField] private RectTransform _gameModePage;

    [SerializeField] private RectTransform _selectLevelPage;

    [SerializeField] private RectTransform _settingsPage;

    [SerializeField] private RectTransform _shopPage;

    [SerializeField] private RectTransform _creditsPage;

    [SerializeField] private Animator _windowBackgroundAnimator;

    [SerializeField] private float _windowAppearFadeDuration = 1f;

    [SerializeField] private RectTransform _windowBackground;

    private RectTransform _currentOpenPage;

    private Vector3 _startPosition;

    [SerializeField] private Ease ease = Ease.OutBack;

    private void Start()
    {
        _gameModePage.gameObject.SetActive(false);
        //_selectLevelPage.gameObject.SetActive(false);
        _settingsPage.gameObject.SetActive(false);
        _shopPage.gameObject.SetActive(false);
        _creditsPage.gameObject.SetActive(false);
    }

    public void OpenSelectModePage()
    {
        OpenPage(_gameModePage);

    }

    public void OpenSelectLevelPage()
    {
        OpenPage(_selectLevelPage);
    }

    public void OpenSettingsPage()
    {
        OpenPage(_settingsPage);
    }

    public void OpenShopPage()
    {
        OpenPage(_shopPage);
    }

    public void OpenCreditsPage()
    {
        OpenPage(_creditsPage);
    }

    public void CloseOpenPage()
    {
        _currentOpenPage.localPosition = _startPosition;
        _currentOpenPage.gameObject.SetActive(false);
        _currentOpenPage = null;
        _windowBackgroundAnimator.SetBool("open", false);
    }

    private void OpenPage(RectTransform page)
    {
        if (_currentOpenPage == page) return;

        _windowBackgroundAnimator.SetBool("open", true);


        if (_currentOpenPage != null)
        {
            _currentOpenPage.localPosition = _startPosition;
            _currentOpenPage.gameObject.SetActive(false);
        }

        _currentOpenPage = page;
        _currentOpenPage.gameObject.SetActive(true);

        _startPosition = page.localPosition;
        _currentOpenPage.localPosition = _windowBackground.localPosition;
    }
}


