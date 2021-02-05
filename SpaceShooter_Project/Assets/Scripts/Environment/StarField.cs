using UnityEngine;

public class StarField : MonoBehaviour
{

    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private Transform[] _parallaxObjects;
    [SerializeField] private float _parallaxStrength = 0.1f;

    private Transform _playerTransform;

    private void Start()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag(_playerTag);

        if (playerGameObject)
        {
            _playerTransform = playerGameObject.transform;
        }
    }

    private void Update()
    {
        if (_playerTransform)
        {
            for (int i = 0; i < _parallaxObjects.Length; i++)
            {
                Vector3 newPosition = _parallaxObjects[i].position;

                newPosition.x = -_playerTransform.position.x * _parallaxStrength / (i + 1);
                newPosition.y = -_playerTransform.position.y * _parallaxStrength / (i + 1);

                _parallaxObjects[i].position = newPosition;
            }
        }
    }
}
