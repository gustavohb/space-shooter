using UnityEngine;

public class ShieldHitEffect : MonoBehaviour, IShotEffect
{
    public void Setup(Color color)
    {
        SpriteRenderer spriteRenderer = transform.Find("Projectile").Find("Glow").GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }
}
