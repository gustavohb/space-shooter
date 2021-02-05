using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private GameObject _loadingScreen;

    [SerializeField] private Slider _slider;

    [SerializeField] private float _loadDelay = 0.5f;

    [SerializeField] private bool _startLoading = false;

    [SerializeField] private CanvasGroup _crossFadeCanvasGroup;

    [SerializeField] private float _fadeDuration = 1f;

    [SerializeField] private FloatGameEvent _loadArcadeEvent = default;

    [SerializeField] private FloatGameEvent _loadStartEvent = default;

    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        GameTime.isPaused = false;
        Time.timeScale = 1.0f;

        _crossFadeCanvasGroup.alpha = 1;
    }

    private IEnumerator Start()
    {
        _loadArcadeEvent?.AddListener(LoadArcade);
        _loadStartEvent?.AddListener(LoadStart);


        _crossFadeCanvasGroup.alpha = 1;
        yield return new WaitForSeconds(1);

        FadeOut();
        yield return new WaitForSeconds(_fadeDuration);

        if (_startLoading)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(string sceneName, float delay)
    {
        if (delay > 0.0f)
        {
            yield return new WaitForSeconds(delay);
        }
        StartCoroutine(LoadLevel(sceneName));
    }

    public void LoadStart(float delay = 0.0f)
    {
        StartCoroutine(LoadLevel("Start", delay));
    }

    public void LoadArcade(float delay = 0.0f)
    {
        StartCoroutine(LoadLevel("Arcade", delay));
    }

    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void ReloadLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        Time.timeScale = 1.0f;

        _loadingScreen.SetActive(true);

        yield return new WaitForSeconds(_loadDelay);

        StartCoroutine(LoadAsynchronously(sceneName));

        yield return new WaitForEndOfFrame();
    }

    IEnumerator LoadLevel(int sceneIndex)
    {

        Time.timeScale = 1.0f;

        _loadingScreen.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(LoadAsynchronously(sceneIndex));

        yield return new WaitForEndOfFrame();

    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        yield return null;

        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        _asyncOperation.allowSceneActivation = false;


        Debug.LogWarning("ASYNC LOAD STARTED - " +
            "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");

        while (_asyncOperation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);

            _slider.value = progress;

            yield return new WaitForEndOfFrame();
        }

        _slider.value = 1.0f;

        FadeIn();
        yield return new WaitForSeconds(_fadeDuration);

        _asyncOperation.allowSceneActivation = true;

        yield return null;
    }



    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        yield return null;

        _asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        _asyncOperation.allowSceneActivation = false;


        Debug.LogWarning("ASYNC LOAD STARTED - " +
            "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");

        while (_asyncOperation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);

            _slider.value = progress;

            yield return new WaitForEndOfFrame();
        }

        _slider.value = 1.0f;

        FadeIn();
        yield return new WaitForSeconds(_fadeDuration);

        _asyncOperation.allowSceneActivation = true;

        yield return null;
    }

    private void FadeIn()
    {
        _crossFadeCanvasGroup.blocksRaycasts = true;
        _crossFadeCanvasGroup.DOFade(1, _fadeDuration);
    }

    private void FadeOut()
    {
        _crossFadeCanvasGroup.DOFade(0, _fadeDuration).OnComplete(() => {
            _crossFadeCanvasGroup.blocksRaycasts = false;
        });
    }

    private void OnDestroy()
    {
        _loadArcadeEvent?.RemoveListener(LoadArcade);
        _loadStartEvent?.RemoveListener(LoadStart);
    }
}
