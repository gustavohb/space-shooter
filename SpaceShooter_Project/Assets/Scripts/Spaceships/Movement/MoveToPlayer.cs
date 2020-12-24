using UnityEngine;

[RequireComponent(typeof(IMovePosition))]
public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";

    [SerializeField] private float _minDistanceToTarget = 1.2f;

    private Transform _playerTransform;

    private IMovePosition _movePosition;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag(_playerTag)?.transform;
        _movePosition = GetComponent<IMovePosition>();
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            Vector3 dirToTarget = (_playerTransform.position - transform.position).normalized;
            Vector3 targetPosition = _playerTransform.position - dirToTarget * (_minDistanceToTarget);
            SetMovePosition(targetPosition);
        }
    }

    private void SetMovePosition(Vector3 movePosition)
    {
        _movePosition.SetMovePosition(movePosition);
    }

}
