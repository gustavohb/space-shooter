using UnityEngine;

public class SelfDisable : MonoBehaviour
{
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
