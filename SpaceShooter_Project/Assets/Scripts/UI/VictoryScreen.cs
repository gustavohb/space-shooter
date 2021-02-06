using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(CanvasGroup))]
public class VictoryScreen : ExtendedCustomMonoBehavior
{

    [SerializeField] private float _panelFadeDuration = 2.0f;

    [SerializeField] private FloatGameEvent _loadStartEvent = default;

    [SerializeField] private GameObject _doubleCollectedCoinsScreen;

    private CanvasGroup _panelCanvasGroup;

    private void Awake()
    {
        _panelCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _panelCanvasGroup.alpha = 0.0f;
        _panelCanvasGroup.DOFade(1.0f, _panelFadeDuration);
        GameTime.isPaused = true;
    }

    private void PlayVictorySound()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.Victory);

    }

    public void OpenDoubleCollectCoinsScreen()
    {
        _doubleCollectedCoinsScreen?.SetActive(true);
    }
}
