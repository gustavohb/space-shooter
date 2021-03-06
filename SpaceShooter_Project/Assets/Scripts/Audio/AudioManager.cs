﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SoundLibrary))]
public class AudioManager : SingletonMonoBehavior<AudioManager>
{

    public enum AudioChannel { Sfx, Music };

    public bool musicOn { get; private set; }
    public bool sfxOn { get; private set; }

    public float musicVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }

    [SerializeField] private float _lowPitchRange = 0.95f;
    [SerializeField] private float _highPitchRange = 1.05f;
    [SerializeField] private GameObject _audioSourcePrefab;
    [SerializeField] private float _slowTimePitch = 3.0f;

    private SoundLibrary _library;

    private AudioSource _sfx2DSource;
    private AudioSource[] _musicSources;
    private int _activeMusicSourceIndex;
    private Transform _audioListenerTransform;
    private Transform _playerTransform;
    public int currentTrack = 0;
    public bool slowTime = false;

    protected override void Awake()
    {
        base.Awake();

        _library = GetComponent<SoundLibrary>();

        musicVolumePercent = PlayerPrefs.GetFloat("MusicVolume", 1);

        sfxVolumePercent = PlayerPrefs.GetFloat("SfxVolume", 1);

        _audioListenerTransform = FindObjectOfType<AudioListener>().transform;

        _musicSources = new AudioSource[2];
        for (int i = 0; i < _musicSources.Length; i++)
        {
            GameObject newMusicSource = new GameObject("MusicSource " + (i + 1));
            _musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            _musicSources[i].loop = true;
            newMusicSource.transform.parent = transform;

        }

        GameObject newSfx2DSource = new GameObject("2DSfxSource");
        _sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
        newSfx2DSource.transform.parent = transform;

        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        sfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;


        if (musicOn)
        {
            SetVolume(musicVolumePercent, AudioChannel.Music);
        }
        else
        {
            SetVolume(0.0f, AudioChannel.Music);
        }


        if (sfxOn)
        {
            SetVolume(1.0f, AudioChannel.Sfx);
        }
        else
        {
            SetVolume(0.0f, AudioChannel.Sfx);
        }

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            _audioListenerTransform.position = _playerTransform.position;
        }
        else
        {
            _audioListenerTransform.position = Vector3.zero;
        }
    }

    /// <summary>
    /// Get audio source object from object pool.
    /// </summary>
    protected AudioSource GetAudioSource(Vector3 position, bool forceInstantiate = false)
    {
        if (_audioSourcePrefab == null)
        {
            Debug.LogWarning("Cannot generate a audio source because AudioSourcePrefab is not set.");
            return null;
        }

        // get Bullet GameObject from ObjectPool
        var goAudioSource = ObjectPool.Instance.GetGameObject(_audioSourcePrefab, position, Quaternion.identity, forceInstantiate);
        if (goAudioSource == null)
        {
            Debug.LogError("Failed to pool audio source prefab.");
            return null;

        }

        // get or add Bullet component
        var audioSource = goAudioSource.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Adding audio source component");
            audioSource = goAudioSource.AddComponent<AudioSource>();
        }


        return audioSource;
    }

    public void MuteMusic(bool mute)
    {
        if (mute)
        {

            musicOn = false;
            _musicSources[0].Stop();
            _musicSources[1].Stop();

            SetVolume(0.0f, AudioChannel.Music);

            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            musicOn = true;

            _musicSources[_activeMusicSourceIndex].Play();

            SetVolume(1.0f, AudioChannel.Music);

            PlayerPrefs.SetInt("MusicOn", 1);

        }

        PlayerPrefs.Save();
    }

    public void MuteSfx(bool mute)
    {
        if (mute)
        {
            sfxOn = false;

            SetVolume(0.0f, AudioChannel.Sfx);

            PlayerPrefs.SetInt("SfxOn", 0);
        }
        else
        {
            sfxOn = true;

            SetVolume(1.0f, AudioChannel.Sfx);

            PlayerPrefs.SetInt("SfxOn", 1);
        }

        PlayerPrefs.Save();
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Sfx:
                sfxOn = volumePercent > 0;
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicOn = volumePercent > 0;
                musicVolumePercent = volumePercent;
                break;
        }

        _musicSources[0].volume = musicVolumePercent;
        _musicSources[1].volume = musicVolumePercent;

        _sfx2DSource.volume = sfxVolumePercent;

        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.SetInt("SfxOn", sfxOn ? 1 : 0);

        PlayerPrefs.SetFloat("MusicVolume", musicVolumePercent);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolumePercent);
        PlayerPrefs.Save();
    }


    public void PlayMusic(AudioClip clip, float fadeDuration = 1.0f)
    {
        _activeMusicSourceIndex = 1 - _activeMusicSourceIndex;
        _musicSources[_activeMusicSourceIndex].volume = 0.0f;
        _musicSources[_activeMusicSourceIndex].clip = clip;

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void StopMusic()
    {
        _musicSources[_activeMusicSourceIndex].Stop();
    }

    public void PlaySound2D(SoundLibrary.Sound soundType)
    {
        AudioClip audioClip = _library.GetClipFromType(soundType);
        if (audioClip != null)
        {
            PlaySound2D(audioClip);
        }
    }

    public void PlaySound2D(AudioClip clip)
    {
        _sfx2DSource.PlayOneShot(clip);
    }

    public void RandomizeSfx(Vector3 position, params AudioClip[] clips)
    {
        if (slowTime)
        {
            _sfx2DSource.pitch = _slowTimePitch;
        }
        else
        {
            float randomPitch = Random.Range(_lowPitchRange, _highPitchRange);
            _sfx2DSource.pitch = randomPitch;
        }

        int randomIndex = Random.Range(0, clips.Length);

        _sfx2DSource.transform.position = position;

        _sfx2DSource.PlayOneShot(clips[randomIndex]);

    }

    public void PlaySound(SoundLibrary.Sound soundType, Vector3 position)
    {
        if (soundType == SoundLibrary.Sound.None) return;

        AudioClip audioClip = GetAudioClip(soundType);

        AudioSource audioSource = GetAudioSource(position);

        if (slowTime)
        {
            audioSource.pitch = _slowTimePitch;
        }
        else
        {
            float randomPitch = Random.Range(_lowPitchRange, _highPitchRange);
            audioSource.pitch = randomPitch;
        }

        audioSource.PlayOneShot(audioClip, sfxVolumePercent);

        StartCoroutine(ReleaseAudioSource(audioSource, audioClip.length));

    }

    private IEnumerator ReleaseAudioSource(AudioSource audioSource, float releaseTime)
    {
        yield return new WaitForSeconds(releaseTime + 0.1f);
        if (audioSource != null)
        {
            ObjectPool.Instance.ReleaseGameObject(audioSource.gameObject);
        }
        
    }

    public AudioClip GetAudioClip(SoundLibrary.Sound soundType)
    {
        return _library.GetClipFromType(soundType);
    }

    private IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        _musicSources[_activeMusicSourceIndex].volume = 0.0f;

        _musicSources[_activeMusicSourceIndex].Play();

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;

            _musicSources[_activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent, percent);
            _musicSources[1 - _activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent, 0, percent);

            yield return new WaitForEndOfFrame();
        }

        _musicSources[1 - _activeMusicSourceIndex].Stop();
    }

}
