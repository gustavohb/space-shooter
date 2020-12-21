using System;
using UnityEngine;

public static class TransformExtention
{
    static Vector3 _tempV3A = Vector3.zero;
    static Vector3 _tempV3B = Vector3.zero;
    static Vector2 _tempV2 = Vector2.zero;

    #region Getter

    public static Vector2 GetLocalPositionVector2XY(this Transform transform)
    {
        _tempV2.x = transform.localPosition.x;
        _tempV2.y = transform.localPosition.y;
        return _tempV2;
    }

    public static Vector2 GetPositionVector2XY(this Transform transform)
    {
        _tempV2.x = transform.position.x;
        _tempV2.y = transform.position.y;
        return _tempV2;
    }

    #endregion

    #region Reset

    public static void ResetTransform(this Transform transform, bool worldSpace = false)
    {
        transform.ResetPosition(worldSpace);
        transform.ResetRotation(worldSpace);
        transform.ResetLocalScale();
    }

    public static void ResetPosition(this Transform transform, bool worldSpace = false)
    {
        if (worldSpace)
        {
            transform.position = Vector3.zero;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public static void ResetRotation(this Transform transform, bool worldSpace = false)
    {
        if (worldSpace)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    public static void ResetLocalScale(this Transform transform)
    {
        transform.localScale = Vector3.one;
    }

    #endregion

    #region SetPosition

    public static void SetPosition(this Transform transform, float x, float y, float z)
    {
        _tempV3A.Set(x, y, z);
        transform.position = _tempV3A;
    }

    public static void SetPosition(this Transform transform, float x, float y)
    {
        transform.SetPosition(x, y, transform.position.z);
    }

    public static void SetPositionX(this Transform transform, float x)
    {
        transform.SetPosition(x, transform.position.y, transform.position.z);
    }

    public static void SetPositionY(this Transform transform, float y)
    {
        transform.SetPosition(transform.position.x, y, transform.position.z);
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
        transform.SetPosition(transform.position.x, transform.position.y, z);
    }

    #endregion

    #region SetLocalPosition

    public static void SetLocalPosition(this Transform transform, float x, float y, float z)
    {
        _tempV3A.Set(x, y, z);
        transform.localPosition = _tempV3A;
    }

    public static void SetLocalPosition(this Transform transform, float x, float y)
    {
        transform.SetLocalPosition(x, y, transform.localPosition.z);
    }

    public static void SetLocalPositionX(this Transform transform, float x)
    {
        transform.SetLocalPosition(x, transform.localPosition.y, transform.localPosition.z);
    }

    public static void SetLocalPositionY(this Transform transform, float y)
    {
        transform.SetLocalPosition(transform.localPosition.x, y, transform.localPosition.z);
    }

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
        transform.SetLocalPosition(transform.localPosition.x, transform.localPosition.y, z);
    }

    #endregion

    #region SetEulerAngles

    public static void SetEulerAngles(this Transform transform, float x, float y, float z)
    {
        _tempV3A.Set(x, y, z);
        transform.eulerAngles = _tempV3A;
    }

    public static void SetEulerAnglesX(this Transform transform, float x)
    {
        transform.SetEulerAngles(x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public static void SetEulerAnglesY(this Transform transform, float y)
    {
        transform.SetEulerAngles(transform.eulerAngles.x, y, transform.eulerAngles.z);
    }

    public static void SetEulerAnglesZ(this Transform transform, float z)
    {
        transform.SetEulerAngles(transform.eulerAngles.x, transform.eulerAngles.y, z);
    }

    #endregion

    #region SetLocalEulerAngles

    public static void SetLocalEulerAngles(this Transform transform, float x, float y, float z)
    {
        _tempV3A.Set(x, y, z);
        transform.localEulerAngles = _tempV3A;
    }

    public static void SetLocalEulerAnglesX(this Transform transform, float x)
    {
        transform.SetLocalEulerAngles(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public static void SetLocalEulerAnglesY(this Transform transform, float y)
    {
        transform.SetLocalEulerAngles(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }

    public static void SetLocalEulerAnglesZ(this Transform transform, float z)
    {
        transform.SetLocalEulerAngles(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
    }

    #endregion

    #region SetLocalScale

    public static void SetLocalScale(this Transform transform, float x, float y, float z)
    {
        _tempV3A.Set(x, y, z);
        transform.localScale = _tempV3A;
    }

    public static void SetLocalScaleX(this Transform transform, float x)
    {
        transform.SetLocalScale(x, transform.localScale.y, transform.localScale.z);
    }

    public static void SetLocalScaleY(this Transform transform, float y)
    {
        transform.SetLocalScale(transform.localScale.x, y, transform.localScale.z);
    }

    public static void SetLocalScaleZ(this Transform transform, float z)
    {
        transform.SetLocalScale(transform.localScale.x, transform.localScale.y, z);
    }

    #endregion

    #region AddPosition

    public static void AddPosition(this Transform transform, float x, float y, float z)
    {
        transform.SetPosition(transform.position.x + x, transform.position.y + y, transform.position.z + z);
    }

    public static void AddPositionX(this Transform transform, float x)
    {
        transform.SetPositionX(transform.position.x + x);
    }

    public static void AddPositionY(this Transform transform, float y)
    {
        transform.SetPositionY(transform.position.y + y);
    }

    public static void AddPositionZ(this Transform transform, float z)
    {
        transform.SetPositionZ(transform.position.z + z);
    }

    #endregion

    #region AddLocalPosition

    public static void AddLocalPosition(this Transform transform, float x, float y, float z)
    {
        transform.SetLocalPosition(transform.localPosition.x + x, transform.localPosition.y + y, transform.localPosition.z + z);
    }

    public static void AddLocalPositionX(this Transform transform, float x)
    {
        transform.SetLocalPositionX(transform.localPosition.x + x);
    }

    public static void AddLocalPositionY(this Transform transform, float y)
    {
        transform.SetLocalPositionY(transform.localPosition.y + y);
    }

    public static void AddLocalPositionZ(this Transform transform, float z)
    {
        transform.SetLocalPositionZ(transform.localPosition.z + z);
    }

    #endregion

    #region AddEulerAngles

    public static void AddEulerAngles(this Transform transform, float x, float y, float z)
    {
        transform.SetEulerAngles(transform.eulerAngles.x + x, transform.eulerAngles.y + y, transform.eulerAngles.z + z);
    }

    public static void AddEulerAnglesX(this Transform transform, float x)
    {
        transform.SetEulerAnglesX(transform.eulerAngles.x + x);
    }

    public static void AddEulerAnglesY(this Transform transform, float y)
    {
        transform.SetEulerAnglesY(transform.eulerAngles.y + y);
    }

    public static void AddEulerAnglesZ(this Transform transform, float z)
    {
        transform.SetEulerAnglesZ(transform.eulerAngles.z + z);
    }

    #endregion

    #region AddLocalEulerAngles

    public static void AddLocalEulerAngles(this Transform transform, float x, float y, float z)
    {
        transform.SetLocalEulerAngles(transform.localEulerAngles.x + x, transform.localEulerAngles.y + y, transform.localEulerAngles.z + z);
    }

    public static void AddLocalEulerAnglesX(this Transform transform, float x)
    {
        transform.SetLocalEulerAnglesX(transform.localEulerAngles.x + x);
    }

    public static void AddLocalEulerAnglesY(this Transform transform, float y)
    {
        transform.SetLocalEulerAnglesY(transform.localEulerAngles.y + y);
    }

    public static void AddLocalEulerAnglesZ(this Transform transform, float z)
    {
        transform.SetLocalEulerAnglesZ(transform.localEulerAngles.z + z);
    }

    #endregion

    #region AddLocalScale

    public static void AddLocalScale(this Transform transform, float x, float y, float z)
    {
        transform.SetLocalScale(transform.localScale.x + x, transform.localScale.y + y, transform.localScale.z + z);
    }

    public static void AddLocalScaleX(this Transform transform, float x)
    {
        transform.SetLocalScaleX(transform.localScale.x + x);
    }

    public static void AddLocalScaleY(this Transform transform, float y)
    {
        transform.SetLocalScaleY(transform.localScale.y + y);
    }

    public static void AddLocalScaleZ(this Transform transform, float z)
    {
        transform.SetLocalScaleZ(transform.localScale.z + z);
    }

    #endregion

    #region LerpPosition

    public static void LerpPosition(this Transform transform, Vector3 to, float t)
    {
        transform.position = Vector3.Lerp(transform.position, to, t);
    }

    public static void LerpPosition(this Transform transform, Vector2 to, float t)
    {
        _tempV3A.Set(to.x, to.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _tempV3A, t);
    }

    public static void LerpPosition(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.position = Vector3.Lerp(from, to, t);
    }

    public static void LerpPosition(this Transform transform, Vector2 from, Vector2 to, float t)
    {
        _tempV3A.Set(from.x, from.y, transform.position.z);
        _tempV3B.Set(to.x, to.y, transform.position.z);
        transform.position = Vector3.Lerp(_tempV3A, _tempV3B, t);
    }

    public static void LerpPositionX(this Transform transform, float to, float t)
    {
        transform.SetPositionX(Mathf.Lerp(transform.position.x, to, t));
    }

    public static void LerpPositionY(this Transform transform, float to, float t)
    {
        transform.SetPositionY(Mathf.Lerp(transform.position.y, to, t));
    }

    public static void LerpPositionZ(this Transform transform, float to, float t)
    {
        transform.SetPositionZ(Mathf.Lerp(transform.position.z, to, t));
    }

    public static void LerpPositionX(this Transform transform, float from, float to, float t)
    {
        transform.SetPositionX(Mathf.Lerp(from, to, t));
    }

    public static void LerpPositionY(this Transform transform, float from, float to, float t)
    {
        transform.SetPositionY(Mathf.Lerp(from, to, t));
    }

    public static void LerpPositionZ(this Transform transform, float from, float to, float t)
    {
        transform.SetPositionZ(Mathf.Lerp(from, to, t));
    }

    #endregion

    #region LerpLocalPosition

    public static void LerpLocalPosition(this Transform transform, Vector3 to, float t)
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, to, t);
    }

    public static void LerpLocalPosition(this Transform transform, Vector2 to, float t)
    {
        _tempV3A.Set(to.x, to.y, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, _tempV3A, t);
    }

    public static void LerpLocalPosition(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.localPosition = Vector3.Lerp(from, to, t);
    }

    public static void LerpLocalPosition(this Transform transform, Vector2 from, Vector2 to, float t)
    {
        _tempV3A.Set(from.x, from.y, transform.localPosition.z);
        _tempV3B.Set(to.x, to.y, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(_tempV3A, _tempV3B, t);
    }

    public static void LerpLocalPositionX(this Transform transform, float to, float t)
    {
        transform.SetLocalPositionX(Mathf.Lerp(transform.localPosition.x, to, t));
    }

    public static void LerpLocalPositionY(this Transform transform, float to, float t)
    {
        transform.SetLocalPositionY(Mathf.Lerp(transform.localPosition.y, to, t));
    }

    public static void LerpLocalPositionZ(this Transform transform, float to, float t)
    {
        transform.SetLocalPositionZ(Mathf.Lerp(transform.localPosition.z, to, t));
    }

    public static void LerpLocalPositionX(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalPositionX(Mathf.Lerp(from, to, t));
    }

    public static void LerpLocalPositionY(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalPositionY(Mathf.Lerp(from, to, t));
    }

    public static void LerpLocalPositionZ(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalPositionZ(Mathf.Lerp(from, to, t));
    }

    #endregion

    #region LerpRotate

    public static void LerpRotate(this Transform transform, Quaternion to, float t)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, to, t);
    }

    public static void LerpRotate(this Transform transform, Vector3 to, float t)
    {
        transform.LerpRotate(Quaternion.Euler(to), t);
    }

    public static void LerpRotate(this Transform transform, Quaternion from, Quaternion to, float t)
    {
        transform.rotation = Quaternion.Lerp(from, to, t);
    }

    public static void LerpRotate(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.LerpRotate(Quaternion.Euler(from), Quaternion.Euler(to), t);
    }

    public static void LerpEulerAnglesX(this Transform transform, float to, float t)
    {
        transform.SetEulerAnglesX(Mathf.LerpAngle(transform.eulerAngles.x, to, t));
    }

    public static void LerpEulerAnglesY(this Transform transform, float to, float t)
    {
        transform.SetEulerAnglesY(Mathf.LerpAngle(transform.eulerAngles.y, to, t));
    }

    public static void LerpEulerAnglesZ(this Transform transform, float to, float t)
    {
        transform.SetEulerAnglesZ(Mathf.LerpAngle(transform.eulerAngles.z, to, t));
    }

    public static void LerpEulerAnglesX(this Transform transform, float from, float to, float t)
    {
        transform.SetEulerAnglesX(Mathf.LerpAngle(from, to, t));
    }

    public static void LerpEulerAnglesY(this Transform transform, float from, float to, float t)
    {
        transform.SetEulerAnglesY(Mathf.LerpAngle(from, to, t));
    }

    public static void LerpEulerAnglesZ(this Transform transform, float from, float to, float t)
    {
        transform.SetEulerAnglesZ(Mathf.LerpAngle(from, to, t));
    }

    #endregion

    #region LerpLocalRotate

    public static void LerpLocalRotate(this Transform transform, Quaternion to, float t)
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, to, t);
    }

    public static void LerpLocalRotate(this Transform transform, Vector3 to, float t)
    {
        transform.LerpLocalRotate(Quaternion.Euler(to), t);
    }

    public static void LerpLocalRotate(this Transform transform, Quaternion from, Quaternion to, float t)
    {
        transform.localRotation = Quaternion.Lerp(from, to, t);
    }

    public static void LerpLocalRotate(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.LerpLocalRotate(Quaternion.Euler(from), Quaternion.Euler(to), t);
    }

    public static void LerpLocalEulerAnglesX(this Transform transform, float to, float t)
    {
        transform.SetLocalEulerAnglesX(Mathf.LerpAngle(transform.localEulerAngles.x, to, t));
    }

    public static void LerpLocalEulerAnglesY(this Transform transform, float to, float t)
    {
        transform.SetLocalEulerAnglesY(Mathf.LerpAngle(transform.localEulerAngles.y, to, t));
    }

    public static void LerpLocalEulerAnglesZ(this Transform transform, float to, float t)
    {
        transform.SetLocalEulerAnglesZ(Mathf.LerpAngle(transform.localEulerAngles.z, to, t));
    }

    public static void LerpLocalEulerAnglesX(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalEulerAnglesX(Mathf.LerpAngle(from, to, t));
    }

    public static void LerpLocalEulerAnglesY(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalEulerAnglesY(Mathf.LerpAngle(from, to, t));
    }

    public static void LerpLocalEulerAnglesZ(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalEulerAnglesZ(Mathf.LerpAngle(from, to, t));
    }

    #endregion

    #region SlerpRotate

    public static void SlerpRotate(this Transform transform, Quaternion to, float t)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, to, t);
    }

    public static void SlerpRotate(this Transform transform, Vector3 to, float t)
    {
        transform.SlerpRotate(Quaternion.Euler(to), t);
    }

    public static void SlerpRotate(this Transform transform, Quaternion from, Quaternion to, float t)
    {
        transform.rotation = Quaternion.Slerp(from, to, t);
    }

    public static void SlerpRotate(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.SlerpRotate(Quaternion.Euler(from), Quaternion.Euler(to), t);
    }

    #endregion

    #region SlerpLocalRotate

    public static void SlerpLocalRotate(this Transform transform, Quaternion to, float t)
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, to, t);
    }

    public static void SlerpLocalRotate(this Transform transform, Vector3 to, float t)
    {
        transform.SlerpLocalRotate(Quaternion.Euler(to), t);
    }

    public static void SlerpLocalRotate(this Transform transform, Quaternion from, Quaternion to, float t)
    {
        transform.localRotation = Quaternion.Slerp(from, to, t);
    }

    public static void SlerpLocalRotate(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.SlerpLocalRotate(Quaternion.Euler(from), Quaternion.Euler(to), t);
    }

    #endregion

    #region LerpLocalScale

    public static void LerpLocalScale(this Transform transform, Vector3 to, float t)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, to, t);
    }

    public static void LerpLocalScale(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        transform.localScale = Vector3.Lerp(from, to, t);
    }

    public static void LerpLocalScaleX(this Transform transform, float to, float t)
    {
        transform.SetLocalScaleX(Mathf.Lerp(transform.localScale.x, to, t));
    }

    public static void LerpLocalScaleY(this Transform transform, float to, float t)
    {
        transform.SetLocalScaleY(Mathf.Lerp(transform.localScale.y, to, t));
    }

    public static void LerpLocalScaleZ(this Transform transform, float to, float t)
    {
        transform.SetLocalScaleZ(Mathf.Lerp(transform.localScale.z, to, t));
    }

    public static void LerpLocalScaleX(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalScaleX(Mathf.Lerp(from, to, t));
    }

    public static void LerpLocalScaleY(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalScaleY(Mathf.Lerp(from, to, t));
    }

    public static void LerpLocalScaleZ(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalScaleZ(Mathf.Lerp(from, to, t));
    }

    #endregion

    #region SmoothStepPosition

    public static void SmoothStepPosition(this Transform transform, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(transform.position.x, to.x, t);
        var newY = Mathf.SmoothStep(transform.position.y, to.y, t);
        var newZ = Mathf.SmoothStep(transform.position.z, to.z, t);
        transform.SetPosition(newX, newY, newZ);
    }

    public static void SmoothStepPosition(this Transform transform, Vector2 to, float t)
    {
        _tempV3A.Set(to.x, to.y, transform.position.z);
        transform.SmoothStepPosition(_tempV3A, t);
    }

    public static void SmoothStepPosition(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(from.x, to.x, t);
        var newY = Mathf.SmoothStep(from.y, to.y, t);
        var newZ = Mathf.SmoothStep(from.z, to.z, t);
        transform.SetPosition(newX, newY, newZ);
    }

    public static void SmoothStepPosition(this Transform transform, Vector2 from, Vector2 to, float t)
    {
        _tempV3A.Set(from.x, from.y, transform.position.z);
        _tempV3B.Set(to.x, to.y, transform.position.z);
        transform.SmoothStepPosition(_tempV3A, _tempV3B, t);
    }

    public static void SmoothStepPositionX(this Transform transform, float to, float t)
    {
        transform.SetPositionX(Mathf.SmoothStep(transform.position.x, to, t));
    }

    public static void SmoothStepPositionY(this Transform transform, float to, float t)
    {
        transform.SetPositionY(Mathf.SmoothStep(transform.position.y, to, t));
    }

    public static void SmoothStepPositionZ(this Transform transform, float to, float t)
    {
        transform.SetPositionZ(Mathf.SmoothStep(transform.position.z, to, t));
    }

    public static void SmoothStepPositionX(this Transform transform, float from, float to, float t)
    {
        transform.SetPositionX(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepPositionY(this Transform transform, float from, float to, float t)
    {
        transform.SetPositionY(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepPositionZ(this Transform transform, float from, float to, float t)
    {
        transform.SetPositionZ(Mathf.SmoothStep(from, to, t));
    }

    #endregion

    #region SmoothStepLocalPosition

    public static void SmoothStepLocalPosition(this Transform transform, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(transform.localPosition.x, to.x, t);
        var newY = Mathf.SmoothStep(transform.localPosition.y, to.y, t);
        var newZ = Mathf.SmoothStep(transform.localPosition.z, to.z, t);
        transform.SetLocalPosition(newX, newY, newZ);
    }

    public static void SmoothStepLocalPosition(this Transform transform, Vector2 to, float t)
    {
        _tempV3A.Set(to.x, to.y, transform.localPosition.z);
        transform.SmoothStepLocalPosition(_tempV3A, t);
    }

    public static void SmoothStepLocalPosition(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(from.x, to.x, t);
        var newY = Mathf.SmoothStep(from.y, to.y, t);
        var newZ = Mathf.SmoothStep(from.z, to.z, t);
        transform.SetLocalPosition(newX, newY, newZ);
    }

    public static void SmoothStepLocalPosition(this Transform transform, Vector2 from, Vector2 to, float t)
    {
        _tempV3A.Set(from.x, from.y, transform.localPosition.z);
        _tempV3B.Set(to.x, to.y, transform.localPosition.z);
        transform.SmoothStepLocalPosition(_tempV3A, _tempV3B, t);
    }

    public static void SmoothStepLocalPositionX(this Transform transform, float to, float t)
    {
        transform.SetLocalPositionX(Mathf.SmoothStep(transform.localPosition.x, to, t));
    }

    public static void SmoothStepLocalPositionY(this Transform transform, float to, float t)
    {
        transform.SetLocalPositionY(Mathf.SmoothStep(transform.localPosition.y, to, t));
    }

    public static void SmoothStepLocalPositionZ(this Transform transform, float to, float t)
    {
        transform.SetLocalPositionZ(Mathf.SmoothStep(transform.localPosition.z, to, t));
    }

    public static void SmoothStepLocalPositionX(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalPositionX(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepLocalPositionY(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalPositionY(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepLocalPositionZ(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalPositionZ(Mathf.SmoothStep(from, to, t));
    }

    #endregion

    #region SmoothStepEulerAngles

    public static void SmoothStepEulerAngles(this Transform transform, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(transform.eulerAngles.x, to.x, t);
        var newY = Mathf.SmoothStep(transform.eulerAngles.y, to.y, t);
        var newZ = Mathf.SmoothStep(transform.eulerAngles.z, to.z, t);
        transform.SetEulerAngles(newX, newY, newZ);
    }

    public static void SmoothStepEulerAngles(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(from.x, to.x, t);
        var newY = Mathf.SmoothStep(from.y, to.y, t);
        var newZ = Mathf.SmoothStep(from.z, to.z, t);
        transform.SetEulerAngles(newX, newY, newZ);
    }

    public static void SmoothStepEulerAnglesX(this Transform transform, float to, float t)
    {
        transform.SetEulerAnglesX(Mathf.SmoothStep(transform.eulerAngles.x, to, t));
    }

    public static void SmoothStepEulerAnglesY(this Transform transform, float to, float t)
    {
        transform.SetEulerAnglesY(Mathf.SmoothStep(transform.eulerAngles.y, to, t));
    }

    public static void SmoothStepEulerAnglesZ(this Transform transform, float to, float t)
    {
        transform.SetEulerAnglesZ(Mathf.SmoothStep(transform.eulerAngles.z, to, t));
    }

    public static void SmoothStepEulerAnglesX(this Transform transform, float from, float to, float t)
    {
        transform.SetEulerAnglesX(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepEulerAnglesY(this Transform transform, float from, float to, float t)
    {
        transform.SetEulerAnglesY(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepEulerAnglesZ(this Transform transform, float from, float to, float t)
    {
        transform.SetEulerAnglesZ(Mathf.SmoothStep(from, to, t));
    }

    #endregion

    #region SmoothStepLocalEulerAngles

    public static void SmoothStepLocalEulerAngles(this Transform transform, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(transform.localEulerAngles.x, to.x, t);
        var newY = Mathf.SmoothStep(transform.localEulerAngles.y, to.y, t);
        var newZ = Mathf.SmoothStep(transform.localEulerAngles.z, to.z, t);
        transform.SetLocalEulerAngles(newX, newY, newZ);
    }

    public static void SmoothStepLocalEulerAngles(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(from.x, to.x, t);
        var newY = Mathf.SmoothStep(from.y, to.y, t);
        var newZ = Mathf.SmoothStep(from.z, to.z, t);
        transform.SetLocalEulerAngles(newX, newY, newZ);
    }

    public static void SmoothStepLocalEulerAnglesX(this Transform transform, float to, float t)
    {
        transform.SetLocalEulerAnglesX(Mathf.SmoothStep(transform.localEulerAngles.x, to, t));
    }

    public static void SmoothStepLocalEulerAnglesY(this Transform transform, float to, float t)
    {
        transform.SetLocalEulerAnglesY(Mathf.SmoothStep(transform.localEulerAngles.y, to, t));
    }

    public static void SmoothStepLocalEulerAnglesZ(this Transform transform, float to, float t)
    {
        transform.SetLocalEulerAnglesZ(Mathf.SmoothStep(transform.localEulerAngles.z, to, t));
    }

    public static void SmoothStepLocalEulerAnglesX(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalEulerAnglesX(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepLocalEulerAnglesY(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalEulerAnglesY(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepLocalEulerAnglesZ(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalEulerAnglesZ(Mathf.SmoothStep(from, to, t));
    }

    #endregion

    #region SmoothStepLocalScale

    public static void SmoothStepLocalScale(this Transform transform, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(transform.localScale.x, to.x, t);
        var newY = Mathf.SmoothStep(transform.localScale.y, to.y, t);
        var newZ = Mathf.SmoothStep(transform.localScale.z, to.z, t);
        transform.SetLocalScale(newX, newY, newZ);
    }

    public static void SmoothStepLocalScale(this Transform transform, Vector2 to, float t)
    {
        var newX = Mathf.SmoothStep(transform.localScale.x, to.x, t);
        var newY = Mathf.SmoothStep(transform.localScale.y, to.y, t);
        transform.SetLocalScale(newX, newY, transform.localScale.z);
    }

    public static void SmoothStepLocalScale(this Transform transform, Vector3 from, Vector3 to, float t)
    {
        var newX = Mathf.SmoothStep(from.x, to.x, t);
        var newY = Mathf.SmoothStep(from.y, to.y, t);
        var newZ = Mathf.SmoothStep(from.z, to.z, t);
        transform.SetLocalScale(newX, newY, newZ);
    }

    public static void SmoothStepLocalScale(this Transform transform, Vector2 from, Vector2 to, float t)
    {
        var newX = Mathf.SmoothStep(from.x, to.x, t);
        var newY = Mathf.SmoothStep(from.y, to.y, t);
        transform.SetLocalScale(newX, newY, transform.localScale.z);
    }

    public static void SmoothStepLocalScaleX(this Transform transform, float to, float t)
    {
        transform.SetLocalScaleX(Mathf.SmoothStep(transform.localScale.x, to, t));
    }

    public static void SmoothStepLocalScaleY(this Transform transform, float to, float t)
    {
        transform.SetLocalScaleY(Mathf.SmoothStep(transform.localScale.y, to, t));
    }

    public static void SmoothStepLocalScaleZ(this Transform transform, float to, float t)
    {
        transform.SetLocalScaleZ(Mathf.SmoothStep(transform.localScale.z, to, t));
    }

    public static void SmoothStepLocalScaleX(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalScaleX(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepLocalScaleY(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalScaleY(Mathf.SmoothStep(from, to, t));
    }

    public static void SmoothStepLocalScaleZ(this Transform transform, float from, float to, float t)
    {
        transform.SetLocalScaleZ(Mathf.SmoothStep(from, to, t));
    }

    #endregion

    #region ClampPosition

    public static void ClampPosition(this Transform transform, Vector3 min, Vector3 max)
    {
        var newX = Mathf.Clamp(transform.position.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.position.y, min.y, max.y);
        var newZ = Mathf.Clamp(transform.position.z, min.z, max.z);
        transform.SetPosition(newX, newY, newZ);
    }

    public static void ClampPosition(this Transform transform, Vector2 min, Vector2 max)
    {
        var newX = Mathf.Clamp(transform.position.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.position.y, min.y, max.y);
        transform.SetPosition(newX, newY);
    }

    public static void ClampPositionX(this Transform transform, float min, float max)
    {
        transform.SetPositionX(Mathf.Clamp(transform.position.x, min, max));
    }

    public static void ClampPositionY(this Transform transform, float min, float max)
    {
        transform.SetPositionY(Mathf.Clamp(transform.position.y, min, max));
    }

    public static void ClampPositionZ(this Transform transform, float min, float max)
    {
        transform.SetPositionZ(Mathf.Clamp(transform.position.z, min, max));
    }

    #endregion

    #region ClampLocalPosition

    public static void ClampLocalPosition(this Transform transform, Vector3 min, Vector3 max)
    {
        var newX = Mathf.Clamp(transform.localPosition.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.localPosition.y, min.y, max.y);
        var newZ = Mathf.Clamp(transform.localPosition.z, min.z, max.z);
        transform.SetLocalPosition(newX, newY, newZ);
    }

    public static void ClampLocalPosition(this Transform transform, Vector2 min, Vector2 max)
    {
        var newX = Mathf.Clamp(transform.localPosition.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.localPosition.y, min.y, max.y);
        transform.SetLocalPosition(newX, newY);
    }

    public static void ClampLocalPositionX(this Transform transform, float min, float max)
    {
        transform.SetLocalPositionX(Mathf.Clamp(transform.localPosition.x, min, max));
    }

    public static void ClampLocalPositionY(this Transform transform, float min, float max)
    {
        transform.SetLocalPositionY(Mathf.Clamp(transform.localPosition.y, min, max));
    }

    public static void ClampLocalPositionZ(this Transform transform, float min, float max)
    {
        transform.SetLocalPositionZ(Mathf.Clamp(transform.localPosition.z, min, max));
    }

    #endregion

    #region ClampEulerAngles

    public static void ClampEulerAngles(this Transform transform, Vector3 min, Vector3 max)
    {
        var newX = Mathf.Clamp(transform.eulerAngles.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.eulerAngles.y, min.y, max.y);
        var newZ = Mathf.Clamp(transform.eulerAngles.z, min.z, max.z);
        transform.SetEulerAngles(newX, newY, newZ);
    }

    public static void ClampEulerAnglesX(this Transform transform, float min, float max)
    {
        transform.SetEulerAnglesX(Mathf.Clamp(transform.eulerAngles.x, min, max));
    }

    public static void ClampEulerAnglesY(this Transform transform, float min, float max)
    {
        transform.SetEulerAnglesY(Mathf.Clamp(transform.eulerAngles.y, min, max));
    }

    public static void ClampEulerAnglesZ(this Transform transform, float min, float max)
    {
        transform.SetEulerAnglesZ(Mathf.Clamp(transform.eulerAngles.z, min, max));
    }

    #endregion

    #region ClampLocalEulerAngles

    public static void ClampLocalEulerAngles(this Transform transform, Vector3 min, Vector3 max)
    {
        var newX = Mathf.Clamp(transform.localEulerAngles.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.localEulerAngles.y, min.y, max.y);
        var newZ = Mathf.Clamp(transform.localEulerAngles.z, min.z, max.z);
        transform.SetLocalEulerAngles(newX, newY, newZ);
    }

    public static void ClampLocalEulerAnglesX(this Transform transform, float min, float max)
    {
        transform.SetLocalEulerAnglesX(Mathf.Clamp(transform.localEulerAngles.x, min, max));
    }

    public static void ClampLocalEulerAnglesY(this Transform transform, float min, float max)
    {
        transform.SetLocalEulerAnglesY(Mathf.Clamp(transform.localEulerAngles.y, min, max));
    }

    public static void ClampLocalEulerAnglesZ(this Transform transform, float min, float max)
    {
        transform.SetLocalEulerAnglesZ(Mathf.Clamp(transform.localEulerAngles.z, min, max));
    }

    #endregion

    #region ClampLocalScale

    public static void ClampLocalScale(this Transform transform, Vector3 min, Vector3 max)
    {
        var newX = Mathf.Clamp(transform.localScale.x, min.x, max.x);
        var newY = Mathf.Clamp(transform.localScale.y, min.y, max.y);
        var newZ = Mathf.Clamp(transform.localScale.z, min.z, max.z);
        transform.SetLocalScale(newX, newY, newZ);
    }

    public static void ClampLocalScaleX(this Transform transform, float min, float max)
    {
        transform.SetLocalScaleX(Mathf.Clamp(transform.localScale.x, min, max));
    }

    public static void ClampLocalScaleY(this Transform transform, float min, float max)
    {
        transform.SetLocalScaleY(Mathf.Clamp(transform.localScale.y, min, max));
    }

    public static void ClampLocalScaleZ(this Transform transform, float min, float max)
    {
        transform.SetLocalScaleZ(Mathf.Clamp(transform.localScale.z, min, max));
    }

    #endregion

    #region HasChanged

    public static void HasChanged(this Transform transform, Action changed)
    {
        if (transform.hasChanged)
        {
            changed();
            transform.hasChanged = false;
        }
    }

    static void HasChanged(this Transform transform, Action<Transform> changed)
    {
        if (transform.hasChanged)
        {
            changed(transform);
            transform.hasChanged = false;
        }
    }

    public static void HasChangedInChildren(this Transform transform, Action<Transform> changed)
    {
        var childs = transform.GetComponentsInChildren<Transform>();
        if (childs == null)
        {
            return;
        }
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].HasChanged(changed);
        }
    }

    public static void HasChangedInParent(this Transform transform, Action<Transform> changed)
    {
        var parents = transform.GetComponentsInParent<Transform>();
        if (parents == null)
        {
            return;
        }
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i].HasChanged(changed);
        }
    }

    #endregion

    #region LookAt2D

    public static void LookAt2D(this Transform transform, Transform target)
    {
        LookAt2D(transform, target.position, Vector3.forward, 0);
    }

    public static void LookAt2D(this Transform transform, Vector2 target)
    {
        LookAt2D(transform, target, Vector3.forward, 0);
    }

    public static void LookAt2D(this Transform transform, Transform target, float angle)
    {
        LookAt2D(transform, target.position, Vector3.forward, angle);
    }

    public static void LookAt2D(this Transform transform, Vector2 target, float angle)
    {
        LookAt2D(transform, target, Vector3.forward, angle);
    }

    public static void LookAt2D(this Transform transform, Transform target, Vector3 axis)
    {
        LookAt2D(transform, target.position, axis, 0);
    }

    public static void LookAt2D(this Transform transform, Vector2 target, Vector3 axis)
    {
        LookAt2D(transform, target, axis, 0);
    }

    public static void LookAt2D(this Transform transform, Transform target, Vector3 axis, float angle)
    {
        LookAt2D(transform, target.position, axis, angle);
    }

    public static void LookAt2D(this Transform transform, Vector2 target, Vector3 axis, float angle)
    {
        _tempV2.Set(target.x - transform.position.x, target.y - transform.position.y);
        angle = angle + Mathf.Atan2(_tempV2.y, _tempV2.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, axis);
    }

    #endregion

    #region Find

    public static Transform FindInChildren(this Transform self, string name)
    {
        int count = self.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = self.GetChild(i);
            if (child.name == name) return child;
            Transform subChild = child.FindInChildren(name);
            if (subChild != null) return subChild;
        }
        return null;
    }

    #endregion

}