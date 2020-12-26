using UnityEngine;

public class PlaySFX : ExtendedCustomMonoBehavior
{
    [SerializeField] private SoundLibrary.Sound _sfx;

    private void OnEnable()
    {
        AudioManager.Instance.PlaySound(_sfx, transform.position);
    }
}
