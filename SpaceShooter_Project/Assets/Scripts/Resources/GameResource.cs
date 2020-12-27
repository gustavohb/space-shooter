using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class GameResource : ExtendedCustomMonoBehavior
{
    public ResourcesManager.ResourceType resourceType;

    public int value = 1;

    public float lifeTime = 15f;

    [Range(0.05f, 1f)]
    public float startToFadePct = 0.15f;

    [HideInInspector] public Transform controlPointIn;

    public SpriteRenderer itemSpriteRenderer;

    public string playerTag = "Player";

    private float _lifeTimer = 0;

    private IEnumerator _blink;

    private float _fadePct;

    private void Start()
    {
        switch (resourceType)
        {
            case (ResourcesManager.ResourceType.Coin):
                if (transform.position.x < ResourcesManager.Instance.coinIcon.position.x)
                {
                    controlPointIn = transform.Find("ControlPointInRight");
                }
                else
                {
                    controlPointIn = transform.Find("ControlPointInLeft");
                }
                break;
            case (ResourcesManager.ResourceType.Star):
                if (transform.position.x < ResourcesManager.Instance.starIcon.position.x)
                {
                    controlPointIn = transform.Find("ControlPointInRight");
                }
                else
                {
                    controlPointIn = transform.Find("ControlPointInLeft");
                }
                break;
            case (ResourcesManager.ResourceType.Health):
                if (transform.position.x < ResourcesManager.Instance.healthSliderPosition.position.x)
                {
                    controlPointIn = transform.Find("ControlPointInRight");
                }
                else
                {
                    controlPointIn = transform.Find("ControlPointInLeft");
                }
                break;
            case (ResourcesManager.ResourceType.Shield):
                if (transform.position.x < ResourcesManager.Instance.shieldSliderPosition.position.x)
                {
                    controlPointIn = transform.Find("ControlPointInRight");
                }
                else
                {
                    controlPointIn = transform.Find("ControlPointInLeft");
                }
                break;
        }

        _lifeTimer = lifeTime;
    }

    private void Update()
    {
        _lifeTimer -= Time.deltaTime;

        _fadePct = _lifeTimer / lifeTime;

        if (_fadePct <= startToFadePct && _blink == null)
        {
            _blink = StartToBlink();
            StartCoroutine(_blink);
        }

        if (_fadePct <= 0)
        {
            Destroy(gameObject);
        }

    }

    private IEnumerator StartToBlink()
    {
        Color originalColor = itemSpriteRenderer.material.color;
        Color fadeColor = originalColor;

        while (true)
        {
            fadeColor.a = _fadePct / startToFadePct;
            itemSpriteRenderer.material.color = fadeColor;
            yield return new WaitForSeconds(startToFadePct - _fadePct);
            itemSpriteRenderer.material.color = originalColor;
            yield return new WaitForSeconds(startToFadePct - _fadePct);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            ResourcesManager.Instance.CollectResource(this);
        }
    }

}
