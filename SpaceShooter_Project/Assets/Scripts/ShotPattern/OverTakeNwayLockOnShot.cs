using UnityEngine;

/// <summary>
/// Over take nway lock on shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Over Take nWay Shot (Lock On)")]
public class OverTakeNwayLockOnShot : OverTakeNwayShot
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

        centerAngle = Util.GetAngleFromTwoPosition(transform, targetTransform);

        base.Shot();
    }
}