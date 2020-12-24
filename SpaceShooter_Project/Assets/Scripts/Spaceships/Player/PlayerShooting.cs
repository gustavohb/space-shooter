using UnityEngine;

public class PlayerShooting : ExtendedCustomMonoBehavior
{
    [SerializeField] private PlayerWeapon[] _weapons;

    [SerializeField] private int _weaponIndex = 0;

    public int WeaponIndex => _weaponIndex;

    private PlayerWeapon _currentWeapon;

    private Transform _currentTarget;

    public string targetTag = "Enemy";

    [SerializeField] private float _findClosestTargetDelay = 1.5f;
    private float _findClosestTargetTimer = 0.0f;

    [SerializeField] private Rect _findTargetArea;

    private void Awake()
    {
        SetWeapon(0);
    }

    private void Update()
    {
        HandleShootBlasterProjectile();
    }

    public void SetWeapon(int setValue)
    {
        if (_weaponIndex >= 0)
        {
            if (_currentWeapon)
            {
                Destroy(_currentWeapon.gameObject);
            }

            if (setValue >= 0 && setValue < _weapons.Length)
            {
                _currentWeapon = Instantiate(_weapons[setValue], transform.position, Quaternion.identity);
                _currentWeapon.transform.parent = transform;
                _currentWeapon.targetTag = targetTag;
            }
        }
    }

    public void FindClosestTarget()
    {
        _currentTarget = null;

        foreach (GameObject possibleTarget in GameObject.FindGameObjectsWithTag(targetTag))
        {
            if (_currentTarget == null)
            {
                if (possibleTarget.transform.position.x < _findTargetArea.width && possibleTarget.transform.position.x > _findTargetArea.x && possibleTarget.transform.position.y < _findTargetArea.height && possibleTarget.transform.position.y > _findTargetArea.y)
                {
                    _currentTarget = possibleTarget.transform;
                }
            }
            else if (possibleTarget.transform.position.x < _findTargetArea.width && possibleTarget.transform.position.x > _findTargetArea.x && possibleTarget.transform.position.y < _findTargetArea.height && possibleTarget.transform.position.y > _findTargetArea.y)
            {
                if (Vector3.Distance(transform.position, possibleTarget.transform.position) < Vector3.Distance(transform.position, _currentTarget.position))
                {
                    _currentTarget = possibleTarget.transform;
                }
            }
        }

        if (_currentTarget != null && _currentWeapon != null)
        {
            _currentWeapon.SetTarget(_currentTarget);
        }

        if (_currentTarget == null && _currentWeapon != null)
        {
            _currentWeapon?.GetComponent<PlayerWeapon>().StopShooting();
        }
        else
        {
            _currentWeapon?.GetComponent<PlayerWeapon>().StartShooting();
        }

        if (_currentTarget && _currentWeapon)
        {
            _currentWeapon.GetComponent<PlayerWeapon>().SetTarget(_currentTarget);
        }
    }

    private void HandleShootBlasterProjectile()
    {

        _findClosestTargetTimer += GameTime.deltaTime;

        if (_findClosestTargetTimer > _findClosestTargetDelay || _currentTarget == null)
        {
            FindClosestTarget();
            _findClosestTargetTimer = 0.0f;
        }
    }

    public void ChangeWeapon(int changeValue)
    {
        if (_weaponIndex + changeValue < _weapons.Length)
        {
            _weaponIndex += changeValue;

            SetWeapon(_weaponIndex);
        }
    }
}
