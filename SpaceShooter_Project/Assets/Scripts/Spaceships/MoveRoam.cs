using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IMovePosition))]
public class MoveRoam : MonoBehaviour
{

    [SerializeField] private Rect _moveArea = new Rect(-14.0f, -8.0f, 14.0f, 6.0f);
    [SerializeField] private float _waypointTime = 5f;

    public UnityEvent OnMovingFinished;

    private float _waypointTimer;
    private Vector3 _targetMovePosition;
    private int _currentQuadrant;
    private IMovePosition _movePosition;
    private bool _onMovingFinishedInvonked = false;

    private void Awake()
    {
        _movePosition = GetComponent<IMovePosition>();
    }

    private void Start()
    {
        SetRandomMovePosition();
    }

    private void SetRandomMovePosition()
    {

        int newQuadrant;

        do
        {
            newQuadrant = Random.Range(0, 3);
        }
        while (_currentQuadrant == newQuadrant);

        _currentQuadrant = newQuadrant;

        Vector3 newMovePosition = Vector3.zero;

        switch (_currentQuadrant)
        {
            case 0:
                newMovePosition.x = Random.Range(0, _moveArea.width);
                newMovePosition.y = Random.Range(0, _moveArea.height);
                break;
            case 1:
                newMovePosition.x = Random.Range(_moveArea.x, 0);
                newMovePosition.y = Random.Range(0, _moveArea.height);
                break;
            case 2:
                newMovePosition.x = Random.Range(_moveArea.x, 0);
                newMovePosition.y = Random.Range(_moveArea.y, 0);
                break;
            case 3:
                newMovePosition.x = Random.Range(0, _moveArea.width);
                newMovePosition.y = Random.Range(_moveArea.y, 0);
                break;
        }


        _targetMovePosition = newMovePosition;
    }

    private void Update()
    {
        

        float arrivedAtPositionDistance = 1f;

        if (Vector3.Distance(transform.position, _targetMovePosition) < arrivedAtPositionDistance && !_onMovingFinishedInvonked)
        {
            // Reached position
            OnMovingFinished?.Invoke();
            _onMovingFinishedInvonked = true;
            _waypointTimer = _waypointTime;
        }
        else
        {
            SetMovePosition(_targetMovePosition);
        }


        if (_onMovingFinishedInvonked)
        {
            _waypointTimer -= Time.deltaTime;

            if (_waypointTimer <= 0f)
            {
                MoveToNewPosition();
            }
        }
    }

    public void MoveToNewPosition()
    {
        SetRandomMovePosition();
        _onMovingFinishedInvonked = false;
    }

    private void SetMovePosition(Vector3 movePosition)
    {
        _movePosition.SetMovePosition(movePosition);
    }

    public void OnDrawGizmosSelected()
    {

        // Draw waipoint area
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(_moveArea.x, _moveArea.y, 0), new Vector3(_moveArea.width, _moveArea.y, 0));
        Gizmos.DrawLine(new Vector3(_moveArea.x, _moveArea.height, 0), new Vector3(_moveArea.width, _moveArea.height, 0));
        Gizmos.DrawLine(new Vector3(_moveArea.x, _moveArea.y, 0), new Vector3(_moveArea.x, _moveArea.height, 0));
        Gizmos.DrawLine(new Vector3(_moveArea.width, _moveArea.y, 0), new Vector3(_moveArea.width, _moveArea.height, 0));
    }

}
