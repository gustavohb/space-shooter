using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Attractor : MonoBehaviour
{
    [SerializeField] private float _attractionRange = 3.5f;

    [SerializeField] private float _attractionSpeed = 15f;

    [SerializeField] private string _attractionTag = "Attractive";

    private CircleCollider2D m_Collider;

    private void Start()
    {
        m_Collider = GetComponent<CircleCollider2D>();
        m_Collider.radius = _attractionRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerStay2D(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameTime.isPaused)
        {
            return;
        }
        GameObject attractive = collision.gameObject;
        if (attractive.tag == _attractionTag)
        {
            float rotationTarget = Mathf.Atan2(transform.position.y - attractive.transform.position.y, transform.position.x - attractive.transform.position.x) * (180 / Mathf.PI);
            Vector3 newPosition = attractive.transform.position;

            newPosition.x += Mathf.Cos(rotationTarget * Mathf.PI / 180) * _attractionSpeed * Time.deltaTime;
            newPosition.y += Mathf.Sin(rotationTarget * Mathf.PI / 180) * _attractionSpeed * Time.deltaTime;

            attractive.transform.position = newPosition;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _attractionRange);
    }

}
