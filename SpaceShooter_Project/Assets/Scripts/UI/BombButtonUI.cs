using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class BombButtonUI : MonoBehaviour
{

    [SerializeField] private Slider _bombStatusSlider;

    [SerializeField] private float _bombRechargeSpeed = 0.5f;

    [SerializeField] private GameObject _fullStatusIndicator;

    [SerializeField] private GameEvent _bombEvent = default;

    private float _bombStatus = 0;

    private void Start()
    {
        if (!GameDataController.IsBombAbilityEnable())
        {
            gameObject.SetActive(false);
        }

        _bombStatus = 0;
    }

    private void Update()
    {
        if (GameTime.isPaused)
        {
            return;
        }

        _bombStatus += Time.deltaTime * _bombRechargeSpeed;
        _bombStatus = Mathf.Clamp(_bombStatus, 0, 100);

        if (_bombStatusSlider != null)
        {
            _bombStatusSlider.value = _bombStatus;
        }

        if (_fullStatusIndicator != null)
        {
            if (_bombStatus >= 100)
            {
                _fullStatusIndicator.SetActive(true);
            }
            else
            {
                _fullStatusIndicator.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        UseBomb();
    }

    public void UseBomb()
    {
        if (GameTime.isPaused)
        {
            return;
        }
        if (_bombStatus >= 100.0f)
        {
            _bombEvent?.Raise();
            _bombStatus = 0;
        }
    }
}
