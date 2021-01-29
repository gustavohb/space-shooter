using UnityEngine;

public class ShopItem
{
    public enum ShopItemType
    {
        //Pickups
        None,
        Bomb,
        Time,
        Repair,
        Void,
        // Shot
        Shot_1,
        Shot_2,
        Shot_3,
        // Speed
        Speed_1,
        Speed_2,
        Speed_3,
        // Health
        Health_1,
        Health_2,
        Health_3,
        Health_4,
        // Shield
        Shield_1,
        Shield_2,
        Shield_3,
        Shield_4,
    }

    public static ItemCost GetCost(ShopItemType itemType)
    {
#if PLATFORM_WEBGL
        switch (itemType)
        {
            default:
            case ShopItemType.None: return new ItemCost { coins = 0, stars = 0 };

            // Pickups
            case ShopItemType.Bomb: return new ItemCost { coins = 100, stars = 1 };
            case ShopItemType.Time: return new ItemCost { coins = 50, stars = 0 };
            case ShopItemType.Repair: return new ItemCost { coins = 125, stars = 0 };
            case ShopItemType.Void: return new ItemCost { coins = 70, stars = 0 };
            // Shot
            case ShopItemType.Shot_1: return new ItemCost { coins = 50, stars = 1 };
            case ShopItemType.Shot_2: return new ItemCost { coins = 100, stars = 1 };
            case ShopItemType.Shot_3: return new ItemCost { coins = 150, stars = 1 };
            // Speed
            case ShopItemType.Speed_1: return new ItemCost { coins = 50, stars = 0 };
            case ShopItemType.Speed_2: return new ItemCost { coins = 100, stars = 0 };
            case ShopItemType.Speed_3: return new ItemCost { coins = 150, stars = 0 };
            // Health
            case ShopItemType.Health_1: return new ItemCost { coins = 50, stars = 0 };
            case ShopItemType.Health_2: return new ItemCost { coins = 100, stars = 0 };
            case ShopItemType.Health_3: return new ItemCost { coins = 150, stars = 0 };
            case ShopItemType.Health_4: return new ItemCost { coins = 200, stars = 0 };
            // Shield
            case ShopItemType.Shield_1: return new ItemCost { coins = 50, stars = 0 };
            case ShopItemType.Shield_2: return new ItemCost { coins = 100, stars = 0 };
            case ShopItemType.Shield_3: return new ItemCost { coins = 150, stars = 0 };
            case ShopItemType.Shield_4: return new ItemCost { coins = 200, stars = 0 };
        }

#endif 

        switch (itemType)
        {
            default:
            case ShopItemType.None: return new ItemCost { coins = 0, stars = 0 };

            // Pickups
            case ShopItemType.Bomb: return new ItemCost { coins = 400, stars = 1 };
            case ShopItemType.Time: return new ItemCost { coins = 250, stars = 0 };
            case ShopItemType.Repair: return new ItemCost { coins = 500, stars = 0 };
            case ShopItemType.Void: return new ItemCost { coins = 350, stars = 0 };
            // Shot
            case ShopItemType.Shot_1: return new ItemCost { coins = 400, stars = 1 };
            case ShopItemType.Shot_2: return new ItemCost { coins = 800, stars = 1 };
            case ShopItemType.Shot_3: return new ItemCost { coins = 1200, stars = 2 };
            // Speed
            case ShopItemType.Speed_1: return new ItemCost { coins = 250, stars = 0 };
            case ShopItemType.Speed_2: return new ItemCost { coins = 450, stars = 0 };
            case ShopItemType.Speed_3: return new ItemCost { coins = 650, stars = 0 };
            // Health
            case ShopItemType.Health_1: return new ItemCost { coins = 0, stars = 0 };
            case ShopItemType.Health_2: return new ItemCost { coins = 350, stars = 0 };
            case ShopItemType.Health_3: return new ItemCost { coins = 650, stars = 0 };
            case ShopItemType.Health_4: return new ItemCost { coins = 950, stars = 0 };
            // Shield
            case ShopItemType.Shield_1: return new ItemCost { coins = 300, stars = 0 };
            case ShopItemType.Shield_2: return new ItemCost { coins = 600, stars = 0 };
            case ShopItemType.Shield_3: return new ItemCost { coins = 900, stars = 0 };
            case ShopItemType.Shield_4: return new ItemCost { coins = 1200, stars = 0 };
        }
    }

    public static Sprite GetSprite(ShopItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ShopItemType.None: return null;
            // Pickups
            case ShopItemType.Bomb: return GameAssets.Instance.bombItemShopSprite;
            case ShopItemType.Void: return GameAssets.Instance.voidItemShopSprite;
            case ShopItemType.Time: return GameAssets.Instance.slowmoItemShopSprite;
            case ShopItemType.Repair: return GameAssets.Instance.repairItemShopSprite;
            // Shot
            case ShopItemType.Shot_1: return GameAssets.Instance.shot1ItemShopSprite;
            case ShopItemType.Shot_2: return GameAssets.Instance.shot2ItemShopSprite;
            case ShopItemType.Shot_3: return GameAssets.Instance.shot3ItemShopSprite;
            // Speed        
            case ShopItemType.Speed_1: return GameAssets.Instance.speed1ItemShopSprite;
            case ShopItemType.Speed_2: return GameAssets.Instance.speed2ItemShopSprite;
            case ShopItemType.Speed_3: return GameAssets.Instance.speed3ItemShopSprite;
            // Health
            case ShopItemType.Health_1: return GameAssets.Instance.health1ItemShopSprite;
            case ShopItemType.Health_2: return GameAssets.Instance.health2ItemShopSprite;
            case ShopItemType.Health_3: return GameAssets.Instance.health3ItemShopSprite;
            case ShopItemType.Health_4: return GameAssets.Instance.health4ItemShopSprite;
            // Shield
            case ShopItemType.Shield_1: return GameAssets.Instance.shield1ItemShopSprite;
            case ShopItemType.Shield_2: return GameAssets.Instance.shield2ItemShopSprite;
            case ShopItemType.Shield_3: return GameAssets.Instance.shield2ItemShopSprite;
            case ShopItemType.Shield_4: return GameAssets.Instance.shield2ItemShopSprite;
        }
    }

    public struct ItemCost
    {
        public int coins;
        public int stars;
    }

}
