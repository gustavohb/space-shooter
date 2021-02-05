using UnityEngine;
using DG.Tweening;
using System.Collections;
using ScriptableObjectArchitecture;

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

    [SerializeField] private float _closeWindowDelay = 0.7f;

    [SerializeField] private GameEvent _closeWindowEvent = default;

    private RectTransform _currentOpenPage;

    private Vector3 _startPosition;

    [SerializeField] private Ease ease = Ease.OutBack;

    private void Start()
    {
        _gameModePage.gameObject.SetActive(false);
        _selectLevelPage.gameObject.SetActive(false);
        _settingsPage.gameObject.SetActive(false);
        _shopPage.gameObject.SetActive(false);
        _creditsPage.gameObject.SetActive(false);

        _closeWindowEvent.AddListener(CloseWindow);

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

    private void CloseWindow()
    {
        CloseOpenPage(_closeWindowDelay);
    }

    public void CloseOpenPage(float delay)
    {
        StartCoroutine(CloseRoutine(delay));
    }

    private IEnumerator CloseRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ClickButton01);
        CloseOpenPage();
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

    private void OnDestroy()
    {
        _closeWindowEvent.RemoveListener(CloseWindow);
    }
}


