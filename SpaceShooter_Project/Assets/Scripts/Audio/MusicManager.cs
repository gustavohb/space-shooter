using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : SingletonMonoBehavior<MusicManager>
{

    public enum MusicTheme
    {
        Menu,
        Game,
        Boss
    }

    public AudioClip menuTheme;
    public AudioClip mainTheme;
    public AudioClip bossTheme;

    [SerializeField] private float _musicFadeDuration = 0.4f;

    private float _invokeDelay = 0.2f;
    private string _sceneName;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != _sceneName)
        {
            _sceneName = newSceneName;
            Invoke("PlayMusic", _invokeDelay);
        }
    }

    public void PlayMusic(MusicTheme musicTheme)
    {
        AudioClip clipToPlay = null;
        switch (musicTheme)
        {
            case MusicTheme.Menu:
                clipToPlay = mainTheme;
                break;
            case MusicTheme.Game:
                clipToPlay = mainTheme;
                break;
            case MusicTheme.Boss:
                clipToPlay = bossTheme;
                break;
        }

        if (clipToPlay != null)
        {
            AudioManager.Instance.PlayMusic(clipToPlay, _musicFadeDuration);
        }
    }
}