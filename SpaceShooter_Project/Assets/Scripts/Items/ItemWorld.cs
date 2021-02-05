using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _lifeTime = 50f;

    [SerializeField] private InventorySO _inventory = default;

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.itemWorldPrefab, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private Item _item;

    [HideInInspector]
    public bool isPickingUp = false;

    [SerializeField]
    private string _playerTag = "Player";

    private SpriteRenderer _background1SpriteRenderer;
    private SpriteRenderer _background2SpriteRenderer;
    Transform _amountTransform;
    private TextMeshPro _textAmount;

    private bool isRemoving = false;

    private void Awake()
    {
        _background1SpriteRenderer = transform.Find("Base").Find("Background01").GetComponent<SpriteRenderer>();
        _background2SpriteRenderer = transform.Find("Base").Find("Background02").GetComponent<SpriteRenderer>();

        _amountTransform = transform.Find("Base").Find("Amount");
        _textAmount = _amountTransform.Find("Text").GetComponent<TextMeshPro>();

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

    }

    private void Start()
    {
        AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ItemAppear);
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0 && !isRemoving)
        {
            isRemoving = true;
            AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ItemDisappear);
            _animator.SetTrigger("Pickup");
        }

    }


    public void SetItem(Item item)
    {
        _item = item;

        _background1SpriteRenderer.sprite = item.GetIcon();
        _background2SpriteRenderer.sprite = item.GetIcon();

        if (item.amount > 1)
        {
            _textAmount.SetText(item.amount.ToString());
        }
        else
        {
            _amountTransform.gameObject.SetActive(false);
        }

    }

    public Item GetItem()
    {
        return _item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }


    public void PickUp()
    {
        if (!isPickingUp)
        {
            _inventory?.AddItem(GetItem());

            AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ItemPickup);

            if (_animator != null)
            {
                _animator.SetTrigger("Pickup");
            }
            else
            {

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _playerTag)
        {

            if (!isPickingUp)
            {
                isPickingUp = true;
                _inventory?.AddItem(GetItem());

                AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ItemPickup);

                if (_animator != null)
                {
                    _animator.SetTrigger("Pickup");
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
