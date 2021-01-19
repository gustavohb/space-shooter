using UnityEngine;

public class BossShotController : ExtendedCustomMonoBehavior
{

    [Header("Shooting")]

    [SerializeField]
    private ShotController[] _shotControllers;

    private Transform _target;

    private float _rotationTarget;

    private bool _isShooting = false;

    private bool _enableShotController = false;

    private int _shotControllerIndex = 0;

    private ShotController _currentShotController;

    private Vector3 _startPosition;

    BossBattle m_BossBattle;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _currentShotController = _shotControllers[0];
    }

    
    public void SetBossBattle(BossBattle bossBattle)
    {
        m_BossBattle = bossBattle;
        m_BossBattle.OnStageChanged += BossController_OnStageChanged;
    }

    private void BossController_OnStageChanged(object sender, BossBattle.Stage e)
    {

        StopShot();

        switch (e)
        {
            case BossBattle.Stage.Stage_1:
                _shotControllerIndex = 0;
                break;
            case BossBattle.Stage.Stage_2:
                _shotControllerIndex = 1;
                break;
            case BossBattle.Stage.Stage_3:
                _shotControllerIndex = 2;
                break;
            case BossBattle.Stage.Stage_4:
                _shotControllerIndex = 3;
                break;
        }

        _currentShotController?.StopAllCoroutines();
        _currentShotController = _shotControllers[_shotControllerIndex];
    }

    public void StartShot()
    {
        _currentShotController.StartShotRoutine();
    }


    public void StopShot()
    {
        if (_currentShotController != null)
        {
            foreach (ShotController shotController in _shotControllers)
            {
                shotController.StopShotRoutine();
            }
            _isShooting = false;
        }
    }
}
