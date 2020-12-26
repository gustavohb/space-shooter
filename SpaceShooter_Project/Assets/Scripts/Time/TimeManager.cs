

public class TimeManager : SingletonMonoBehavior<TimeManager>
{
    public float frameCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (!GameTime.isPaused)
        {
            frameCount++;
        }
    }
}
