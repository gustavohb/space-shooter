using UnityEngine;

public class ReleaseGameObject : ExtendedCustomMonoBehavior
{
    [SerializeField]
    private float _releaseAfter = 1.0f;

    private float _timer;

    private void OnEnable()
    {
        _timer = _releaseAfter;
    }

    private void Update()
    {
        _timer -= GameTime.deltaTime;

        if (_timer <= 0)
        {
            ObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }
}
