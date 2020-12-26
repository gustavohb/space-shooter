using UnityEngine;

/// <summary>
/// Hole circle shot.
/// </summary>
[AddComponentMenu("Game/Shot Pattern/Hole Circle Shot")]
public class HoleCircleShot : BaseShot
{
	// "Set a center angle of hole. (0 to 360)"
	[Range(0f, 360f)]
	public float holeCenterAngle = 180f;
	// "Set a size of hole. (0 to 360)"
	[Range(0f, 360f)]
	public float holeSize = 20f;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Shot()
	{
		if (bulletNum <= 0 || bulletSpeed <= 0f)
		{
			Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
			return;
		}

		holeCenterAngle = Util.Get360Angle(holeCenterAngle);
		float startAngle = holeCenterAngle - (holeSize / 2f);
		float endAngle = holeCenterAngle + (holeSize / 2f);

		float shiftAngle = 360f / (float)bulletNum;

		AudioManager.Instance.PlaySound(shotSFX, transform.position);

		for (int i = 0; i < bulletNum; i++)
		{
			float angle = shiftAngle * i;
			if (startAngle <= angle && angle <= endAngle)
			{
				continue;
			}

			var bullet = GetBullet(transform.position, transform.rotation);
			if (bullet == null)
			{
				break;
			}

			ShotBullet(bullet, bulletSpeed, angle);
		}

		FinishedShot();
	}

	public override void StopShot()
	{
		// Do nothing
	}
}