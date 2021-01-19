using System;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : ExtendedCustomMonoBehavior
{
    public enum Stage
    {
        WaitingToStart,
        Stage_1,
        Stage_2,
        Stage_3,
        Stage_4
    }

    public event EventHandler<Stage> OnStageChanged;

    public event EventHandler OnBossDeath;

    [SerializeField] private GameObject _enemyStage1Prefab;

    [SerializeField] private int _maxEnemiesAlineStage1 = 3;

    [SerializeField] private GameObject _enemyStage2Prefab;

    [SerializeField] private int _maxEnemiesAlineStage2 = 3;

    [SerializeField] private GameObject _enemyStage3Prefab;

    [SerializeField] private int _maxEnemiesAlineStage3 = 3;

    [SerializeField] private GameObject _enemyStage4Prefab;

    [SerializeField] private float _spawnEnemyTime = 15.0f;

    [SerializeField] private int _maxEnemiesAlineStage4 = 3;

    [SerializeField] private Transform _healthPickupPrefab;

    [SerializeField] private float _healthPickupSpawnTime = 12.0f;

    [SerializeField] private Rect _spawnArea = new Rect(-21, -12, 21, 12);

    private List<Vector3> _spawnPositionList;

    [SerializeField] private GameObject _bossPrefab;

    private EnemyHealthShield _bossHealthShield;

    private Vector3 _bossSpawnPosition;

    private Stage _currentStage;

    private List<GameObject> _enemySpawnList;

    private int maxEnemiesAlive = 3;

    [SerializeField] private Rect _spawnHealthPickupArea = new Rect(-18, -9, 18, 8);

    [SerializeField] private bool m_IsFinalBoss = false;

    private void Awake()
    {
        _spawnPositionList = new List<Vector3>();
        _enemySpawnList = new List<GameObject>();

        foreach (Transform spawnPosition in transform.Find("SpawnPositions"))
        {
            _spawnPositionList.Add(spawnPosition.position);
        }

        _bossSpawnPosition = transform.Find("BossSpawnPosition").position;


        _currentStage = Stage.WaitingToStart;

    }

    private void Start()
    {
        StartBattle();

        transform.position = Vector3.zero;
    }

    private void StartBattle()
    {
        Debug.Log("StartBattle");

        MusicManager.Instance.PlayMusic(MusicManager.MusicTheme.Boss);
        SpawnBoss();

        StartNextStage();

        FunctionPeriodic.Create(SpawnEnemy, _spawnEnemyTime, "enemySpawn");
        FunctionPeriodic.Create(SpawnHealthPickup, _healthPickupSpawnTime, "spawnHealthPickup");
    }

    private void SpawnBoss()
    {
        GameObject bossGO = Instantiate(_bossPrefab, _bossSpawnPosition, Quaternion.identity);
        BossShotController bossController = bossGO.GetComponent<BossShotController>();
        bossController.SetBossBattle(this);
        _bossHealthShield = bossGO.GetComponent<EnemyHealthShield>();
        _bossHealthShield.OnDamaged += BossBattle_OnDamaged;
        _bossHealthShield.OnDeath += BossBattle_OnDeath;
    }

    private void BossBattle_OnDeath(object sender, System.EventArgs e)
    {
        // Boss dead! Boss battle over!
        Debug.Log("Boss Battle Over!");
        FunctionPeriodic.StopAllFunc("enemySpawn");
        FunctionPeriodic.StopAllFunc("spawnHealthPickup");
        FunctionPeriodic.StopAllFunc("spawnShieldPickup");
        DestroyAllEnemies();
        if (!m_IsFinalBoss)
        {
            MusicManager.Instance.PlayMusic(MusicManager.MusicTheme.Game);
        }


        OnBossDeath?.Invoke(this, EventArgs.Empty);
    }

    private void BossBattle_OnDamaged(object sender, System.EventArgs e)
    {
        switch (_currentStage)
        {
            case Stage.Stage_1:
                if (_bossHealthShield.GetShieldPct() <= 0.5f)
                {
                    // Boss under 50% shield
                    StartNextStage();


                    SpawnEnemy();
                    SpawnEnemy();


                }
                break;
            case Stage.Stage_2:
                if (_bossHealthShield.GetShieldPct() <= 0.0f)
                {
                    // Boss with no shield
                    StartNextStage();


                    SpawnEnemy();
                    SpawnEnemy();

                    SpawnHealthPickup();
                    SpawnHealthPickup();
                }
                break;
            case Stage.Stage_3:
                if (_bossHealthShield.GetHealthPct() <= 0.5f)
                {
                    // Boss under 50% health
                    StartNextStage();

                    SpawnShieldPickup();
                    SpawnShieldPickup();

                    SpawnHealthPickup();
                    SpawnHealthPickup();

                    SpawnEnemy();
                    SpawnEnemy();
                }
                break;
        }
    }

    private void StartNextStage()
    {
        switch (_currentStage)
        {
            case Stage.WaitingToStart:
                _currentStage = Stage.Stage_1;
                maxEnemiesAlive = _maxEnemiesAlineStage1;
                break;
            case Stage.Stage_1:
                _currentStage = Stage.Stage_2;
                maxEnemiesAlive = _maxEnemiesAlineStage2;
                break;
            case Stage.Stage_2:
                _currentStage = Stage.Stage_3;
                maxEnemiesAlive = _maxEnemiesAlineStage3;
                break;
            case Stage.Stage_3:
                _currentStage = Stage.Stage_4;
                maxEnemiesAlive = _maxEnemiesAlineStage4;
                break;
        }
        Debug.Log("Starting next stage: " + _currentStage);

        OnStageChanged?.Invoke(this, _currentStage);
    }

    private void SpawnEnemy()
    {
        int aliveCount = 0;
        foreach (GameObject enemySpawnedGO in _enemySpawnList)
        {
            if (enemySpawnedGO != null)
            {
                EnemyHealthShield enemySpawned = enemySpawnedGO.GetComponent<EnemyHealthShield>();
                if (enemySpawned.isAlive)
                {
                    // Enemy alive
                    aliveCount++;

                    if (aliveCount >= maxEnemiesAlive)
                    {
                        // Don't spawn more enemies
                        return;
                    }
                }
            }
        }

        GameObject enemySpawn = null;
        switch (_currentStage)
        {
            case Stage.Stage_1:
                enemySpawn = _enemyStage1Prefab;
                break;
            case Stage.Stage_2:
                enemySpawn = _enemyStage2Prefab;
                break;
            case Stage.Stage_3:
                enemySpawn = _enemyStage3Prefab;
                break;
            case Stage.Stage_4:
                enemySpawn = _enemyStage4Prefab;
                break;
        }

        Vector3 spawnPosition = _spawnPositionList[UnityEngine.Random.Range(0, _spawnPositionList.Count - 1)];

        if (enemySpawn != null)
        {
            GameObject newEnemy = Instantiate(enemySpawn, spawnPosition, Quaternion.identity);

            _enemySpawnList.Add(newEnemy);
        }

    }

    private void SpawnHealthPickup()
    {
        Vector3 spawnPosition = Vector3.zero;
        // Place it at a random position within the spawn area
        spawnPosition.x = UnityEngine.Random.Range(_spawnHealthPickupArea.x, _spawnHealthPickupArea.width);
        spawnPosition.y = UnityEngine.Random.Range(_spawnHealthPickupArea.y, _spawnHealthPickupArea.height);
        Instantiate(_healthPickupPrefab, spawnPosition, Quaternion.identity);
    }


    private void SpawnShieldPickup()
    {
        Vector3 spawnPosition = Vector3.zero;
        // Place it at a random position within the spawn area
        spawnPosition.x = UnityEngine.Random.Range(_spawnHealthPickupArea.x, _spawnHealthPickupArea.width);
        spawnPosition.y = UnityEngine.Random.Range(_spawnHealthPickupArea.y, _spawnHealthPickupArea.height);
    }

    private void DestroyAllEnemies()
    {
        foreach (GameObject enemySpawn in _enemySpawnList)
        {
            if (enemySpawn != null)
            {
                EnemyHealthShield enemyHealthShield = enemySpawn.GetComponent<EnemyHealthShield>();
                if (enemyHealthShield != null && enemyHealthShield.isAlive)
                {
                    enemyHealthShield.Die();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.y, 0), new Vector3(_spawnArea.width, _spawnArea.y, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.height, 0), new Vector3(_spawnArea.width, _spawnArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.y, 0), new Vector3(_spawnArea.x, _spawnArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.width, _spawnArea.y, 0), new Vector3(_spawnArea.width, _spawnArea.height, 0));

        Gizmos.color = Color.green;

        Gizmos.DrawLine(new Vector3(_spawnHealthPickupArea.x, _spawnHealthPickupArea.y, 0), new Vector3(_spawnHealthPickupArea.width, _spawnHealthPickupArea.y, 0));
        Gizmos.DrawLine(new Vector3(_spawnHealthPickupArea.x, _spawnHealthPickupArea.height, 0), new Vector3(_spawnHealthPickupArea.width, _spawnHealthPickupArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnHealthPickupArea.x, _spawnHealthPickupArea.y, 0), new Vector3(_spawnHealthPickupArea.x, _spawnHealthPickupArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnHealthPickupArea.width, _spawnHealthPickupArea.y, 0), new Vector3(_spawnHealthPickupArea.width, _spawnHealthPickupArea.height, 0));
    }

}
