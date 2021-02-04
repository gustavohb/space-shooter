using UnityEngine;

public class ItemAssets : SingletonMonoBehavior<ItemAssets>
{

    public Sprite voidInventoryImage;
    public Sprite slowmoInventoryImage;
    public Sprite repairInventoryImage;

    public Sprite voidItemWorldImage;
    public Sprite slowmoItemWorldImage;
    public Sprite repairItemWorldImage;

    public Sprite voidItemIcon;
    public Sprite slowmoItemIcon;
    public Sprite repairItemIcon;

    public Transform itemWorldPrefab;

    public Color voidItemColor = Color.magenta;
    public Color slowmoItemColor = Color.magenta;
    public Color repairItemColor = Color.magenta;
}
