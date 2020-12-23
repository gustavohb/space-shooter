using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField]
    // The duration of the tween.
    private float _duration = 0.2f;

    [SerializeField]
    // The shake strength. Using a Vector3 instead of a float lets you choose the strength for each axis.
    private float _strength = 0.25f;

    [SerializeField]
    // Indicates how much will the shake vibrate.
    private int _vibrato = 20;

    [SerializeField]
    // Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). Setting it to 0 will shake along a single direction.
    private float _randomness = 45.0f;

    [SerializeField]
    // If TRUE the tween will smoothly snap all values to integers.
    private bool _snapping = false;

    [SerializeField]
    //  If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not.
    private bool _fadeOut = false;


    [SerializeField]
    private bool _vibratesHandheld = false;

    [SerializeField]
    private float _startDelay = 0;

    private Transform _targetCamera;

    private IEnumerator Start()
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

    public void StartShake()
    {
        _targetCamera.transform.DOShakePosition(_duration, _strength, _vibrato, _randomness, _snapping, _fadeOut);
    }

}

