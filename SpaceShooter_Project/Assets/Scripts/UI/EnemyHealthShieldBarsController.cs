using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EnemyHealthShieldBarsController : MonoBehaviour
{
    [SerializeField] private EnemyHealthShieldBarsUI _healthShieldBarsPrefab;

    [SerializeField] private EnemyHealthShieldBarsUI _bossHealthShieldBarsPrefab;

    private Dictionary<EnemyHealthShield, EnemyHealthShieldBarsUI> enemiesHealthShieldBars = new Dictionary<EnemyHealthShield, EnemyHealthShieldBarsUI>();

    private void Awake()
    {
        EnemyHealthShield.OnHealthShieldAdded += AddHealthShieldBars;
        EnemyHealthShield.OnHealthShieldRemoved += RemoveHealthShieldBars;
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

    private void OnDestroy()
    {
        EnemyHealthShield.OnHealthShieldAdded -= AddHealthShieldBars;
        EnemyHealthShield.OnHealthShieldRemoved -= RemoveHealthShieldBars;
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
}
