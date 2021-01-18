using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInCanvasGroup : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1.0f;

    [SerializeField] private float _startDelay = 0.0f;

    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeIn(_fadeDuration));
    }

    private IEnumerator FadeIn(float duration)
    {
        float percent = 0.0f;
        float velocity = 1.0f / duration;
        _canvasGroup.alpha = percent;

        yield return new WaitForSeconds(_startDelay);

        while (percent < 1)
        {
            percent += Time.deltaTime * velocity;
            _canvasGroup.alpha = percent;
            yield return null;
        }
    }
}
