using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(CanvasGroup))]
public class EnemyHealthShieldBarsController : MonoBehaviour
{
    [SerializeField] private EnemyHealthShieldBarsUI _healthShieldBarsPrefab;

    [SerializeField] private EnemyHealthShieldBarsUI _bossHealthShieldBarsPrefab;

    [SerializeField] private GameEvent _playerDeathEvent = default;

    private Dictionary<EnemyHealthShield, EnemyHealthShieldBarsUI> enemiesHealthShieldBars = new Dictionary<EnemyHealthShield, EnemyHealthShieldBarsUI>();

    private CanvasGroup _canvasGroup;



    private void Awake()
    {
        EnemyHealthShield.OnHealthShieldAdded += AddHealthShieldBars;
        EnemyHealthShield.OnHealthShieldRemoved += RemoveHealthShieldBars;
        _canvasGroup = GetComponent<CanvasGroup>();

        _playerDeathEvent.AddListener(HideHealthBars);

    }

    private void AddHealthShieldBars(EnemyHealthShield healthShield)
    {
        if (healthShield.IsBoss)
        {
            EnemyHealthShieldBarsUI healthShieldBars = Instantiate(_bossHealthShieldBarsPrefab, transform);
            healthShieldBars.SetHealthShield(healthShield); 
            enemiesHealthShieldBars.Add(healthShield, healthShieldBars);
        }
        else
        {
            EnemyHealthShieldBarsUI healthShieldBars = Instantiate(_healthShieldBarsPrefab, transform);
            healthShieldBars.SetHealthShield(healthShield); 
            enemiesHealthShieldBars.Add(healthShield, healthShieldBars);
        }
    }

    private void RemoveHealthShieldBars(EnemyHealthShield healthShield)
    {
        if (enemiesHealthShieldBars.ContainsKey(healthShield))
        {
            if (enemiesHealthShieldBars[healthShield] != null)
            {
                Destroy(enemiesHealthShieldBars[healthShield].gameObject);
            }
            enemiesHealthShieldBars.Remove(healthShield);
        }
    }

    private void HideHealthBars()
    {
        _canvasGroup.alpha = 0.0f;
    }

    private void OnDestroy()
    {
        EnemyHealthShield.OnHealthShieldAdded -= AddHealthShieldBars;
        EnemyHealthShield.OnHealthShieldRemoved -= RemoveHealthShieldBars;

        _playerDeathEvent.RemoveListener(HideHealthBars);
    }
}
