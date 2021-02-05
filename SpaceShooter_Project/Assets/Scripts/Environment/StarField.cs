using UnityEngine;

public class StarField : MonoBehaviour
{

    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private Transform[] _parallaxObjects;
    [SerializeField] private float _parallaxStrength = 0.1f;

    private Transform _playerTransform;

    private Camera _mainCamera;

    private void Start()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag(_playerTag);

        _mainCamera = Camera.main;

        if (playerGameObject)
        {
            _playerTransform = playerGameObject.transform;
        }
        // Resize starfield according to the screen size
        for (int i = 0; i < _parallaxObjects.Length; i++)
        {
            SpriteRenderer spriteRenderer = _parallaxObjects[i].GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) continue;
            float width = spriteRenderer.sprite.bounds.size.x;
            float height = spriteRenderer.sprite.bounds.size.y;

            float worldScreenHeight = _mainCamera.orthographicSize * 2.0f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            _parallaxObjects[i].localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);
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
