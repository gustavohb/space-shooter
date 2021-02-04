using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CircleCollider2D))]
public class DamageTriggerCollider : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Enemy";

    [SerializeField] private float _damage = 0.8f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == _targetTag)
        {
            IDamageable damageableObject = collision.GetComponent<IDamageable>();

            if (damageableObject != null)
            {
                damageableObject.Damage(_damage, true);
            }
        }
    }

}
