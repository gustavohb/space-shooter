using UnityEngine;
using System.Collections;

/// <summary>
/// Spread nway shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Spread nWay Shot")]
public class SpreadNwayShot : BaseShot
{
	// "Set a number of shot way."
	public int WayNum = 8;
	// "Set a center angle of shot. (0 to 360)"
	[Range(0f, 360f)]
	public float CenterAngle = 180f;
	// "Set a angle between bullet and next bullet. (0 to 360)"
	[Range(0f, 360f)]
	public float BetweenAngle = 10f;
	// "Set a difference speed between shot and next line shot."
	public float DiffSpeed = 0.5f;


	private IEnumerator m_Shot;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Shot()
	{
		if (m_Shot != null)
		{
			StopCoroutine(m_Shot);
		}

		m_Shot = ShotCoroutine();

		StartCoroutine(m_Shot);
	}

	public override void StopShot()
	{
		if (m_Shot != null)
		{
			StopCoroutine(m_Shot);

			m_Shot = null;
		}

		FinishedShot();
	}

	IEnumerator ShotCoroutine()
	{
		if (bulletNum <= 0 || bulletSpeed <= 0f || WayNum <= 0)
		{
			Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or WayNum is not set.");
			yield break;
		}
		if (_shooting)
		{
			yield break;
		}
		_shooting = true;

		int wayIndex = 0;

		AudioManager.Instance.PlaySound(shotSFX, transform.position);

		for (int i = 0; i < bulletNum; i++)
		{
			if (WayNum <= wayIndex)
			{
				wayIndex = 0;

				bulletSpeed -= DiffSpeed;
				while (bulletSpeed <= 0)
				{
					bulletSpeed += Mathf.Abs(DiffSpeed);
				}
			}

			var bullet = GetBullet(transform.position, transform.rotation);
			if (bullet == null)
			{
				break;
			}

			float baseAngle = WayNum % 2 == 0 ? CenterAngle - (BetweenAngle / 2f) : CenterAngle;

			float angle = Util.GetShiftedAngle(wayIndex, baseAngle, BetweenAngle);

			while (GameTime.isPaused)
			{
				yield return null;
			}


			ShotBullet(bullet, bulletSpeed, angle);

			AutoReleaseBulletGameObject(bullet.gameObject);

			wayIndex++;
		}

		FinishedShot();
	}
}