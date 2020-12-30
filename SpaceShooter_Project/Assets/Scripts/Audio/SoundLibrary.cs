using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public enum Sound
    {
        None,
        Shot,
        Hit,
        ShieldHit,
        NormalExplosion,
        BigExplosion,
        MeleeAttack,
        EnemyBuzzsawAttack,
        WaveBannerAppear,
        WaveBannerDisappear,
        Victory,
        ClickButton01,
        OrbPickup,
        UIPickupOrb,
        CoinPickup,
        UIPickupCoin,
        StarPickup,
        UIStarPickup,
    };

    [System.Serializable]
    public class SoundGroup
    {
        public Sound soundType;
        public AudioClip[] sounds;
    }

    public SoundGroup[] soundGroups;

    private Dictionary<Sound, AudioClip[]> _groupDictionary = new Dictionary<Sound, AudioClip[]>();

    private void Awake()
    {
        foreach (SoundGroup soundGroup in soundGroups)
        {
            _groupDictionary.Add(soundGroup.soundType, soundGroup.sounds);
        }
    }

    public AudioClip GetClipFromType(Sound soundType)
    {
        if (_groupDictionary.ContainsKey(soundType))
        {
            AudioClip[] sounds = _groupDictionary[soundType];
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }
}
