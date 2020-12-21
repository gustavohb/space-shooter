using UnityEngine;

[RequireComponent(typeof(IMoveVelocity))]
public class MovePositionDirect : ExtendedCustomMonoBehavior, IMovePosition
{
    private Vector3 _movePosition;

    private IMoveVelocity _moveVelocity;

    private void Start()
    {
        _moveVelocity = GetComponent<IMoveVelocity>();
    }


    public void SetMovePosition(Vector3 movePosition)
    {
        _movePosition = movePosition;
    }

    private void Update()
    {
        Vector3 moveDir = (_movePosition - transform.position).normalized;
        if (Vector3.Distance(_movePosition, transform.position) < 1f)
        {
            moveDir = Vector3.zero; // Stop moving when near

        }

        _moveVelocity.SetVelocity(moveDir);
        RotateTowardTargetPosition();
    }

    private void RotateTowardTargetPosition()
    {
        Vector3 newEulerAngles = transform.eulerAngles;
        newEulerAngles.z = (transform.position.x - _movePosition.x) * 3;
        transform.eulerAngles = newEulerAngles;
    }

}
