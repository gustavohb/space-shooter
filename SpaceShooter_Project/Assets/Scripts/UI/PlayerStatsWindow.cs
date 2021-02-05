using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class PlayerStatsWindow : ExtendedCustomMonoBehavior
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1.0f;
    private const float DAMAGED_SHIELD_FADE_TIMER_MAX = 1.0f;
    private const int LOW_HEALTH_INDICATOR = 10;
    private const int HEALTH_SEGMENT_COUNT = 4;
    private const int SHIELD_SEGMENT_COUNT = 4;

    private PlayerHealthShield _playerHealthShield;

    [SerializeField] private BoolGameEvent PlayerHealthShieldChangedEvent = default;
    [SerializeField] private GameEvent PlayerHealthShildRepairEvent = default;


    [SerializeField] private FloatVariable _playerHealth;
    [SerializeField] private FloatVariable _playerShield;

    public Color tier1Color = new Color32(37, 255, 249, 255);

    public Color tier2Color = new Color32(37, 255, 249, 255);

    public Color tier3Color = new Color32(37, 255, 249, 255);

    public Color tier4Color = new Color32(37, 255, 249, 255);

    public Color healthBarColor = new Color32(255, 6, 136, 255);

    public Color healthDamageBarColor = new Color32(255, 189, 211, 255);

    public Color flashingHealhtBarColor = new Color32(200, 0, 78, 255);

    public Color shieldDamageBarColor = Color.white;

    private Slider _health1Slider;
    private Slider _health2Slider;
    private Slider _health3Slider;
    private Slider _health4Slider;

    private Slider[] _healthSliderArray;

    private Slider _damageHealth1Slider;
    private Slider _damageHealth2Slider;
    private Slider _damageHealth3Slider;
    private Slider _damageHealth4Slider;

    private Slider[] _damageHealthSliderArray;

    private CanvasGroup _damageHealth1CanvasGroup;
    private CanvasGroup _damageHealth2CanvasGroup;
    private CanvasGroup _damageHealth3CanvasGroup;
    private CanvasGroup _damageHealth4CanvasGroup;

    private CanvasGroup[] _damageHealthCanvasGroupArray;

    private Slider _flashingHealth1Slider;
    private Slider _flashingHealth2Slider;
    private Slider _flashingHealth3Slider;
    private Slider _flashingHealth4Slider;

    private Slider[] _flashingHealthSliderArray;

    private CanvasGroup _flashingHealth1CanvasGroup;
    private CanvasGroup _flashingHealth2CanvasGroup;
    private CanvasGroup _flashingHealth3CanvasGroup;
    private CanvasGroup _flashingHealth4CanvasGroup;

    private CanvasGroup[] _flashingHealthCanvasGroupArray;

    private float _lowHealthAlphaChange;
    private float _damageHealthFadeTimer;

    [SerializeField] private Slider _hiddenHealthSlider;

    

    private Slider _shield1Slider;
    private Slider _shield2Slider;
    private Slider _shield3Slider;
    private Slider _shield4Slider;

    private Slider[] _shieldSliderArray;

    private Slider _damageShield1Slider;
    private Slider _damageShield2Slider;
    private Slider _damageShield3Slider;
    private Slider _damageShield4Slider;

    private Slider[] _damageShieldSliderArray;

    private CanvasGroup _damageShield1CanvasGroup;
    private CanvasGroup _damageShield2CanvasGroup;
    private CanvasGroup _damageShield3CanvasGroup;
    private CanvasGroup _damageShield4CanvasGroup;

    [SerializeField] private Slider _hiddenShieldSlider;

    private CanvasGroup[] _damageShieldCanvasGroupArray;

    private float _damageShieldFadeTimer;

    [SerializeField] private Animator _healthIconAnimator;

    private float[] _damageHealthPreviousHealthAmountArray = new float[HEALTH_SEGMENT_COUNT];

    private float[] _damageShieldPreviousShieldAmountArray = new float[SHIELD_SEGMENT_COUNT];

    [SerializeField] private Animator _healthIconAnimated;

    private float _preChangeHealth;

    private float _preChangeShield;

    [SerializeField]
    private float _updateSpeedSeconds = 0.3f;

    private void Awake()
    {
        _lowHealthAlphaChange = +4f;

        SetUpHealthBars();
        SetUpShieldBars();
    }


    private void Start()
    {
        
        _playerHealthShield = FindObjectOfType<PlayerHealthShield>();
        if (_playerHealthShield != null)
        {
            SetPlayerHealthShield(_playerHealthShield);
            UpdateHealthSegment();
            UpdateShieldSegment();
        }
        
        UpdateHealthSegment();
        UpdateShieldSegment();
    }



    private void Update()
    {


        // Is the damage health slider visible
        if (_damageHealth1CanvasGroup.alpha > 0)
        {
            // Count down fade timer
            _damageHealthFadeTimer -= GameTime.deltaTime;
            if (_damageHealthFadeTimer < 0)
            {
                // Fade timer over, lower alpha
                float newAlpha = _damageHealth1CanvasGroup.alpha;
                newAlpha -= GameTime.deltaTime * 4f;

                // Damage health bar not visible, set size 
                for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
                {
                    _damageHealthCanvasGroupArray[i].alpha = newAlpha;
                }
            }
        }

        // Is the damage shield slider visible
        if (_damageShield1CanvasGroup.alpha > 0)
        {
            // Count down fade timer
            _damageShieldFadeTimer -= GameTime.deltaTime;
            if (_damageShieldFadeTimer < 0)
            {
                // Fade timer over, lower alpha
                float newAlpha = _damageShield1CanvasGroup.alpha;
                newAlpha -= GameTime.deltaTime * 4f;

                // Damage health bar not visible, set size 
                for (int i = 0; i < SHIELD_SEGMENT_COUNT; i++)
                {
                    _damageShieldCanvasGroupArray[i].alpha = newAlpha;
                }
            }
        }


        // Flashing health image;
        if (_flashingHealth1CanvasGroup.gameObject.activeSelf)
        {
            float lowHealthAlpha = _flashingHealth1CanvasGroup.alpha;
            lowHealthAlpha += _lowHealthAlphaChange * GameTime.deltaTime * 1.0f;

            if (lowHealthAlpha > 1f)
            {
                _lowHealthAlphaChange *= -1f;
                lowHealthAlpha = 1f;
            }
            if (lowHealthAlpha < 0f)
            {
                _lowHealthAlphaChange *= -1f;
                lowHealthAlpha = 0f;
            }

            for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
            {
                _flashingHealthCanvasGroupArray[i].alpha = lowHealthAlpha;
            }
        }

    }

    private void SetUpHealthBars()
    {
        var healthBars = transform.Find("HealthBars");

        var healthBar01 = healthBars.Find("Health01");
        _health1Slider = healthBar01.Find("BarSlider").GetComponent<Slider>();

        var healthBar02 = healthBars.Find("Health02");
        _health2Slider = healthBar02.Find("BarSlider").GetComponent<Slider>();

        var healthBar03 = healthBars.Find("Health03");
        _health3Slider = healthBar03.Find("BarSlider").GetComponent<Slider>();

        var healthBar04 = healthBars.Find("Health04");
        _health4Slider = healthBar04.Find("BarSlider").GetComponent<Slider>();

        _healthSliderArray = new Slider[]
        {
            _health1Slider,
            _health2Slider,
            _health3Slider,
            _health4Slider
        };


        var flashingBar01 = healthBar01.Find("FlashingSlider");
        _flashingHealth1Slider = flashingBar01.GetComponent<Slider>();
        _flashingHealth1CanvasGroup = flashingBar01.GetComponent<CanvasGroup>();

        var flashingBar02 = healthBar02.Find("FlashingSlider");
        _flashingHealth2Slider = flashingBar02.GetComponent<Slider>();
        _flashingHealth2CanvasGroup = flashingBar02.GetComponent<CanvasGroup>();

        var flashingBar03 = healthBar03.Find("FlashingSlider");
        _flashingHealth3Slider = flashingBar03.GetComponent<Slider>();
        _flashingHealth3CanvasGroup = flashingBar03.GetComponent<CanvasGroup>();

        var flashingBar04 = healthBar04.Find("FlashingSlider");
        _flashingHealth4Slider = flashingBar04.GetComponent<Slider>();
        _flashingHealth4CanvasGroup = flashingBar04.GetComponent<CanvasGroup>();


        _flashingHealthSliderArray = new Slider[]
        {
            _flashingHealth1Slider,
            _flashingHealth2Slider,
            _flashingHealth3Slider,
            _flashingHealth4Slider
        };


        _flashingHealth1CanvasGroup.alpha = 0;
        _flashingHealth2CanvasGroup.alpha = 0;
        _flashingHealth3CanvasGroup.alpha = 0;
        _flashingHealth4CanvasGroup.alpha = 0;

        _flashingHealthCanvasGroupArray = new CanvasGroup[]
        {
            _flashingHealth1CanvasGroup,
            _flashingHealth2CanvasGroup,
            _flashingHealth3CanvasGroup,
            _flashingHealth4CanvasGroup
        };



        var damageBar01 = healthBar01.Find("DamageSlider");
        _damageHealth1Slider = damageBar01.GetComponent<Slider>();
        _damageHealth1CanvasGroup = damageBar01.GetComponent<CanvasGroup>();

        var damageBar02 = healthBar02.Find("DamageSlider");
        _damageHealth2Slider = damageBar02.GetComponent<Slider>();
        _damageHealth2CanvasGroup = damageBar02.GetComponent<CanvasGroup>();

        var damageBar03 = healthBar03.Find("DamageSlider");
        _damageHealth3Slider = damageBar03.GetComponent<Slider>();
        _damageHealth3CanvasGroup = damageBar03.GetComponent<CanvasGroup>();

        var damageBar04 = healthBar04.Find("DamageSlider");
        _damageHealth4Slider = damageBar04.GetComponent<Slider>();
        _damageHealth4CanvasGroup = damageBar04.GetComponent<CanvasGroup>();

        _damageHealthSliderArray = new Slider[]
        {
            _damageHealth1Slider,
            _damageHealth2Slider,
            _damageHealth3Slider,
            _damageHealth4Slider
        };

        _damageHealthCanvasGroupArray = new CanvasGroup[]
        {
            _damageHealth1CanvasGroup,
            _damageHealth2CanvasGroup,
            _damageHealth3CanvasGroup,
            _damageHealth4CanvasGroup
        };

    }

    private void SetUpShieldBars()
    {
        var shieldBars = transform.Find("ShieldBars");

        var shieldBar01 = shieldBars.Find("Shield01");
        _shield1Slider = shieldBar01.Find("BarSlider").GetComponent<Slider>();

        var shieldBar02 = shieldBars.Find("Shield02");
        _shield2Slider = shieldBar02.Find("BarSlider").GetComponent<Slider>();

        var shieldBar03 = shieldBars.Find("Shield03");
        _shield3Slider = shieldBar03.Find("BarSlider").GetComponent<Slider>();

        var shieldBar04 = shieldBars.Find("Shield04");
        _shield4Slider = shieldBar04.Find("BarSlider").GetComponent<Slider>();

        _shieldSliderArray = new Slider[]
        {
            _shield1Slider,
            _shield2Slider,
            _shield3Slider,
            _shield4Slider
        };

        var damageBar01 = shieldBar01.Find("DamageSlider");
        _damageShield1Slider = damageBar01.GetComponent<Slider>();
        _damageShield1CanvasGroup = damageBar01.GetComponent<CanvasGroup>();

        var damageBar02 = shieldBar02.Find("DamageSlider");
        _damageShield2Slider = damageBar02.GetComponent<Slider>();
        _damageShield2CanvasGroup = damageBar02.GetComponent<CanvasGroup>();

        var damageBar03 = shieldBar03.Find("DamageSlider");
        _damageShield3Slider = damageBar03.GetComponent<Slider>();
        _damageShield3CanvasGroup = damageBar03.GetComponent<CanvasGroup>();

        var damageBar04 = shieldBar04.Find("DamageSlider");
        _damageShield4Slider = damageBar04.GetComponent<Slider>();
        _damageShield4CanvasGroup = damageBar04.GetComponent<CanvasGroup>();

        _damageShieldSliderArray = new Slider[]
        {
            _damageShield1Slider,
            _damageShield2Slider,
            _damageShield3Slider,
            _damageShield4Slider
        };

        _damageShieldCanvasGroupArray = new CanvasGroup[]
        {
            _damageShield1CanvasGroup,
            _damageShield2CanvasGroup,
            _damageShield3CanvasGroup,
            _damageShield4CanvasGroup
        };

    }

    public void SetPlayerHealthShield(PlayerHealthShield playerHealthShield)
    {
        UpdateDamageHealthPreviousHealthAmounts();
        UpdateDamageShieldPreviousShieldAmounts();

        PlayerHealthShieldChangedEvent.AddListener(PlayerHealthShield_OnHealthShieldChanged);
        PlayerHealthShildRepairEvent.AddListener(PlayerHealthShield_OnRepair);
       

    }

    private void PlayerHealthShield_OnRepair()
    {
        if (_healthIconAnimated != null)
        {
            _healthIconAnimated.SetTrigger("Repair");
        }
    }

    private void UpdateDamageHealthPreviousHealthAmounts()
    {
        _preChangeHealth = _playerHealth.Value;

        for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
        {
            int healthSegmentMin = i * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
            int healthSegmentMax = (i + 1) * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;

            if (_playerHealth.Value < healthSegmentMin)
            {
                // Health amount under minimum for this segment
                _damageHealthPreviousHealthAmountArray[i] = 0;
            }
            else
            {
                if (_playerHealth.Value >= healthSegmentMax)
                {
                    // Health amount above max
                    _damageHealthPreviousHealthAmountArray[i] = PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
                }
                else
                {
                    // Health amount somewhere in between this segment
                    _damageHealthPreviousHealthAmountArray[i] = _playerHealth.Value - healthSegmentMin;
                }
            }

        }
    }

    private void UpdateDamageShieldPreviousShieldAmounts()
    {
        _preChangeShield = _playerShield.Value;

        for (int i = 0; i < SHIELD_SEGMENT_COUNT; i++)
        {
            int shieldSegmentMin = i * PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;
            int shieldSegmentMax = (i + 1) * PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;

            if (_playerShield.Value < shieldSegmentMin)
            {
                // Health amount under minimum for this segment
                _damageShieldPreviousShieldAmountArray[i] = 0;
            }
            else
            {
                if (_playerShield.Value >= shieldSegmentMax)
                {
                    // Health amount above max
                    _damageShieldPreviousShieldAmountArray[i] = PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;
                }
                else
                {
                    // Health amount somewhere in between this segment
                    _damageShieldPreviousShieldAmountArray[i] = _playerShield.Value - shieldSegmentMin;
                }
            }

        }
    }


    private void PlayerHealthShield_OnHealthShieldChanged(bool isDamage)
    {
        if (_healthIconAnimator != null)
        {
            if (isDamage)
            {
                _healthIconAnimator.SetTrigger("Damage");
            }
            else
            {
                _healthIconAnimator.SetTrigger("Heal");
            }

        }

        StartCoroutine(UpdateHealthShield());

        // Health changed, reset fade 
        _damageHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;

        if (_damageHealth1CanvasGroup.alpha <= 0)
        {
            // Damage health bar not visible, set size 
            for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
            {
                _damageHealthSliderArray[i].value = (float)_damageHealthPreviousHealthAmountArray[i] / PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
                _damageHealthCanvasGroupArray[i].alpha = 1f;
            }
        }

        // Make damage health bar visible

        _damageShieldFadeTimer = DAMAGED_SHIELD_FADE_TIMER_MAX;

        // Damage health bar not visible, set size
        if (_damageShield1CanvasGroup.alpha <= 0)
        {
            for (int i = 0; i < SHIELD_SEGMENT_COUNT; i++)
            {
                _damageShieldSliderArray[i].value = (float)_damageShieldPreviousShieldAmountArray[i] / PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;
                _damageShieldCanvasGroupArray[i].alpha = 1f;
            }
        }

        if (!isDamage)
        {
            for (int i = 0; i < SHIELD_SEGMENT_COUNT; i++)
            {
                if (_damageShieldSliderArray[i].value <= _shieldSliderArray[i].value)
                {
                    _damageShieldCanvasGroupArray[i].alpha = 0f;
                }
            }
        }


        if (_playerHealth.Value <= LOW_HEALTH_INDICATOR)
        {
            
            _flashingHealth1CanvasGroup.gameObject.SetActive(true);

            _flashingHealth2CanvasGroup.gameObject.SetActive(_playerHealthShield.GetHealthLevel() == PlayerHealthShield.HealthLevel.Level_2
                                                        || _playerHealthShield.GetHealthLevel() == PlayerHealthShield.HealthLevel.Level_3
                                                        || _playerHealthShield.GetHealthLevel() == PlayerHealthShield.HealthLevel.Level_4);

            _flashingHealth3CanvasGroup.gameObject.SetActive(_playerHealthShield.GetHealthLevel() == PlayerHealthShield.HealthLevel.Level_3
                                                        || _playerHealthShield.GetHealthLevel() == PlayerHealthShield.HealthLevel.Level_4);
            _flashingHealth4CanvasGroup.gameObject.SetActive(_playerHealthShield.GetHealthLevel() == PlayerHealthShield.HealthLevel.Level_4);
            

        }
        else
        {
            for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
            {
                _flashingHealthCanvasGroupArray[i].gameObject.SetActive(false);
            }
        }



    }


    IEnumerator UpdateHealthShield()
    {
        float elapsed = 0f;


        // Update shield bars
        float currentShield = _playerShield.Value;


        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += GameTime.deltaTime;
            float tempShield = Mathf.Lerp(_preChangeShield, currentShield, elapsed / _updateSpeedSeconds);

            for (int i = 0; i < SHIELD_SEGMENT_COUNT; i++)
            {
                int shieldSegmentMin = i * PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;
                int shieldSegmentMax = (i + 1) * PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;

                if (currentShield < shieldSegmentMin)
                {
                    // Health amount under minimum for this segment
                    _shieldSliderArray[i].value = 0f;
                }
                else
                {
                    if (currentShield > shieldSegmentMax)
                    {
                        // Health amount above max
                        _shieldSliderArray[i].value = 1f;
                    }
                    else
                    {
                        // Health amount somewhere in between this segment
                        float fillAmount = (float)(tempShield - shieldSegmentMin) / PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT;
                        _shieldSliderArray[i].value = fillAmount;
                    }
                }

            }

            if (_hiddenShieldSlider != null)
                _hiddenShieldSlider.value = tempShield / (SHIELD_SEGMENT_COUNT * PlayerHealthShield.SHIELD_AMOUNT_PER_SEGMENT);

            yield return null;
        }

        UpdateDamageShieldPreviousShieldAmounts();


        // Update health bars
        elapsed = 0;

        float currentHealth = _playerHealth.Value;

        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += GameTime.deltaTime;
            float tempHealth = Mathf.Lerp(_preChangeHealth, currentHealth, elapsed / _updateSpeedSeconds);


            for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
            {
                int healthSegmentMin = i * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
                int healthSegmentMax = (i + 1) * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;

                if (currentHealth < healthSegmentMin)
                {
                    // Health amount under minimum for this segment
                    _healthSliderArray[i].value = 0f;
                }
                else
                {
                    if (currentHealth > healthSegmentMax)
                    {
                        // Health amount above max
                        _healthSliderArray[i].value = 1f;
                    }
                    else
                    {
                        // Health amount somewhere in between this segment
                        float fillAmount = (float)(tempHealth - healthSegmentMin) / PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
                        _healthSliderArray[i].value = fillAmount;
                    }
                }

            }

            if (_hiddenHealthSlider != null)
                _hiddenHealthSlider.value = tempHealth / (HEALTH_SEGMENT_COUNT * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT);

            yield return null;
        }

        UpdateDamageHealthPreviousHealthAmounts();
    }

    private void UpdateHealthSegment()
    {
        PlayerHealthShield.HealthLevel healthLevel = _playerHealthShield.GetHealthLevel();

        for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
        {
            _healthSliderArray[i].transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = healthBarColor;
            _damageHealthSliderArray[i].transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = healthDamageBarColor;
            _flashingHealthSliderArray[i].transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = flashingHealhtBarColor;
        }

        var healthBars = transform.Find("HealthBars");

        healthBars.Find("Health01").gameObject.SetActive(true);
        healthBars.Find("Health02").gameObject.SetActive(false);
        healthBars.Find("Health03").gameObject.SetActive(false);
        healthBars.Find("Health04").gameObject.SetActive(false);

        
        switch (healthLevel)
        {
            default:
                break;
            case PlayerHealthShield.HealthLevel.Level_2:
                healthBars.Find("Health02").gameObject.SetActive(true);
                break;
            case PlayerHealthShield.HealthLevel.Level_3:
                healthBars.Find("Health02").gameObject.SetActive(true);
                healthBars.Find("Health03").gameObject.SetActive(true);
                break;
            case PlayerHealthShield.HealthLevel.Level_4:
                healthBars.Find("Health02").gameObject.SetActive(true);
                healthBars.Find("Health03").gameObject.SetActive(true);
                healthBars.Find("Health04").gameObject.SetActive(true);
                break;
        }
    }


    private void UpdateShieldSegment()
    {
        PlayerHealthShield.ShieldArmor shieldArmor = _playerHealthShield.GetEquippedShieldArmor();

        var shieldBars = transform.Find("ShieldBars");

        shieldBars.Find("Shield01").gameObject.SetActive(false);
        shieldBars.Find("Shield02").gameObject.SetActive(false);
        shieldBars.Find("Shield03").gameObject.SetActive(false);
        shieldBars.Find("Shield04").gameObject.SetActive(false);

        Color shieldArmorColor = Color.white;

        
        switch (shieldArmor)
        {
            default:
            case PlayerHealthShield.ShieldArmor.None:
                break;
            case PlayerHealthShield.ShieldArmor.Tier_1:
                shieldBars.Find("Shield01").gameObject.SetActive(true);
                shieldArmorColor = tier1Color;
                break;
            case PlayerHealthShield.ShieldArmor.Tier_2:
                shieldBars.Find("Shield01").gameObject.SetActive(true);
                shieldBars.Find("Shield02").gameObject.SetActive(true);
                shieldArmorColor = tier2Color;
                break;
            case PlayerHealthShield.ShieldArmor.Tier_3:
                shieldBars.Find("Shield01").gameObject.SetActive(true);
                shieldBars.Find("Shield02").gameObject.SetActive(true);
                shieldBars.Find("Shield03").gameObject.SetActive(true);
                shieldArmorColor = tier3Color;
                break;
            case PlayerHealthShield.ShieldArmor.Tier_4:
                shieldBars.Find("Shield01").gameObject.SetActive(true);
                shieldBars.Find("Shield02").gameObject.SetActive(true);
                shieldBars.Find("Shield03").gameObject.SetActive(true);
                shieldBars.Find("Shield04").gameObject.SetActive(true);
                shieldArmorColor = tier4Color;
                break;
        }

        for (int i = 0; i < SHIELD_SEGMENT_COUNT; i++)
        {
            _shieldSliderArray[i].transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = shieldArmorColor;
            _damageShieldSliderArray[i].transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = shieldDamageBarColor;
        }
    }

    private void OnDestroy()
    {
        PlayerHealthShieldChangedEvent.RemoveListener(PlayerHealthShield_OnHealthShieldChanged);
        PlayerHealthShildRepairEvent.RemoveListener(PlayerHealthShield_OnRepair);
    }

}
