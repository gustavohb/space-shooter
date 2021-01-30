using UnityEngine;
using ScriptableObjectArchitecture;

public class GameDataController : MonoBehaviour
{
    private static SaveData saveData;

    public static GameEvent changeDataGameEvent = default;

    [SerializeField] private GameEvent _changeDataGameEvent = default;


    [SerializeField] private IntVariable _coinAmount = default;
    [SerializeField] private IntVariable _starAmount = default;

    private static IntVariable s_coinAmount = default;
    private static IntVariable s_starAmount = default;



    private void Awake()
    {
        changeDataGameEvent = _changeDataGameEvent;
        s_coinAmount = _coinAmount;
        s_starAmount = _starAmount;

        LoadData();
    }

    [ContextMenu("Save Data")]
    public static void SaveGame()
    {
        saveData.coins = s_coinAmount.Value;
        saveData.stars = s_starAmount.Value;

        var data = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("GameData", data);
        PlayerPrefs.Save();
    }

    [ContextMenu("Load Data")]
    public static void LoadData()
    {
        string data = PlayerPrefs.GetString("GameData", "");
        if (data.CompareTo("") == 0)
        {
            saveData = new SaveData();
        }
        else
        {
            saveData = JsonUtility.FromJson<SaveData>(data);
        }

        s_coinAmount.Value = saveData.coins;
        s_starAmount.Value = saveData.stars;

    }

    private void OnDisable()
    {
        SaveGame();
    }

    public static bool IsVoidPickupEnable()
    {
        return saveData.isVoidPickupEnable;
    }

    public static bool IsTimePickupEnable()
    {
        return saveData.isTimePickupEnable;
    }

    public static bool IsRepairPickupEnable()
    {
        return saveData.isRepairPickupEnable;
    }

    public static bool IsBombAbilityEnable()
    {
        return saveData.isBombAbilityEnable;
    }

    public static PlayerHealthShield.HealthLevel GetPlayerHealthLevel()
    {
        switch (saveData.healthLevel)
        {
            default:
            case 1:
                return PlayerHealthShield.HealthLevel.Level_1;
            case 2:
                return PlayerHealthShield.HealthLevel.Level_2;
            case 3:
                return PlayerHealthShield.HealthLevel.Level_3;
            case 4:
                return PlayerHealthShield.HealthLevel.Level_4;
        }
    }

    public static int GetPlayerShieldArmorInt()
    {
        return saveData.shieldArmor;
    }

    public static int GetPlayerHealthLevelInt()
    {
        return saveData.healthLevel;
    }

    public static PlayerHealthShield.ShieldArmor GetPlayerShieldArmor()
    {
        switch (saveData.shieldArmor)
        {
            default:
            case 0:
            //return PlayerHealthShield.ShieldArmor.None;
            case 1:
                return PlayerHealthShield.ShieldArmor.Tier_1;
            case 2:
                return PlayerHealthShield.ShieldArmor.Tier_2;
            case 3:
                return PlayerHealthShield.ShieldArmor.Tier_3;
            case 4:
                return PlayerHealthShield.ShieldArmor.Tier_4;
        }
    }

    public static int GetSpeedLevel()
    {
        return saveData.speedLevel;
    }

    public static int GetShotLevel()
    {
        return saveData.shotLevel;
    }

    public static void EnableBombAbility()
    {
        saveData.isBombAbilityEnable = true;

        SaveGame();

        changeDataGameEvent?.Raise();
    }

    public static void EnableVoidPickup()
    {
        saveData.isVoidPickupEnable = true;
        SaveGame();
    }

    public static void EnableTimePickup()
    {
        saveData.isTimePickupEnable = true;
        SaveGame();



        changeDataGameEvent?.Raise();
    }

    public static void EnableRepairPickup()
    {
        saveData.isRepairPickupEnable = true;
        SaveGame();

        changeDataGameEvent?.Raise();
    }

    public static void SetSpeedLevel(int level)
    {
        saveData.speedLevel = level;
        SaveGame();

        changeDataGameEvent?.Raise();
    }

    public static void SetShotLevel(int level)
    {
        saveData.shotLevel = level;

        SaveGame();

        changeDataGameEvent?.Raise();
    }

    public static void SetPlayerHealthLevel(int level)
    {
        saveData.healthLevel = level;

        SaveGame();

        changeDataGameEvent?.Raise();
    }

    public static void SetPlayerShieldArmor(int level)
    {
        saveData.shieldArmor = level;

        SaveGame();

        changeDataGameEvent?.Raise();
    }

    public static void EreaseSaveData()
    {
        PlayerPrefs.SetInt("levelToLoad", 0);

        s_coinAmount.Value = 0;
        s_starAmount.Value = 0;

        saveData = new SaveData();
        SaveGame();

        changeDataGameEvent?.Raise();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
