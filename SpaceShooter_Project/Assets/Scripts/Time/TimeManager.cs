using UnityEngine;

public class TimeManager : SingletonMonoBehavior<TimeManager>
{
    public float frameCount { get; private set; }


    [SerializeField] private float _slowDownFactor = 0.2f;

    [SerializeField] private float _slowdownLength = 7.5f;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (GameTime.isPaused)
        {
            AudioManager.Instance.slowTime = false;
            return;
        }

        frameCount++;
        Time.timeScale += (1f / _slowdownLength) * Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (Time.timeScale >= 1f && AudioManager.Instance.slowTime == true)
        {
            Time.fixedDeltaTime = 0.02f;
            AudioManager.Instance.slowTime = false;
        }
    }

    public void DoSlowmotion()
    {
        Time.timeScale = _slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        AudioManager.Instance.slowTime = true;
    }
}

