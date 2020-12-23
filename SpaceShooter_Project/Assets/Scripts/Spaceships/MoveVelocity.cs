using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveVelocity : ExtendedCustomMonoBehavior, IMoveVelocity
{
    [SerializeField]
    private float _moveSpeed = 10f;

    private Vector3 _velocityVector;

    private bool _isMovementEnable = true;

    public void DisableMovement()
    {
        _isMovementEnable = false;
    }


    public void DisableMovement(float time)
    {
        _isMovementEnable = false;
        Invoke("EnableMovement", time);
    }

    public void EnableMovement()
    {
        _isMovementEnable = true;
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        _velocityVector = velocityVector;
    }

    private void FixedUpdate()
    {
        if(_isMovementEnable)
        {
            rigidbody2D.velocity = _velocityVector * _moveSpeed;
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }
}
