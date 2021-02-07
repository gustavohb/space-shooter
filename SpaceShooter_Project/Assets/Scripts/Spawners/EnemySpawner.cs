using System.Collections;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";

    [SerializeField] private string _enemyTag = "Enemy";

    [SerializeField] private float _startDelay = 1f;

    [SerializeField] private Rect _spawnArea = new Rect(-21, -12, 21, 12);

    [SerializeField] private WaveSO[] _waves;

    [SerializeField] private GameObject _victoryScreen;

    [SerializeField] private IntVariable _levelToLoad = default;

    private Transform _playerTransform;

    private string _currentWaveName;

    public event System.Action<string> OnNewWave;

    private int _enemiesRemainingAlive;

    private int _currentEnemiesAlive;

    private bool _waveIsCleared = false;

    private int _lastLevelReached;
        

    IEnumerator Start()
    {
        _lastLevelReached = PlayerPrefs.GetInt("levelReached", 0);

        GameObject playerGameObject = GameObject.FindGameObjectWithTag(_playerTag);

        if (playerGameObject)
        {
            _playerTransform = playerGameObject.transform;
        }

        _waves[_levelToLoad.Value].waveStartDelay = 0;

        yield return new WaitForSeconds(_startDelay);

        //To fix:
        _levelToLoad.Value -= 1;
        WaveCleared();

    }


    private void NextWave()
    {
        foreach (Spawn spawn in _waves[_levelToLoad.Value].spawns)
        {
            _enemiesRemainingAlive += spawn.count;
        }
    }

    private void Update()
    {
        if (_playerTransform == null)
        {
            return;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _enemiesRemainingAlive = 0;
            WaveCleared();
        }
#endif
        if (!_waveIsCleared)
        {
            if (_levelToLoad.Value >= 0 && _levelToLoad.Value < _waves.Length)
            {
                // Go through all the spawn objects in a wave, and count their spawn time
                foreach (Spawn spawn in _waves[_levelToLoad.Value].spawns)
                {
                    if (spawn.spawnDelayTimer > 0)
                    {
                        spawn.spawnDelayTimer -= GameTime.deltaTime;
                    }
                    else
                    {
                        // Count spawn time
                        spawn.spawnTimer += GameTime.deltaTime;

                        // Immediately spawn the first enemy
                        if (spawn.enemiesRemainingToSpawn == spawn.count)
                        {
                            SpawnNewEnemy(spawn.spawnObject);

                            spawn.enemiesRemainingToSpawn--;
                        }
                        else if (spawn.enemiesRemainingToSpawn > 0 && _currentEnemiesAlive < spawn.maxEnemiesAlive)
                        {
                            if (spawn.spawnTimer >= spawn.spawnTime)
                            {
                                spawn.spawnTimer = 0;

                                SpawnNewEnemy(spawn.spawnObject);

                                spawn.enemiesRemainingToSpawn--;
                            }
                        }
                    }
                }

                // Count down the total spawn time
                _waves[_levelToLoad.Value].spawnTimeTemp -= GameTime.deltaTime;
            }
        }
    }

    private void SpawnNewEnemy(GameObject spawnObject)
    {
        // Spawn a new enemy
        GameObject newEnemy = Instantiate(spawnObject);

        IDamageable damageable = newEnemy.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.OnDeath += EnemySpawner_OnDeath;
        }

        BossBattle bossBattle = newEnemy.GetComponent<BossBattle>();

        if (bossBattle != null)
        {
            bossBattle.OnBossDeath += EnemySpawner_OnDeath;
        }

        Vector3 newEnemyPosition = newEnemy.transform.position;

        // Place it at a random position outside of the spawn area
        if (Random.value > 0.5f)
        {
            if (Random.value > 0.5f)
            {
                newEnemyPosition.x = -_spawnArea.x + 2;
            }
            else
            {
                newEnemyPosition.x = _spawnArea.x - 2;
            }


            newEnemyPosition.y = Random.Range(-_spawnArea.y, _spawnArea.y);
            newEnemy.transform.position = newEnemyPosition;
        }
        else
        {
            if (Random.value > 0.5f)
                newEnemyPosition.y = -_spawnArea.y + 2;
            else
                newEnemyPosition.y = _spawnArea.y - 2;

            newEnemyPosition.x = Random.Range(-_spawnArea.x, _spawnArea.x);
            newEnemy.transform.position = newEnemyPosition;
        }

        _currentEnemiesAlive++;

    }

    private void EnemySpawner_OnDeath(object sender, System.EventArgs e)
    {
        _enemiesRemainingAlive--;
        _currentEnemiesAlive--;
        if (_enemiesRemainingAlive == 0)
        {
            WaveCleared();
        }
    }

    public void WaveCleared()
    {
        _waveIsCleared = true;

        _levelToLoad.Value++;

        if (_lastLevelReached < _levelToLoad.Value)
        {
            _lastLevelReached = _levelToLoad.Value;
            PlayerPrefs.SetInt("levelReached", _lastLevelReached);
            PlayerPrefs.Save();

        }

        if (_levelToLoad.Value < _waves.Length)
        {
            StartCoroutine(StartWave());
        }
        else
        {
            StartCoroutine(Victory());
        }
    }

    /// Starts a new wave, displaying a message, and calculating the time for each enemy to spawn
    public IEnumerator StartWave()
    {
        yield return new WaitForSeconds(_waves[_levelToLoad.Value].waveStartDelay);

        _currentWaveName = _waves[_levelToLoad.Value].waveMessage;

        OnNewWave?.Invoke(_currentWaveName);

        if (_waves[_levelToLoad.Value].spawnInSequence == true)
        {
            _waves[_levelToLoad.Value].spawnTimeTemp = 0;

            foreach (Spawn spawn in _waves[_levelToLoad.Value].spawns)
            {
                spawn.spawnDelay = _waves[_levelToLoad.Value].spawnTimeTemp;

                _waves[_levelToLoad.Value].spawnTimeTemp += spawn.spawnTime * spawn.count;

                spawn.enemiesRemainingToSpawn = spawn.count;
            }
        }
        else
        {
            _waves[_levelToLoad.Value].spawnTimeTemp = _waves[_levelToLoad.Value].spawnTime;

            foreach (Spawn spawn in _waves[_levelToLoad.Value].spawns)
            {
                spawn.spawnDelayTimer = spawn.spawnDelay;
                spawn.spawnTime = _waves[_levelToLoad.Value].spawnTime / spawn.count;

                spawn.enemiesRemainingToSpawn = spawn.count;
            }

            NextWave();
        }
        _waveIsCleared = false;
    }

    private IEnumerator Victory()
    {
        yield return new WaitForSeconds(1.0f);

        if (_victoryScreen != null)
        {
            _victoryScreen.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.y, 0), new Vector3(_spawnArea.width, _spawnArea.y, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.height, 0), new Vector3(_spawnArea.width, _spawnArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.y, 0), new Vector3(_spawnArea.x, _spawnArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.width, _spawnArea.y, 0), new Vector3(_spawnArea.width, _spawnArea.height, 0));
    }
}
