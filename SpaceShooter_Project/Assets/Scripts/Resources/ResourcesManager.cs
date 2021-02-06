using DG.Tweening;
using TMPro;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ResourcesManager : SingletonMonoBehavior<ResourcesManager>
{
    public enum ResourceType { Coin, Star, Health, Shield } //resource types

    public enum DetectionMode { Detect2D, Detect3D } //detection mode

    public Camera targetCamera;

    public PlayerHealthShield player;

    [Header("UI References")]
    public Transform targetCanvas;

    [Space(5)]
    public RectTransform coinIcon; //reference to the coin icon (where the animated coin flies to)
    public TextMeshProUGUI coinCounterTMPro; //reference to the coin counter (show how many coins the player has)
    public Transform coinDeviationTransform;

    [SerializeField] private IntVariable _coins = default; //variable for coins

    [SerializeField] private IntVariable _collectedCoins = default;

    [Space(5)]
    public RectTransform starIcon; //reference to the star icon (where the animated coin flies to)
    public TextMeshProUGUI starCounterTMPro; //reference to the star counter (show how many stars the player has)
    public Transform starDeviationTransform;
    [SerializeField] private IntVariable _stars = default; //variable for stars

    [Space(5)]
    public RectTransform healthSliderPosition;
    public Transform healthDeviationTransform;

    [Space(5)]
    public RectTransform shieldSliderPosition;
    public Transform shieldDeviationTransform;

    [Header("Prefab")]
    public GameObject coinAnimatedPrefab;
    public GameObject coinEffectPrefab;
    [Space(5)]
    public GameObject starAnimatedPrefab;
    public GameObject starEffectPrefab;
    [Space(5)]
    public GameObject healthAnimatedPrefab;
    public GameObject healthEffectPrefab;
    [Space(5)]
    public GameObject shieldAnimatedPrefab;
    public GameObject shieldEffectPrefab;

    [Header("Settings")]
    public DetectionMode detectionMode = DetectionMode.Detect2D; //the active detection mode (2D or 3D)
    public bool useMouse = true; //should the system detect mouse
    public bool useTouch = true; //should the system detect touch

    [Space(5)]
    public float animationMaxDuration = 0.6f;
    public float animationMinDuration = 0.3f;
    public float animationSpeedSeconds = 0.035f;
    public Ease animationEase = Ease.OutCirc;


    [Header("Events")]
    [SerializeField] private GameEvent _doubleCollectedCoinsEvent = default;

    private GameResource _gameResource;

    private void Start()
    {
        player = FindObjectOfType<PlayerHealthShield>();

        _collectedCoins.Value = 0;

        if (targetCamera == null) //is any camera referenced?
        {
            targetCamera = Camera.main; //get the reference to the main camera
        }

        if (targetCamera == null) //is any camera referenced? (sanity check)
        {
            Debug.Log("No camera found! The '" + GetType().Name + "' component is now disable.", gameObject); //let the developer know that this script has no camera referenced
            enabled = false; //disable this component
            return; //stop here
        }

        if (!useMouse && !useTouch) //check that at least one input type is enabled
        {
            Debug.Log("Mouse and Touch inputs are disabled. You need to enable at least one input mode in order to detect resources pickups. The '" + GetType().Name + "' component is now disable.", gameObject); //let the developer know what is wrong
            enabled = false; //disable this component
            return; //stop here
        }

        if (detectionMode == DetectionMode.Detect2D && !targetCamera.orthographic) //check that the targetCamera is set to Orthographic if detection mode is set to 2D (the system will not work otherwise
        {
            Debug.Log("Because detection mode is set to '" + detectionMode + "', the camera's projection should be set to 'Orthographic'", gameObject);
        }

        //_coins.Value = PlayerPrefs.GetInt(PLAYER_PREFS_KEY_COINS, 0); //get the coins value from the PlayerPrefs
        //_stars.Value = PlayerPrefs.GetInt(PLAYER_PREFS_KEY_STARS, 0); //get the stars value from the PlayerPrefs

        UpdateResourceCounter(ResourceType.Coin);
        UpdateResourceCounter(ResourceType.Star);

        _doubleCollectedCoinsEvent?.AddListener(DoubleCollectedCoins);

    }

    private void Update()
    {
        DetectInput(); //detects mouse and/or touch
    }

    private void LateUpdate()
    {
        if (coinCounterTMPro.rectTransform.localScale.x > 1.5f || coinCounterTMPro.rectTransform.localScale.y > 1.5f)
        {
            Vector3 newSize = new Vector3(1.5f, 1.5f, 1.0f);
            coinCounterTMPro.rectTransform.localScale = newSize;
        }

        if (coinIcon.localScale.x > 1.5f || coinIcon.localScale.y > 1.5f)
        {
            Vector3 newSize = new Vector3(1.5f, 1.5f, 1.0f);
            coinIcon.localScale = newSize;
        }

        if (coinIcon.localScale.x < 0.6f || coinIcon.localScale.y < 0.6f)
        {
            Vector3 newSize = new Vector3(0.6f, 0.6f, 1.0f);
            coinIcon.localScale = newSize;
        }
    }

    private bool MouseDetected()
    {
        return Input.GetMouseButtonDown(0); //is the left mouse button down?
    }

    private bool TouchDetected()
    {
        return Input.touchCount > 0;// is there at least one finger touching the screen?
    }

    private void DetectInput()
    {
        if (useMouse && MouseDetected())
        {
            switch (detectionMode)
            {
                case DetectionMode.Detect2D: Raycast2D(targetCamera.ScreenPointToRay(Input.mousePosition)); break; //raycast2D from the mouse position
                case DetectionMode.Detect3D: Raycast3D(targetCamera.ScreenPointToRay(Input.mousePosition)); break; //raycast3D from the mouse position
            }
        }
        else if (useTouch && TouchDetected()) //is touch enabled and is at least one finger touching the screen?
        {
            switch (detectionMode)
            {
                case DetectionMode.Detect2D: Raycast2D(targetCamera.ScreenPointToRay(Input.GetTouch(0).position)); break; //raycast2D from the first finger position
                case DetectionMode.Detect3D: Raycast3D(targetCamera.ScreenPointToRay(Input.GetTouch(0).position)); break; //raycast3D from the first finger position
            }
        }
    }

    private void Raycast2D(Ray ray)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, Vector2.zero, 0); //execute a raycast2D from the origin

        if (hit2D) //did the ray hit anything (any collider)
        {
            _gameResource = hit2D.transform.GetComponent<GameResource>(); //does the hit target have the proper component
            if (_gameResource != null) //proper component found
            {
                CollectResource(_gameResource); //collect the resource
            }
        }
    }

    private void Raycast3D(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) //execute a raycast from the origin
        {
            _gameResource = hit.transform.GetComponent<GameResource>(); //does the hit target have the proper component
            if (_gameResource != null) //proper component found
            {
                CollectResource(_gameResource); //collect the resource
            }
        }
    }

    private void DoubleCollectedCoins()
    {
        _coins.Value += _collectedCoins.Value;
    }

    public void CollectResource(GameResource gameResource)
    {
        switch (gameResource.resourceType)
        {
            case ResourceType.Coin: ExecutePickup(gameResource, coinAnimatedPrefab, coinEffectPrefab, coinIcon); break;//collect the coin resource
            case ResourceType.Star: ExecutePickup(gameResource, starAnimatedPrefab, starEffectPrefab, starIcon); break;//collect the star resource
            case ResourceType.Health: ExecutePickup(gameResource, healthAnimatedPrefab, healthEffectPrefab, healthSliderPosition); break; //collect the health resource
            case ResourceType.Shield: ExecutePickup(gameResource, shieldAnimatedPrefab, shieldEffectPrefab, shieldSliderPosition); break; //collect the health resource
        }
    }

    private void ExecutePickup(GameResource gameResource, GameObject resourceAnimatedPrefab, GameObject resourceEffectPrefab, RectTransform resourceIcon)
    {
        RectTransform clone = Instantiate(resourceAnimatedPrefab, targetCanvas, false).GetComponent<RectTransform>();
        clone.anchorMin = targetCamera.WorldToViewportPoint(gameResource.transform.position); //get the world viewport position and set it as the anchor min
        clone.anchorMax = clone.anchorMin; //anchor max = anchor min

        clone.anchoredPosition = clone.localPosition; //anchore position = local position

        clone.anchorMin = new Vector2(0.5f, 0.5f); // center anchor min
        clone.anchorMax = clone.anchorMin; // center anchor max

        clone.SetParent(resourceIcon);

        Vector3 controlPointOut = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;

        switch (gameResource.resourceType)
        {
            case ResourceType.Coin:
                controlPointOut = coinDeviationTransform.position;
                targetPosition = coinIcon.position;
                break;
            case ResourceType.Star:
                controlPointOut = starDeviationTransform.position;
                targetPosition = starIcon.position;
                break;
            case ResourceType.Health:
                controlPointOut = healthDeviationTransform.position;
                targetPosition = healthSliderPosition.position;
                break;
            case ResourceType.Shield:
                controlPointOut = shieldDeviationTransform.position;
                targetPosition = shieldSliderPosition.position;
                break;
        }

        float distance = Vector3.Distance(clone.position, targetPosition);

        clone.DOPath(new Vector3[] { targetPosition, gameResource.controlPointIn.position, controlPointOut }, Mathf.Clamp((animationSpeedSeconds * distance), animationMinDuration, animationMaxDuration), PathType.CubicBezier, PathMode.Sidescroller2D, 10, null)
            .SetEase(animationEase)
            .OnComplete(() =>
            {
                Destroy(clone.gameObject);


                resourceIcon.DOPunchScale(new Vector3(0.2f, 0.2f, 0.0f), 0.6f)
                    .OnComplete(() =>
                    {
                        resourceIcon.localScale = Vector3.one;
                    })
                    .Play();

                Instantiate(resourceEffectPrefab, resourceIcon, false);

                UpdateResourceValueBy(gameResource.resourceType, gameResource.value);
                UpdateResourceCounter(gameResource.resourceType);
            })
            .Play();

        Destroy(gameResource.gameObject);
    }

    private void UpdateResourceValueBy(ResourceType resourceType, int value)
    {
        switch (resourceType)
        {
            case ResourceType.Coin: _coins.Value += value; _collectedCoins.Value += value; break; //update the coins with the set value
            case ResourceType.Star: _stars.Value += value; break; //update the stars with the set value
            case ResourceType.Health: player?.HealHealth(value); break; //update the player's health with the set value
            case ResourceType.Shield: player?.HealShield(value); break; //update the player's health with the set value
        }
    }

    private void UpdateResourceCounter(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Coin:
                coinCounterTMPro.text = _coins.Value.ToString(); //update the coin counter text
                if (coinCounterTMPro.rectTransform.localScale.x < 1.5f && coinCounterTMPro.rectTransform.localScale.y < 1.5f)
                {
                    coinCounterTMPro.rectTransform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.0f), 0.8f)
                                        .OnComplete(() =>
                                        {
                                            coinCounterTMPro.rectTransform.localScale = Vector3.one;
                                        })
                                        .Play();
                }
                break;
            case ResourceType.Star:
                starCounterTMPro.text = _stars.ToString(); //update the coin counter text
                starCounterTMPro.rectTransform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.0f), 0.8f)
                   .OnComplete(() =>
                   {
                       starCounterTMPro.rectTransform.localScale = Vector3.one;
                   })
                   .Play();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        _doubleCollectedCoinsEvent?.RemoveListener(DoubleCollectedCoins);
    }
}
