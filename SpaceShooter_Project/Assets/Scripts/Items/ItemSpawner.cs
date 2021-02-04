using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnMaxTime = 400.0f;
    [SerializeField] private float _spawnMinTime = 100.0f;
    [SerializeField] private Rect _spawnArea = new Rect(-18, -10, 18, 10);
    [SerializeField] private float _spawnMinPlayerDistance = 5.0f;
    [SerializeField] Transform _joystickTransform;
    [SerializeField] private float _spawnMinJoystickDistance = 5.0f;

    private float _spawnMinJoystickSqrtDistance;
    private List<Item> _itemsToSpawn = new List<Item>();
    private int _itemIndex = 0;
    private float _spawnTime = 400.0f;
    private float _spawnTimer = 0.0f;
    private float _spawnMinPlayerSqrtDistance;

    private void Start()
    {
        _spawnTime = _spawnMaxTime;

        _spawnMinPlayerSqrtDistance = Mathf.Pow(_spawnMinPlayerDistance, 2);
        _spawnMinJoystickSqrtDistance = Mathf.Pow(_spawnMinJoystickDistance, 2);

        if (GameDataController.IsVoidPickupEnable())
        {
            Item newItem = new Item();
            newItem.amount = 1;
            newItem.itemType = Item.ItemType.Void;

            _itemsToSpawn.Add(newItem);

            _spawnTime -= _spawnMinTime;
        }

        if (GameDataController.IsTimePickupEnable())
        {
            Item newItem = new Item();
            newItem.amount = 1;
            newItem.itemType = Item.ItemType.Slowmo;
            _itemsToSpawn.Add(newItem);

            _spawnTime -= _spawnMinTime;
        }

        if (GameDataController.IsRepairPickupEnable())
        {
            Item newItem = new Item();
            newItem.amount = 1;
            newItem.itemType = Item.ItemType.Repair;
            _itemsToSpawn.Add(newItem);

            _spawnTime -= _spawnMinTime;
        }

        if (_spawnTime < _spawnMinTime)
        {
            _spawnTime = _spawnMinTime;
        }

    }

    private void Update()
    {
        if (_itemsToSpawn.Count > 0)
        {
            _spawnTimer += GameTime.deltaTime;

            if (_spawnTimer >= _spawnTime)
            {
                StartCoroutine(SpawnItem());
                _spawnTimer = 0.0f;
            }
        }
    }

    private IEnumerator SpawnItem()
    {
        yield return null;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            yield break;
        }

        Vector3 spawnPosition = Vector3.zero;

        float sqrtDstToPlayer;
        float srqtDstToJoyStick;
        do
        {
            if (playerTransform == null)
            {
                yield break;
            }

            spawnPosition.x = Random.Range(_spawnArea.x, _spawnArea.width);
            spawnPosition.y = Random.Range(_spawnArea.y, _spawnArea.height);

            sqrtDstToPlayer = (spawnPosition - playerTransform.position).sqrMagnitude;
            srqtDstToJoyStick = (spawnPosition - _joystickTransform.position).sqrMagnitude;

        } while (sqrtDstToPlayer < _spawnMinPlayerSqrtDistance || srqtDstToJoyStick < _spawnMinJoystickSqrtDistance);

        Item duplicateItem = new Item { itemType = _itemsToSpawn[_itemIndex].itemType, amount = _itemsToSpawn[_itemIndex].amount };
        ItemWorld.SpawnItemWorld(spawnPosition, duplicateItem);

        _itemIndex = (_itemIndex + 1) % _itemsToSpawn.Count;

        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.y, 0), new Vector3(_spawnArea.width, _spawnArea.y, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.height, 0), new Vector3(_spawnArea.width, _spawnArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.x, _spawnArea.y, 0), new Vector3(_spawnArea.x, _spawnArea.height, 0));
        Gizmos.DrawLine(new Vector3(_spawnArea.width, _spawnArea.y, 0), new Vector3(_spawnArea.width, _spawnArea.height, 0));
    }

}
