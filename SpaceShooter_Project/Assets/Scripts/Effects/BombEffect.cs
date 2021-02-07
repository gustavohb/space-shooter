using UnityEngine;
using ScriptableObjectArchitecture;

public class BombEffect : MonoBehaviour
{
    [SerializeField] private GameObject _bombEffectPrefab;

    [SerializeField] private GameEvent _bombEvent = default;

    private void Start()
    {
        _bombEvent?.AddListener(Explode);
    }

    private void Explode()
    {
        if (_bombEffectPrefab != null)
        {
            Instantiate(_bombEffectPrefab, transform.position, transform.rotation);
        }
    }

    private void OnDestroy()
    {
        _bombEvent?.RemoveListener(Explode);
    }
}
