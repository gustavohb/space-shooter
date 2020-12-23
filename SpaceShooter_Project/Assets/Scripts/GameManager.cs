using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        BulletForParticles.Init();
        MusicManager.Instance.PlayMusic(MusicManager.MusicTheme.Game);
    }
}
