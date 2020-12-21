using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DestroyArea : ExtendedCustomMonoBehavior
{
    void OnTriggerEnter2D(Collider2D c)
    {
        HitCheck(c.transform);
    }

    void HitCheck(Transform colTrans)
    {
        string goName = colTrans.name;
        if (goName.Contains("Bullet"))
        {
            ObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
        }
    }
}
