using UnityEngine;
using DG.Tweening;

public class VictoryScreen : ExtendedCustomMonoBehavior
{

    [SerializeField] private float _panelFadeDuration = 2.0f;

    private CanvasGroup _panelCanvasGroup;

    private void Awake()
    {
        _panelCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _panelCanvasGroup.alpha = 0.0f;
        _panelCanvasGroup.DOFade(1.0f, _panelFadeDuration);
    }

    private void PlayVictorySound()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.Victory);

    }
}
