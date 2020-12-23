using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[AddComponentMenu("Game/Controller/Shot Controller")]
public class ShotController : ExtendedCustomMonoBehavior
{
    [Serializable]
    public class ShotInfo
    {
        public BaseShot shotObj;
        public float afterDelay;

        public bool finishShotSequence = false;
    }


    public bool startOnAwake = false;
    public float startOnAwakeDelay = 1f;

    public float startShotDelay = 1f;

    public bool loop = false;
    public bool atRandom = false;
    public List<ShotInfo> shotList = new List<ShotInfo>();
    private bool _shooting;

    private IEnumerator _shot;

    private int _nowIndex = 0;

    public UnityEvent OnShootingFinished;

    private List<ShotInfo> _tmpShotInfoList;

    IEnumerator Start()
    {
        if (startOnAwake)
        {
            if (0f < startOnAwakeDelay)
            {
                yield return new WaitForSeconds(startOnAwakeDelay);
            }
            StartShotRoutine();
        }
    }

    void OnDisable()
    {
        _shooting = false;
    }

    /// <summary>
    /// Start the shot routine.
    /// </summary>
    public void StartShotRoutine()
    {

        if (_shot != null)
        {
            StopCoroutine(_shot);
            _shooting = false;
        }

        _shot = ShotCoroutine();

        StartCoroutine(_shot);
    }

    IEnumerator ShotCoroutine()
    {
        if (shotList == null || shotList.Count <= 0)
        {
            Debug.LogWarning("Cannot shot because ShotList is not set.");
            yield break;
        }

        bool enableShot = false;
        for (int i = 0; i < shotList.Count; i++)
        {
            if (shotList[i].shotObj != null)
            {
                enableShot = true;
                break;
            }
        }

        bool enableDelay = false;
        for (int i = 0; i < shotList.Count; i++)
        {
            if (0f < shotList[i].afterDelay)
            {
                enableDelay = true;
                break;
            }
        }

        if (enableShot == false || enableDelay == false)
        {
            if (enableShot == false)
            {
                Debug.LogWarning("Cannot shot because all ShotObj of ShotList is not set.");
            }
            if (enableDelay == false)
            {
                Debug.LogWarning("Cannot shot because all AfterDelay of ShotList is zero.");
            }
            yield break;
        }

        if (_shooting)
        {
            yield break;
        }
        _shooting = true;


        _nowIndex = 0;

        if (_tmpShotInfoList == null || _tmpShotInfoList.Count == 0)
        {
            _tmpShotInfoList = new List<ShotInfo>(shotList);
        }


        if (0f < startShotDelay)
        {
            yield return new WaitForSeconds(startShotDelay);
        }

        while (true)
        {
            if (atRandom)
            {
                _nowIndex = UnityEngine.Random.Range(0, _tmpShotInfoList.Count);
            }

            if (_tmpShotInfoList[_nowIndex].shotObj != null)
            {
                _tmpShotInfoList[_nowIndex].shotObj.SetShotCtrl(this);
                _tmpShotInfoList[_nowIndex].shotObj.Shot();
            }

            if (0f < _tmpShotInfoList[_nowIndex].afterDelay)
            {
                yield return new WaitForSeconds(_tmpShotInfoList[_nowIndex].afterDelay);
            }



            if (_tmpShotInfoList[_nowIndex].finishShotSequence)
            {
                _tmpShotInfoList.RemoveAt(_nowIndex);
                break;
            }

            if (atRandom)
            {
                _tmpShotInfoList.RemoveAt(_nowIndex);

                if (_tmpShotInfoList.Count <= 0)
                {
                    if (loop)
                    {
                        _tmpShotInfoList = new List<ShotInfo>(shotList);
                    }
                    else
                    {
                        break;
                    }
                }

            }
            else
            {
                if (loop == false && _tmpShotInfoList.Count - 1 <= _nowIndex)
                {
                    break;
                }

                _nowIndex = (int)Mathf.Repeat(_nowIndex + 1f, _tmpShotInfoList.Count);
            }
        }

        OnShootingFinished?.Invoke();

        _shooting = false;
    }

    public void StopShotRoutine()
    {

        if (_shot != null)
        {
            StopCoroutine(_shot);
            _shot = null;

        }

        if (_tmpShotInfoList != null && _tmpShotInfoList.Count != 0)
        {
            for (int i = 0; i < _tmpShotInfoList.Count; i++)
            {
                _tmpShotInfoList[i].shotObj.StopShot();
            }
        }

        OnShootingFinished?.Invoke();

        _shooting = false;
    }

}
