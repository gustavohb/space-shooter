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

		float myBulletSpeed = bulletSpeed;

		AudioManager.Instance.PlaySound(shotSFX, transform.position);

		for (int i = 0; i < bulletNum; i++)
		{
			if (WayNum <= wayIndex)
			{
				wayIndex = 0;

				myBulletSpeed -= DiffSpeed;
				while (myBulletSpeed <= 0)
				{
					myBulletSpeed += Mathf.Abs(DiffSpeed);
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


			ShotBullet(bullet, myBulletSpeed, angle);

			wayIndex++;
		}

		FinishedShot();
	}
}