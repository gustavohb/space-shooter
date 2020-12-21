using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    GameObject _gameObject;
    Transform _transform;

    protected GameObject _GameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = this.gameObject;
            }
            return _gameObject;
        }
    }

    protected Transform _Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = transform;
            }
            return _transform;
        }
    }

    protected virtual void Awake()
    {
        if (this != Instance)
        {
            GameObject obj = gameObject;
            Destroy(this);
            Destroy(obj);
            return;
        }
    }
}
