using System;
using UnityEngine;

public interface IDamageable
{
    event EventHandler OnDeath;

    bool HasShield();

    bool HasShield(float damageAmount);

    Color GetShieldColor();

    void Damage(float damageAmount, bool isMeleeAttack = false);

    float GetShieldScaleFactor();
}
