using UnityEngine;

public class ExtendedCustomMonoBehavior : MonoBehaviour
{
    Transform _transform;
    Rigidbody2D _rigidbody2D;

    public new Transform transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = GetComponent<Transform>();
            }
            return _transform;
        }
    }

    public new Rigidbody2D rigidbody2D
    {
        get
        {
            if (_rigidbody2D == null)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }
            return _rigidbody2D;
        }
    }

    public Vector3 GetPosition()
    {
        return _transform.position;
    }
}
