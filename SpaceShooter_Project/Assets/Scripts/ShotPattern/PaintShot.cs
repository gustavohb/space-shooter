using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Paint shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Paint Shot")]
public class PaintShot : BaseShot
{
    static readonly string[] SPLIT_VAL = { "\n", "\r", "\r\n" };

    // "Set a paint data text file. (ex.[UniBulletHell] > [Example] > [PaintShotData] in Project view)"
    // "BulletNum is ignored."
    public TextAsset paintDataText;
    // "Set a center angle of shot. (0 to 360) (center of first line)"
    [Range(0f, 360f)]
    public float paintCenterAngle = 180f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f)]
    public float betweenAngle = 3f;
    // "Set a delay time between shot and next line shot. (sec)"
    public float nextLineDelay = 0.1f;

    private IEnumerator _shot;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Shot()
    {
        if (_shot != null)
        {
            StopCoroutine(_shot);
        }

        _shot = ShotCoroutine();

        StartCoroutine(_shot);
    }

    public override void StopShot()
    {
        if (_shot != null)
        {
            StopCoroutine(_shot);

            _shot = null;
        }

        FinishedShot();
    }


    IEnumerator ShotCoroutine()
    {
        if (bulletSpeed <= 0f || paintDataText == null)
        {
            Debug.LogWarning("Cannot shot because BulletSpeed or PaintDataText is not set.");
            yield break;
        }
        if (_shooting)
        {
            yield break;
        }
        _shooting = true;

        var paintData = LoadPaintData();

        float paintStartAngle = paintCenterAngle;
        if (0 < paintData.Count)
        {
            paintStartAngle -= paintData[0].Count % 2 == 0 ?
                (betweenAngle * paintData[0].Count / 2f) + (betweenAngle / 2f) :
                 betweenAngle * Mathf.Floor(paintData[0].Count / 2f);
        }

        for (int lineCnt = 0; lineCnt < paintData.Count; lineCnt++)
        {
            var line = paintData[lineCnt];
            if (0 < lineCnt && 0 < nextLineDelay)
            {
                yield return new WaitForSeconds(nextLineDelay);
            }

            for (int i = 0; i < line.Count; i++)
            {
                if (line[i] == 1)
                {
                    var bullet = GetBullet(transform.position, transform.rotation);
                    if (bullet == null)
                    {
                        break;
                    }

                    float angle = paintStartAngle + (betweenAngle * i);
                    bullet.targetTag = targetTagName;

                    while (GameTime.isPaused)
                    {
                        yield return null;
                    }

                    ShotBullet(bullet, bulletSpeed, angle);
                }
            }


            AudioManager.Instance.PlaySound(shotSFX, transform.position);

        }

        FinishedShot();
    }

    List<List<int>> LoadPaintData()
    {
        var paintData = new List<List<int>>();

        if (string.IsNullOrEmpty(paintDataText.text))
        {
            Debug.LogWarning("Cannot load paint data because PaintDataText file is empty.");
            return paintData;
        }

        string[] lines = paintDataText.text.Split(SPLIT_VAL, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            // lines beginning with "#" are ignored as comments.
            if (lines[i].StartsWith("#"))
            {
                continue;
            }
            // add line
            paintData.Add(new List<int>());

            for (int j = 0; j < lines[i].Length; j++)
            {
                // bullet is fired into position of "*".
                paintData[paintData.Count - 1].Add(lines[i][j] == '*' ? 1 : 0);
            }
        }

        // reverse because fire from bottom left.
        paintData.Reverse();

        return paintData;
    }
}