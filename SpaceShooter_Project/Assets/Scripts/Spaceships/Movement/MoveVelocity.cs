using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveVelocity : ExtendedCustomMonoBehavior, IMoveVelocity
{
    [SerializeField] private float _moveSpeed = 10f;

    private Vector3 _velocityVector;

    private bool _isMovementEnable = true;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

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

        if (_animator != null)
        {
            _animator.SetFloat("DirX", _velocityVector.x);
            _animator.SetFloat("DirY", _velocityVector.y);
        }

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
