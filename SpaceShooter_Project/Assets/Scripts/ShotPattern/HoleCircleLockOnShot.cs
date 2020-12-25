using UnityEngine;

/// <summary>
/// Hole circle lock on shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Hole Circle Shot (Lock On)")]
public class HoleCircleLockOnShot : HoleCircleShot
{

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        if (targetTransform == null && setTargetFromTag)
        {
            targetTransform = Util.GetTransformFromTagName(targetTagName);
        }
        if (targetTransform == null)
        {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        holeCenterAngle = Util.GetAngleFromTwoPosition(transform, targetTransform) + Random.Range(-20, 20);

        base.Shot();
    }
}
