using UnityEngine;
using ScriptableObjectArchitecture;

public class GameUICanvas : ExtendedCustomMonoBehavior
{
    [SerializeField] private GameObject _pauseScreen;

    [SerializeField] private GameObject _enemyHealthShieldUICanvas;

    [SerializeField] private GameEvent _playerDeathEvent = default;

    [SerializeField] private GameObject _defeatedScreen;

    private void Start()
    {
        if (_enemyHealthShieldUICanvas == null)
        {
            _enemyHealthShieldUICanvas = FindObjectOfType<EnemyHealthShieldBarsController>()?.gameObject;
        }

        _playerDeathEvent?.AddListener(ShowDefeatedScreen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ClickButton01);
            OpenPauseScreen();
        }
        if (_enemyHealthShieldUICanvas != null && _pauseScreen.activeSelf && _enemyHealthShieldUICanvas.activeSelf)
        {
            _enemyHealthShieldUICanvas.SetActive(false);
        }
        else if (_enemyHealthShieldUICanvas != null && !_pauseScreen.activeSelf && !_enemyHealthShieldUICanvas.activeSelf)
        {
            _enemyHealthShieldUICanvas.SetActive(true);
        }
    }

    public void OpenPauseScreen()
    {
        if (_pauseScreen != null && !_pauseScreen.activeSelf)
        {
            _pauseScreen.SetActive(true);
        }
    }

    private void ShowDefeatedScreen()
    {
        _defeatedScreen?.SetActive(true);
    }
}
