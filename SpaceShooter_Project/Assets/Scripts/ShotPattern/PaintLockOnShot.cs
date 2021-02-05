using UnityEngine;

/// <summary>
/// Paint lock on shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Paint Shot (Lock On)")]
public class PaintLockOnShot : PaintShot
{


    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        if (_shooting)
        {
            return;
        }
        if (targetTransform == null && setTargetFromTag)
        {
            targetTransform = Util.GetTransformFromTagName(targetTagName);
        }
        if (targetTransform == null)
        {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        paintCenterAngle = Util.GetAngleFromTwoPosition(transform, targetTransform);

        base.Shot();
    }
}