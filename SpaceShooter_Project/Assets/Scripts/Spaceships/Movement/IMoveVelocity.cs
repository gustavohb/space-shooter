using UnityEngine;

public interface IMoveVelocity
{
    void SetVelocity(Vector3 velocityVector);

    void DisableMovement();

    void DisableMovement(float time);

    void EnableMovement();
}
