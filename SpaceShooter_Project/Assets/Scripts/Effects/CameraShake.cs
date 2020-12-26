using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // The duration of the tween.
    [SerializeField] private float _duration = 0.2f;

    // The shake strength. Using a Vector3 instead of a float lets you choose the strength for each axis.
    [SerializeField] private float _strength = 0.25f;

    // Indicates how much will the shake vibrate.
    [SerializeField] private int _vibrato = 20;

    // Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). Setting it to 0 will shake along a single direction.
    [SerializeField] private float _randomness = 45.0f;

    // If TRUE the tween will smoothly snap all values to integers.
    [SerializeField] private bool _snapping = false;

    //  If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not.
    [SerializeField] private bool _fadeOut = false;

    [SerializeField] private bool _vibratesHandheld = false;

    [SerializeField] private float _startDelay = 0;

    private Transform _targetCamera;

    private void OnEnable()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    private IEnumerator CameraShakeRoutine()
    {
        _targetCamera = Camera.main.transform;

        if (_targetCamera)
        {
            if (_startDelay > 0)
            {
                yield return new WaitForSeconds(_startDelay);
            }

#if UNITY_ANDROID
            if (_vibratesHandheld)
            {
                Handheld.Vibrate();
            }
#endif
            StartShake();
        }
    }

    private void StartShake()
    {
        _targetCamera.transform.DOShakePosition(_duration, _strength, _vibrato, _randomness, _snapping, _fadeOut);
    }

}

