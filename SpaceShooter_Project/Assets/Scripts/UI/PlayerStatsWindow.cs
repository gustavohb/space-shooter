using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsWindow : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1.0f;
    private const int LOW_HEALTH_INDICATOR = 10;
    private const int HEALTH_SEGMENT_COUNT = 4;

    private PlayerHealthShield _playerHealthShield;

    public Color tier1Color = new Color32(37, 255, 249, 255);

    public Color tier2Color = new Color32(37, 255, 249, 255);

    public Color tier3Color = new Color32(37, 255, 249, 255);

    public Color tier4Color = new Color32(37, 255, 249, 255);

    public Color healthBarColor = new Color32(255, 6, 136, 255);

    public Color healthDamageBarColor = new Color32(255, 189, 211, 255);

    public Color flashingHealhtBarColor = new Color32(200, 0, 78, 255);

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

    [SerializeField] private Animator _healthIconAnimator;

    private float[] damageHealthPreviousHealthAmountArray = new float[HEALTH_SEGMENT_COUNT];

    [SerializeField] private Animator _healthIconAnimated;

    private float _preChangeHealth;


    [SerializeField]
    private float _updateSpeedSeconds = 0.3f;

    private void Awake()
    {
        _lowHealthAlphaChange = +4f;

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

    private void Start()
    {
        _playerHealthShield = FindObjectOfType<PlayerHealthShield>();
        if (_playerHealthShield != null)
        {
            SetPlayerHealthShield(_playerHealthShield);
            UpdateHealthSegment();
        }

    }

    private void Update()
    {


        // Is the damage health slider visible
        if (_damageHealth1CanvasGroup.alpha > 0)
        {
            // Count down fade timer
            _damageHealthFadeTimer -= Time.deltaTime;
            if (_damageHealthFadeTimer < 0)
            {
                // Fade timer over, lower alpha
                float newAlpha = _damageHealth1CanvasGroup.alpha;
                newAlpha -= Time.deltaTime * 4f;

                // Damage health bar not visible, set size 
                for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
                {
                    _damageHealthCanvasGroupArray[i].alpha = newAlpha;
                }
            }
        }


        // Flashing health image;
        if (_flashingHealth1CanvasGroup.gameObject.activeSelf)
        {
            float lowHealthAlpha = _flashingHealth1CanvasGroup.alpha;
            lowHealthAlpha += _lowHealthAlphaChange * Time.deltaTime * 1.0f;

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

    public void SetPlayerHealthShield(PlayerHealthShield playerHealthShield)
    {
        _playerHealthShield = playerHealthShield;

        UpdateDamageHealthPreviousHealthAmounts();

        playerHealthShield.OnHealthShieldChanged += PlayerHealthShield_OnHealthShieldChanged;
        playerHealthShield.OnRepair += PlayerHealthShield_OnRepair;
    }

    private void PlayerHealthShield_OnRepair(object sender, System.EventArgs e)
    {
        if (_healthIconAnimated != null)
        {
            _healthIconAnimated.SetTrigger("Repair");
        }
    }

    private void UpdateDamageHealthPreviousHealthAmounts()
    {

        float health = _playerHealthShield.GetHealth();

        _preChangeHealth = health;

        for (int i = 0; i < HEALTH_SEGMENT_COUNT; i++)
        {
            int healthSegmentMin = i * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
            int healthSegmentMax = (i + 1) * PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;

            if (health < healthSegmentMin)
            {
                // Health amount under minimum for this segment
                damageHealthPreviousHealthAmountArray[i] = 0;
            }
            else
            {
                if (health >= healthSegmentMax)
                {
                    // Health amount above max
                    damageHealthPreviousHealthAmountArray[i] = PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
                }
                else
                {
                    // Health amount somewhere in between this segment
                    damageHealthPreviousHealthAmountArray[i] = health - healthSegmentMin;
                }
            }

        }
    }

    private void PlayerHealthShield_OnHealthShieldChanged(object sender, bool isDamage)
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
                _damageHealthSliderArray[i].value = (float)damageHealthPreviousHealthAmountArray[i] / PlayerHealthShield.HEALTH_AMOUNT_PER_SEGMENT;
                _damageHealthCanvasGroupArray[i].alpha = 1f;
            }
        }

        if (_playerHealthShield.GetHealth() <= LOW_HEALTH_INDICATOR)
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

        float currentHealth = _playerHealthShield.GetHealth();

        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
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


        transform.Find("HealthBars").Find("Health01").gameObject.SetActive(true);
        transform.Find("HealthBars").Find("Health02").gameObject.SetActive(false);
        transform.Find("HealthBars").Find("Health03").gameObject.SetActive(false);
        transform.Find("HealthBars").Find("Health04").gameObject.SetActive(false);

        switch (healthLevel)
        {
            default:
                break;
            case PlayerHealthShield.HealthLevel.Level_2:
                transform.Find("HealthBars").Find("Health02").gameObject.SetActive(true);
                break;
            case PlayerHealthShield.HealthLevel.Level_3:
                transform.Find("HealthBars").Find("Health02").gameObject.SetActive(true);
                transform.Find("HealthBars").Find("Health03").gameObject.SetActive(true);
                break;
            case PlayerHealthShield.HealthLevel.Level_4:
                transform.Find("HealthBars").Find("Health02").gameObject.SetActive(true);
                transform.Find("HealthBars").Find("Health03").gameObject.SetActive(true);
                transform.Find("HealthBars").Find("Health04").gameObject.SetActive(true);
                break;
        }


    }

}
