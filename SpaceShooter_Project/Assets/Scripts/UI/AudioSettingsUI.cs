
using UnityEngine;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject _soundOnIcon;

    [SerializeField] private GameObject _soundOffIcon;

    [SerializeField] private GameObject _musicOnIcon;

    [SerializeField] private GameObject _musicOffIcon;

    private void Start()
    {
        UpdateMusicIcon();
        UpdateSoundIcon();
    }

    public void ToggleSound()
    {
        AudioManager.Instance.MuteSfx(AudioManager.Instance.sfxOn);
        UpdateSoundIcon();
    }

    private void UpdateSoundIcon()
    {
        if (AudioManager.Instance.sfxOn)
        {
            _soundOnIcon.SetActive(true);
            _soundOffIcon.SetActive(false);
        }
        else
        {
            _soundOnIcon.SetActive(false);
            _soundOffIcon.SetActive(true);
        }
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.MuteMusic(AudioManager.Instance.musicOn);
        UpdateMusicIcon();
    }

    private void UpdateMusicIcon()
    {
        if (AudioManager.Instance.musicOn)
        {
            _musicOnIcon.SetActive(true);
            _musicOffIcon.SetActive(false);
        }
        else
        {
            _musicOnIcon.SetActive(false);
            _musicOffIcon.SetActive(true);
        }
    }
}
