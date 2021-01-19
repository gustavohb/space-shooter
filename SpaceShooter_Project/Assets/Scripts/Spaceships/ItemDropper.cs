using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(EnemyHealthShield))]
public class ItemDropper : ExtendedCustomMonoBehavior
{
    [SerializeField] private ItemDrop[] _itemDrops;

    private EnemyHealthShield _enemyHealthShield;

    private void Start()
    {
        _enemyHealthShield = GetComponent<EnemyHealthShield>();

        _enemyHealthShield.OnDeath += EnemyHealthShield_OnEnemyDie;
    }

    private void EnemyHealthShield_OnEnemyDie(object sender, System.EventArgs e)
    {
        DropItems();
        _enemyHealthShield.OnDeath -= EnemyHealthShield_OnEnemyDie;
    }

    public void DropItems()
    {
        foreach (ItemDrop itemDrop in _itemDrops)
        {
            if (Random.Range(0f, 1f) <= itemDrop.dropChance)
            {
                int dropRate = Random.Range(itemDrop.dropMin, itemDrop.dropMax + 1);
                for (int i = 0; i < dropRate; i++)
                {
                    if (itemDrop.item != null)
                    {
                        GameObject newItem = Instantiate(itemDrop.item, transform.position, Quaternion.identity);
                        Vector3 randomDir = Util.GetRandomDir();
                        Vector3 newPosition = transform.position + randomDir * itemDrop.dropSpread;
                        newItem.transform.DOMove(newPosition, 0.6f);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        _enemyHealthShield.OnDeath -= EnemyHealthShield_OnEnemyDie;
    }
}
