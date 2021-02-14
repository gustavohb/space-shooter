using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        BulletForParticles.Init();
        GameTime.isPaused = false;
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
