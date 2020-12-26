using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingletonMonoBehavior<ObjectPool>
{
    private List<int> _pooledKeyList = new List<int>();
    private Dictionary<int, List<GameObject>> _pooledGODic = new Dictionary<int, List<GameObject>>();


    protected override void Awake()
    {
        base.Awake();
    }

    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation, bool forceInstantiate = false)
    {
        if (prefab == null)
        {
            return null;
        }

        int key = prefab.GetInstanceID();

        if (_pooledKeyList.Contains(key) == false && _pooledGODic.ContainsKey(key) == false)
        {
            _pooledKeyList.Add(key);
            _pooledGODic.Add(key, new List<GameObject>());
        }

        List<GameObject> goList = _pooledGODic[key];
        GameObject go = null;

        if (forceInstantiate == false)
        {
            for (int i = goList.Count - 1; i >= 0; i--)
            {
                go = goList[i];
                if (go == null)
                {
                    goList.Remove(go);
                    continue;
                }
                if (go.activeSelf == false)
                {
                    // Found free GameObject in object pool.
                    Transform goTransform = go.transform;
                    goTransform.position = position;
                    goTransform.rotation = rotation;
                    go.SetActive(true);
                    return go;
                }
            }
        }

        // Instantiate because there is no free GameObject in object pool.
        go = (GameObject)Instantiate(prefab, position, rotation);
        go.transform.parent = _Transform;
        goList.Add(go);

        return go;
    }

    public void ReleaseGameObject(GameObject go, bool destroy = false)
    {
        if (destroy)
        {
            Destroy(go);
            return;
        }
        go.SetActive(false);
    }

    public int GetActivePooledObjectCount()
    {
        int cnt = 0;
        for (int i = 0; i < _pooledKeyList.Count; i++)
        {
            int key = _pooledKeyList[i];
            var goList = _pooledGODic[key];
            for (int j = 0; j < goList.Count; j++)
            {
                var go = goList[j];
                if (go != null && go.activeInHierarchy)
                {
                    cnt++;
                }
            }
        }
        return cnt;
    }

}


