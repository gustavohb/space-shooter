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

    [SerializeField] private Button _add1StarButton;

    [SerializeField] private GameEvent _changeDataGameEvent = default;

    [SerializeField] private Transform _shopItemTemplate;

    [SerializeField] private Material _textMaterialRed;

    private void Awake()
    {
#if UNITY_EDITOR
        _add100CoinsButton.gameObject.SetActive(true);
        _add1StarButton.gameObject.SetActive(true);
#else
        _add100CoinsButton.gameObject.SetActive(false);
        _add1StarButton.gameObject.SetActive(false);
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

    public void Add1Star()
    {
        _stars.Value += 1;
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
    }

    private void TryBuyItem(ShopItem.ShopItemType itemType)
    {
        ShopItem.ItemCost itemCost = ShopItem.GetCost(itemType);

        if (_coins.Value >= itemCost.coins && _stars.Value >= itemCost.stars)
        {
            BoughtItem(itemType);
            _coins.Value -= itemCost.coins;
            _stars.Value -= itemCost.stars;
            _changeDataGameEvent.Raise();
            AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ClickButton02);
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

