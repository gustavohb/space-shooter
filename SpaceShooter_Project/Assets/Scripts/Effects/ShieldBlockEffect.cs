using UnityEngine;

public class ShieldBlockEffect : ExtendedCustomMonoBehavior, IShotEffect
{
    public void Setup(Color color)
    {
        SpriteRenderer spriteRenderer = transform.Find("FieldShieldImage").GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

}
