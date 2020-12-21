using System;

public interface IDamageable
{
    event EventHandler OnDeath;

    void Damage(float damageAmount, bool isMeleeAttack = false);

}
