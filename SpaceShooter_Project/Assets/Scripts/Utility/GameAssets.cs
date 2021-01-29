using UnityEngine;

public class GameAssets : SingletonMonoBehavior<GameAssets>
{
    [Header("Pickups")]
    public Sprite bombItemShopSprite;
    public Sprite voidItemShopSprite;
    public Sprite slowmoItemShopSprite;
    public Sprite repairItemShopSprite;

    [Header("Shot")]
    public Sprite shot1ItemShopSprite;
    public Sprite shot2ItemShopSprite;
    public Sprite shot3ItemShopSprite;

    [Header("Speed")]
    public Sprite speed1ItemShopSprite;
    public Sprite speed2ItemShopSprite;
    public Sprite speed3ItemShopSprite;

    [Header("Health")]
    public Sprite health1ItemShopSprite;
    public Sprite health2ItemShopSprite;
    public Sprite health3ItemShopSprite;
    public Sprite health4ItemShopSprite;

    [Header("Shield")]
    public Sprite shield1ItemShopSprite;
    public Sprite shield2ItemShopSprite;
    public Sprite shield3ItemShopSprite;
    public Sprite shield4ItemShopSprite;

    [Header("Shot Effects")]
    public GameObject shotHitEffectPrefab;
    public GameObject shieldHitEffectPrefab;
    public GameObject shieldEffectPrefab;

    [Header("Others")]

    public GameObject itemVoid;
    public GameObject itemTime;
    public GameObject voidEffectPrefab;



}
