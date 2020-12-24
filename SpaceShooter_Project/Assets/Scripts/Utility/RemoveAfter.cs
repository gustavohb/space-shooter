using UnityEngine;

public class RemoveAfter : MonoBehaviour
{
    [SerializeField]
    private float _removeAfter = 1.0f;

    private void Update()
    {
        _removeAfter -= GameTime.deltaTime;

        if (_removeAfter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
