using UnityEngine;

public class RemoveAllBullets : MonoBehaviour
{

    private void Start()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            ObjectPool.Instance.ReleaseGameObject(bullet.gameObject);
        }
    }
}
