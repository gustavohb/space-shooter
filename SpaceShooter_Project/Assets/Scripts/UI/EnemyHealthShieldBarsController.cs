using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EnemyHealthShieldBarsController : MonoBehaviour
{
    [SerializeField]
    private EnemyHealthShieldBars healthShieldBarsPrefab;

    private Dictionary<EnemyHealthShield, EnemyHealthShieldBars> enemiesHealthShieldBars = new Dictionary<EnemyHealthShield, EnemyHealthShieldBars>();


    private void Awake()
    {
        EnemyHealthShield.OnHealthShieldAdded += AddHealthShieldBars;
        EnemyHealthShield.OnHealthShieldRemoved += RemoveHealthShieldBars;
    }

    private void AddHealthShieldBars(EnemyHealthShield healthShield)
    {
        if (enemiesHealthShieldBars.ContainsKey(healthShield) == false)
        {
            EnemyHealthShieldBars healthShieldBars = Instantiate(healthShieldBarsPrefab, transform);
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
