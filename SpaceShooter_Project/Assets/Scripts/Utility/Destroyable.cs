using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }
}