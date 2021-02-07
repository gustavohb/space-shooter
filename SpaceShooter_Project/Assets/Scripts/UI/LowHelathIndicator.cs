using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LowHelathIndicator : MonoBehaviour
{

    private CanvasGroup _canvasGroup;

    [SerializeField] private float _lowHealthAlphaChange = 4f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private void Update()
    {

        if (GameTime.isPaused)
        {
            return;
        }

        float lowHealthAlpha = _canvasGroup.alpha;
        lowHealthAlpha += _lowHealthAlphaChange * Time.deltaTime * 1.0f;

        if (lowHealthAlpha > 1f)
        {
            _lowHealthAlphaChange *= -1f;
            lowHealthAlpha = 1f;
        }
        if (lowHealthAlpha < 0f)
        {
            _lowHealthAlphaChange *= -1f;
            lowHealthAlpha = 0f;
        }


        _canvasGroup.alpha = lowHealthAlpha;
    }


}
