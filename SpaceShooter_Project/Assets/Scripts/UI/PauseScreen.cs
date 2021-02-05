using UnityEngine;
using ScriptableObjectArchitecture;

public class PauseScreen : ExtendedCustomMonoBehavior
{
    [SerializeField] private FloatGameEvent _loadStartEvent;

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        GameTime.isPaused = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        _loadStartEvent.Raise(0.0f);
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        GameTime.isPaused = false;
    }
}
