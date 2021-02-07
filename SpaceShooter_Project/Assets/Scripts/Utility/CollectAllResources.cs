using UnityEngine;

public class CollectAllResources : MonoBehaviour
{
    private void OnEnable()
    {
        GameResource[] gameResources = FindObjectsOfType<GameResource>();
        foreach (GameResource gameResource in gameResources)
        {
            ResourcesManager.Instance.CollectResource(gameResource);
        }
    }
}
