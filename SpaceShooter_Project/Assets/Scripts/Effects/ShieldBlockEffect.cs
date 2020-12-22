using UnityEngine;

public class ShieldBlockEffect : MonoBehaviour, IShotEffect
{
    public void Setup(Color color)
    {
        SpriteRenderer spriteRenderer = transform.Find("FieldShieldImage").GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }
}
