using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinAmountText;

    [SerializeField] private TextMeshProUGUI _starAmountText;

    [SerializeField] private IntVariable _coins = default;

    [SerializeField] private IntVariable _stars = default;

    [SerializeField] private Transform _container;

    [SerializeField] private Button _add100CoinsButton;

    [SerializeField] private GameEvent _changeDataGameEvent = default;

    [SerializeField] private Transform _shopItemTemplate;

    [SerializeField] private Material _textMaterialRed;

    private void Awake()
    {
#if UNITY_EDITOR
        _add100CoinsButton.gameObject.SetActive(true);
#else
        _add100CoinsButton.gameObject.SetActive(false);
#endif
    }

    private void OnEnable()
    {
        RefreshUI();
        _changeDataGameEvent.AddListener(RefreshUI);
    }

    private void Start()
    {
        RefreshUI();
    }

    public void Add100Coins()
    {
        _coins.Value += 100;
        RefreshUI();
    }

    private void RefreshUI()
    {
        _coinAmountText.text = _coins.Value.ToString();
        _starAmountText.text = _stars.Value.ToString();

        if (_container == null)
        {
            return;
        }

        for (int i = 0; i < _container.childCount; i++)
        {
            GameObject toRemove = _container.GetChild(i).gameObject;
            Destroy(toRemove);
        }

        switch (GameDataController.GetPlayerHealthLevel())
        {
            case PlayerHealthShield.HealthLevel.Level_1:
                CreateItemButton(ShopItem.ShopItemType.Health_2, ShopItem.GetSprite(ShopItem.ShopItemType.Health_2), "HEALTH [LEVEL 2]", "Increases player's health by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Health_2));
                break;
            case PlayerHealthShield.HealthLevel.Level_2:
                CreateItemButton(ShopItem.ShopItemType.Health_3, ShopItem.GetSprite(ShopItem.ShopItemType.Health_3), "HEALTH [LEVEL 3]", "Increases player's health by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Health_3));
                break;
            case PlayerHealthShield.HealthLevel.Level_3:
                CreateItemButton(ShopItem.ShopItemType.Health_4, ShopItem.GetSprite(ShopItem.ShopItemType.Health_4), "HEALTH [LEVEL 4]", "Increases player's health by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Health_4));
                break;
        }

        switch (GameDataController.GetPlayerShieldArmor())
        {
            case PlayerHealthShield.ShieldArmor.None:
                CreateItemButton(ShopItem.ShopItemType.Shield_1, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_1), "SHIELD [LEVEL 1]", "Adds shield to the player.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_1));
                break;
            case PlayerHealthShield.ShieldArmor.Tier_1:
                CreateItemButton(ShopItem.ShopItemType.Shield_2, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_2), "SHIELD [LEVEL 2]", "Increases player's shield by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_2));
                break;
            case PlayerHealthShield.ShieldArmor.Tier_2:
                CreateItemButton(ShopItem.ShopItemType.Shield_3, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_3), "SHIELD [LEVEL 3]", "Increases player's shield by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_3));
                break;
            case PlayerHealthShield.ShieldArmor.Tier_3:
                CreateItemButton(ShopItem.ShopItemType.Shield_4, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_4), "SHIELD [LEVEL 4]", "Increases player's shield by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_4));
                break;
        }

        switch (GameDataController.GetSpeedLevel())
        {
            case 0:
                CreateItemButton(ShopItem.ShopItemType.Speed_1, ShopItem.GetSprite(ShopItem.ShopItemType.Speed_1), "SPEED [LEVEL 2]", "Increases player's speed by 10%.", ShopItem.GetCost(ShopItem.ShopItemType.Speed_1));
                break;
            case 1:
                CreateItemButton(ShopItem.ShopItemType.Speed_2, ShopItem.GetSprite(ShopItem.ShopItemType.Speed_2), "SPEED [LEVEL 3]", "Increases player's speed by 10%.", ShopItem.GetCost(ShopItem.ShopItemType.Speed_2));
                break;
            case 2:
                CreateItemButton(ShopItem.ShopItemType.Speed_3, ShopItem.GetSprite(ShopItem.ShopItemType.Speed_3), "SPEED [LEVEL 4]", "Increases player's speed by 10%.", ShopItem.GetCost(ShopItem.ShopItemType.Speed_3));
                break;
        }


        switch (GameDataController.GetShotLevel())
        {
            case 0:
                CreateItemButton(ShopItem.ShopItemType.Shot_1, ShopItem.GetSprite(ShopItem.ShopItemType.Shot_1), "SHOT [LEVEL 2]", "Increases player's shot by 1.", ShopItem.GetCost(ShopItem.ShopItemType.Shot_1));
                break;
            case 1:
                CreateItemButton(ShopItem.ShopItemType.Shot_2, ShopItem.GetSprite(ShopItem.ShopItemType.Shot_2), "SHOT [LEVEL 3]", "Increases player's shot by 1.", ShopItem.GetCost(ShopItem.ShopItemType.Shot_2));
                break;
        }


#if UNITY_ANDROID
        if (!GameDataController.IsBombAbilityEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Bomb, ShopItem.GetSprite(ShopItem.ShopItemType.Bomb), "BOMB", "Gives player ability to spawn a bomb to damage close enemies.", ShopItem.GetCost(ShopItem.ShopItemType.Bomb));
        }
#endif

        if (!GameDataController.IsTimePickupEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Time, ShopItem.GetSprite(ShopItem.ShopItemType.Time), "SLOWMO [PICKUP]", "Snipe your enemies one by one in an awesome slowmo sequence.", ShopItem.GetCost(ShopItem.ShopItemType.Time));
        }


        if (!GameDataController.IsVoidPickupEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Void, ShopItem.GetSprite(ShopItem.ShopItemType.Void), "VOID [PICKUP]", "Gives player ability to spawn a void vortex to attract and damage enemies.", ShopItem.GetCost(ShopItem.ShopItemType.Void));
        }

        if (!GameDataController.IsRepairPickupEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Repair, ShopItem.GetSprite(ShopItem.ShopItemType.Repair), "REPAIR [PICKUP]", "Repairs player's spaceship to full health and shield.", ShopItem.GetCost(ShopItem.ShopItemType.Repair));
        }


    }

    private void CreateItemButton(ShopItem.ShopItemType itemType, Sprite itemSprite, string itemName, string itemDescription, ShopItem.ItemCost itemCost)
    {
        Transform shopItemTransform = Instantiate(_shopItemTemplate, _container);
        shopItemTransform.SetParent(_container);
        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName;
        shopItemTransform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = itemDescription;

        shopItemTransform.Find("ItemCoinValue").GetComponent<TextMeshProUGUI>().text = itemCost.coins.ToString();
        shopItemTransform.Find("ItemStarValue").GetComponent<TextMeshProUGUI>().text = itemCost.stars.ToString();

        if (itemCost.stars == 0)
        {
            shopItemTransform.Find("ItemStarValue").gameObject.SetActive(false);
            shopItemTransform.Find("StarIcon").gameObject.SetActive(false);
        }


        if (itemCost.coins > _coins.Value || itemCost.stars > _stars.Value)
        {

            shopItemTransform.Find("ItemCoinValue").GetComponent<TextMeshProUGUI>().fontMaterial = _textMaterialRed;
            shopItemTransform.Find("ItemStarValue").GetComponent<TextMeshProUGUI>().fontMaterial = _textMaterialRed;
            shopItemTransform.Find("BuyButton").GetComponent<Button>().interactable = false;
        }

        shopItemTransform.Find("IconImage").GetComponent<Image>().sprite = itemSprite;
        shopItemTransform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            // Clicked on shop item button
            TryBuyItem(itemType);
        });

        //shopItemTransform.gameObject.SetActive(true);
    }

    private void TryBuyItem(ShopItem.ShopItemType itemType)
    {
        ShopItem.ItemCost itemCost = ShopItem.GetCost(itemType);

        if (_coins.Value >= itemCost.coins && _stars.Value >= itemCost.stars)
        {
            BoughtItem(itemType);
            _coins.Value -= itemCost.coins;
            _stars.Value -= itemCost.stars;
        }
    }


    private void BoughtItem(ShopItem.ShopItemType itemType)
    {
        switch (itemType)
        {
            // Pickups
            case ShopItem.ShopItemType.Bomb: GameDataController.EnableBombAbility(); break;
            case ShopItem.ShopItemType.Void: GameDataController.EnableVoidPickup(); break;
            case ShopItem.ShopItemType.Time: GameDataController.EnableTimePickup(); break;
            case ShopItem.ShopItemType.Repair: GameDataController.EnableRepairPickup(); break;

            // Health
            case ShopItem.ShopItemType.Health_2: GameDataController.SetPlayerHealthLevel(2); break;
            case ShopItem.ShopItemType.Health_3: GameDataController.SetPlayerHealthLevel(3); break;
            case ShopItem.ShopItemType.Health_4: GameDataController.SetPlayerHealthLevel(4); break;

            // Shield
            case ShopItem.ShopItemType.Shield_1: GameDataController.SetPlayerShieldArmor(1); break;
            case ShopItem.ShopItemType.Shield_2: GameDataController.SetPlayerShieldArmor(2); break;
            case ShopItem.ShopItemType.Shield_3: GameDataController.SetPlayerShieldArmor(3); break;
            case ShopItem.ShopItemType.Shield_4: GameDataController.SetPlayerShieldArmor(4); break;

            // Shot
            case ShopItem.ShopItemType.Shot_1: GameDataController.SetShotLevel(1); break;
            case ShopItem.ShopItemType.Shot_2: GameDataController.SetShotLevel(2); break;
            case ShopItem.ShopItemType.Shot_3: GameDataController.SetShotLevel(3); break;

            // Speed
            case ShopItem.ShopItemType.Speed_1: GameDataController.SetSpeedLevel(1); break;
            case ShopItem.ShopItemType.Speed_2: GameDataController.SetSpeedLevel(2); break;
            case ShopItem.ShopItemType.Speed_3: GameDataController.SetSpeedLevel(3); break;
        }
    }

    private void OnDisable()
    {
        _changeDataGameEvent.RemoveListener(RefreshUI);
    }

}


/*


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_CoinAmountText;

    [SerializeField]
    private TextMeshProUGUI m_StarAmountText;

    [SerializeField]
    private Transform m_Container;

    [SerializeField]
    private Transform m_ShopItemTemplate;

    private IShopCustomer shopCustomer;

    //private PlayerData m_PlayerData;
    //[SerializeField]
    //private Player player;

    [SerializeField]
    private Material m_TextMaterialRed;

    //private PlayerData playerData;

    private SaveData saveData;

    [SerializeField]
    private Button m_Add100CoinsButton;

    [SerializeField]
    private Button m_Add1StarButton;

    private void Awake()
    {
        saveData = GameDataController.saveData;

        GameDataController.OnSaveDataChanged += GameDataController_OnSaveDataChanged;

        RefreshDisplay();

#if UNITY_EDITOR
        m_Add100CoinsButton.gameObject.SetActive(true);
        m_Add1StarButton.gameObject.SetActive(true);
#else
        m_Add100CoinsButton.gameObject.SetActive(false);
        m_Add1StarButton.gameObject.SetActive(false);
#endif

    }

    private void GameDataController_OnSaveDataChanged(object sender, System.EventArgs e)
    {
        RefreshDisplay();
    }

    private void SaveSystem_OnPlayerDataChanged(object sender, System.EventArgs e)
    {
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (m_Container == null)
        {
            return;
        }

        for (int i = 0; i < m_Container.childCount; i++)
        {
            GameObject toRemove = m_Container.GetChild(i).gameObject;
            Destroy(toRemove);
        }

        m_CoinAmountText.text = GameDataController.GetCoins().ToString();
        m_StarAmountText.text = GameDataController.GetStars().ToString();


       


        switch (GameDataController.GetPlayerHealthLevel())
        {
            case PlayerHealthShield.HealthLevel.Level_1:
                CreateItemButton(ShopItem.ShopItemType.Health_2, ShopItem.GetSprite(ShopItem.ShopItemType.Health_2), "HEALTH [LEVEL 2]", "Increases player's health by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Health_2));
                break;
            case PlayerHealthShield.HealthLevel.Level_2:
                CreateItemButton(ShopItem.ShopItemType.Health_3, ShopItem.GetSprite(ShopItem.ShopItemType.Health_3), "HEALTH [LEVEL 3]", "Increases player's health by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Health_3));
                break;
            case PlayerHealthShield.HealthLevel.Level_3:
                CreateItemButton(ShopItem.ShopItemType.Health_4, ShopItem.GetSprite(ShopItem.ShopItemType.Health_4), "HEALTH [LEVEL 4]", "Increases player's health by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Health_4));
                break;
        }

        switch (GameDataController.GetPlayerShieldArmor())
        {
            case PlayerHealthShield.ShieldArmor.None:
                CreateItemButton(ShopItem.ShopItemType.Shield_1, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_1), "SHIELD [LEVEL 1]", "Adds shield to the player.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_1));
                break;
            case PlayerHealthShield.ShieldArmor.Tier_1:
                CreateItemButton(ShopItem.ShopItemType.Shield_2, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_2), "SHIELD [LEVEL 2]", "Increases player's shield by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_2));
                break;
            case PlayerHealthShield.ShieldArmor.Tier_2:
                CreateItemButton(ShopItem.ShopItemType.Shield_3, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_3), "SHIELD [LEVEL 3]", "Increases player's shield by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_3));
                break;
            case PlayerHealthShield.ShieldArmor.Tier_3:
                CreateItemButton(ShopItem.ShopItemType.Shield_4, ShopItem.GetSprite(ShopItem.ShopItemType.Shield_4), "SHIELD [LEVEL 4]", "Increases player's shield by 25 points.", ShopItem.GetCost(ShopItem.ShopItemType.Shield_4));
                break;
        }

        switch (GameDataController.GetSpeedLevel())
        {
            case 0:
                CreateItemButton(ShopItem.ShopItemType.Speed_1, ShopItem.GetSprite(ShopItem.ShopItemType.Speed_1), "SPEED [LEVEL 2]", "Increases player's speed by 10%.", ShopItem.GetCost(ShopItem.ShopItemType.Speed_1));
                break;
            case 1:
                CreateItemButton(ShopItem.ShopItemType.Speed_2, ShopItem.GetSprite(ShopItem.ShopItemType.Speed_2), "SPEED [LEVEL 3]", "Increases player's speed by 10%.", ShopItem.GetCost(ShopItem.ShopItemType.Speed_2));
                break;
            case 2:
                CreateItemButton(ShopItem.ShopItemType.Speed_3, ShopItem.GetSprite(ShopItem.ShopItemType.Speed_3), "SPEED [LEVEL 4]", "Increases player's speed by 10%.", ShopItem.GetCost(ShopItem.ShopItemType.Speed_3));
                break;
        }


        switch (GameDataController.GetShotLevel())
        {
            case 0:
                CreateItemButton(ShopItem.ShopItemType.Shot_1, ShopItem.GetSprite(ShopItem.ShopItemType.Shot_1), "SHOT [LEVEL 2]", "Increases player's shot by 1.", ShopItem.GetCost(ShopItem.ShopItemType.Shot_1));
                break;
            case 1:
                CreateItemButton(ShopItem.ShopItemType.Shot_2, ShopItem.GetSprite(ShopItem.ShopItemType.Shot_2), "SHOT [LEVEL 3]", "Increases player's shot by 1.", ShopItem.GetCost(ShopItem.ShopItemType.Shot_2));
                break;
        }


#if UNITY_ANDROID
        if (!GameDataController.IsBombAbilityEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Bomb, ShopItem.GetSprite(ShopItem.ShopItemType.Bomb), "BOMB", "Gives player ability to spawn a bomb to damage close enemies.", ShopItem.GetCost(ShopItem.ShopItemType.Bomb));
        }
#endif

        if (!GameDataController.IsTimePickupEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Time, ShopItem.GetSprite(ShopItem.ShopItemType.Time), "SLOWMO [PICKUP]", "Snipe your enemies one by one in an awesome slowmo sequence.", ShopItem.GetCost(ShopItem.ShopItemType.Time));
        }


        if (!GameDataController.IsVoidPickupEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Void, ShopItem.GetSprite(ShopItem.ShopItemType.Void), "VOID [PICKUP]", "Gives player ability to spawn a void vortex to attract and damage enemies.", ShopItem.GetCost(ShopItem.ShopItemType.Void));
        }

        if (!GameDataController.IsRepairPickupEnable())
        {
            CreateItemButton(ShopItem.ShopItemType.Repair, ShopItem.GetSprite(ShopItem.ShopItemType.Repair), "REPAIR [PICKUP]", "Repairs player's spaceship to full health and shield.", ShopItem.GetCost(ShopItem.ShopItemType.Repair));
        }

    }

    private void OnEnable()
{
    RefreshDisplay();
}

private void CreateItemButton(ShopItem.ShopItemType itemType, Sprite itemSprite, string itemName, string itemDescription, ShopItem.ItemCost itemCost)
{
    Transform shopItemTransform = Instantiate(m_ShopItemTemplate, m_Container);
    shopItemTransform.SetParent(m_Container);
    shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName;
    shopItemTransform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = itemDescription;

    shopItemTransform.Find("ItemCoinValue").GetComponent<TextMeshProUGUI>().text = itemCost.coins.ToString();
    shopItemTransform.Find("ItemStarValue").GetComponent<TextMeshProUGUI>().text = itemCost.stars.ToString();

    if (itemCost.stars == 0)
    {
        shopItemTransform.Find("ItemStarValue").gameObject.SetActive(false);
        shopItemTransform.Find("StarIcon").gameObject.SetActive(false);
    }


    if (itemCost.coins > GameDataController.GetCoins() || itemCost.stars > GameDataController.GetStars())
    {

        shopItemTransform.Find("ItemCoinValue").GetComponent<TextMeshProUGUI>().fontMaterial = m_TextMaterialRed;
        shopItemTransform.Find("ItemStarValue").GetComponent<TextMeshProUGUI>().fontMaterial = m_TextMaterialRed;

        //shopItemTransform.Find("ItemCoinValue").GetComponent<TextMeshProUGUI>().color = new Color32(197, 8, 8, 255);
        //shopItemTransform.Find("ItemStarValue").GetComponent<TextMeshProUGUI>().color = new Color32(197, 8, 8, 255);
        shopItemTransform.Find("BuyButton").GetComponent<Button>().interactable = false;
    }

    shopItemTransform.Find("IconImage").GetComponent<Image>().sprite = itemSprite;
    shopItemTransform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() =>
    {
        // Clicked on shop item button
        TryBuyItem(itemType);
    });

    //shopItemTransform.gameObject.SetActive(true);
}

private void TryBuyItem(ShopItem.ShopItemType itemType)
{
    ShopItem.ItemCost itemCost = ShopItem.GetCost(itemType);

    if (GameDataController.saveData.coins >= itemCost.coins && GameDataController.saveData.stars >= itemCost.stars)
    {
        BoughtItem(itemType);
        GameDataController.SetCoins(GameDataController.saveData.coins - itemCost.coins);
        GameDataController.SetStars(GameDataController.saveData.stars - itemCost.stars);
    }
}


private void BoughtItem(ShopItem.ShopItemType itemType)
{
    switch (itemType)
    {
        // Pickups
        case ShopItem.ShopItemType.Bomb: GameDataController.EnableBombAbility(); break;
        //case ShopItem.ShopItemType.Bomb: playerData.isBombAbilityEnabled = true; break;
        case ShopItem.ShopItemType.Void: GameDataController.EnableVoidPickup(); break;
        //case ShopItem.ShopItemType.Void: playerData.isVoidPickupEnabled = true; break;
        case ShopItem.ShopItemType.Time: GameDataController.EnableTimePickup(); break;
        //case ShopItem.ShopItemType.Time: playerData.isTimePickupEnabled = true; break;
        case ShopItem.ShopItemType.Repair: GameDataController.EnableRepairPickup(); break;

        // Health
        case ShopItem.ShopItemType.Health_2: GameDataController.SetPlayerHealthLevel(2); break;
        case ShopItem.ShopItemType.Health_3: GameDataController.SetPlayerHealthLevel(3); break;
        case ShopItem.ShopItemType.Health_4: GameDataController.SetPlayerHealthLevel(4); break;

        // Shield
        case ShopItem.ShopItemType.Shield_1: GameDataController.SetPlayerShieldArmor(1); break;
        case ShopItem.ShopItemType.Shield_2: GameDataController.SetPlayerShieldArmor(2); break;
        case ShopItem.ShopItemType.Shield_3: GameDataController.SetPlayerShieldArmor(3); break;
        case ShopItem.ShopItemType.Shield_4: GameDataController.SetPlayerShieldArmor(4); break;

        // Shot
        case ShopItem.ShopItemType.Shot_1: GameDataController.SetShotLevel(1); break;
        case ShopItem.ShopItemType.Shot_2: GameDataController.SetShotLevel(2); break;
        case ShopItem.ShopItemType.Shot_3: GameDataController.SetShotLevel(3); break;

        // Speed
        case ShopItem.ShopItemType.Speed_1: GameDataController.SetSpeedLevel(1); break;
        case ShopItem.ShopItemType.Speed_2: GameDataController.SetSpeedLevel(2); break;
        case ShopItem.ShopItemType.Speed_3: GameDataController.SetSpeedLevel(3); break;
    }
}

public void Add100Coins()
{
    GameDataController.SetCoins(GameDataController.GetCoins() + 100);
}

public void Add1Star()
{
    GameDataController.SetStars(GameDataController.GetStars() + 1);
}

}



*/