using UnityEngine;

public class PlayerMovement : ExtendedCustomMonoBehavior
{

    public float speed = 11.0f;

    [SerializeField] private Rect _moveArea = new Rect(-18.5f, -10f, 18.5f, 10f);

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 direction)
    {

        transform.Translate(direction * speed * GameTime.deltaTime);


        if (animator != null)
        {
            animator.SetFloat("DirX", direction.x);
        }

        Vector3 newPosition = transform.position;

        if (transform.position.x > _moveArea.width)
        {
            newPosition.x = _moveArea.width;
        }

        if (transform.position.x < _moveArea.x)
        {
            newPosition.x = _moveArea.x;
        }

        if (transform.position.y > _moveArea.height)
        {
            newPosition.y = _moveArea.height;
        }

        if (transform.position.y < _moveArea.y)
        {
            newPosition.y = _moveArea.y;
        }

        transform.position = newPosition;

        

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(new Vector3(_moveArea.x, _moveArea.y, 0), new Vector3(_moveArea.width, _moveArea.y, 0));
        Gizmos.DrawLine(new Vector3(_moveArea.x, _moveArea.height, 0), new Vector3(_moveArea.width, _moveArea.height, 0));
        Gizmos.DrawLine(new Vector3(_moveArea.x, _moveArea.y, 0), new Vector3(_moveArea.x, _moveArea.height, 0));
        Gizmos.DrawLine(new Vector3(_moveArea.width, _moveArea.y, 0), new Vector3(_moveArea.width, _moveArea.height, 0));
    }

}
