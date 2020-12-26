﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Game/Shot Pattern/Waving nWay Shot (Lock On)")]
public class WavingNwayLockOnShot : WavingNwayShot
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

        AimTarget();

        if (targetTransform == null)
        {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot();

        if (aiming)
        {
            StartCoroutine(AimingCoroutine());
        }
    }

    void AimTarget()
    {
        if (targetTransform == null && setTargetFromTag)
        {
            targetTransform = Util.GetTransformFromTagName(targetTagName);
        }
        if (targetTransform != null)
        {
            waveCenterAngle = Util.GetAngleFromTwoPosition(transform, targetTransform);
        }
    }

    IEnumerator AimingCoroutine()
    {
        while (aiming)
        {
            if (_shooting == false)
            {
                yield break;
            }

            AimTarget();

            yield return 0;
        }
    }
}