using UnityEngine;

public class PauseScreen : ExtendedCustomMonoBehavior
{

    private LevelLoader _levelLoader;

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        GameTime.isPaused = true;
    }

    private void Start()
    {
        _levelLoader = FindObjectOfType<LevelLoader>();
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
        _levelLoader?.LoadStart();
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        GameTime.isPaused = false;
    }
}
