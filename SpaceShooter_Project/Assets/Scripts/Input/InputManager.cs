using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : ExtendedCustomMonoBehavior
{

    [SerializeField] private Camera _targetCamera;

    [SerializeField] private Transform _joystickInnerCircle;
    [SerializeField] private Transform _joystickOuterCircle;
    [SerializeField] private GameObject _joystickDisable;

    private bool _touchStart = false;
    private Vector2 _touchPointA;
    private Vector2 _touchPointB;
    private Vector2 _startingPoint;
    private int _touchId = 99;

    private PlayerMovement _player;

    private void Start()
    {
        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }


        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMovement>();
#if !UNITY_ANDROID && !UNITY_EDITOR && !UNITY_WEBGL
        _joystickInnerCircle.GetComponent<SpriteRenderer>().enabled = false;
        _joystickOuterCircle.GetComponent<SpriteRenderer>().enabled = false;
#endif
#if UNITY_WEBGL
        _joystickDisable?.SetActive(false);
#endif
    }

    private void Update()
    {
        if (GameTime.isPaused)
        {
            _joystickInnerCircle.GetComponent<SpriteRenderer>().enabled = false;
            _joystickOuterCircle.GetComponent<SpriteRenderer>().enabled = false;

            _joystickDisable?.SetActive(false);

            return;
        }


#if UNITY_EDITOR || UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
        {
            _touchPointA = TouchToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            _joystickInnerCircle.transform.position = _touchPointA;
            _joystickOuterCircle.transform.position = _touchPointA;

            _joystickInnerCircle.GetComponent<SpriteRenderer>().enabled = true;
            _joystickOuterCircle.GetComponent<SpriteRenderer>().enabled = true;
            _joystickDisable?.SetActive(false);
        }
        
        if (Input.GetMouseButton(0))
        {
            _touchStart = true;
            _touchPointB = TouchToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        else
        {
            _touchStart = false;
        }

        float moveX = 0f;
        float moveY = 0f;

        if (!_touchStart)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1f;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveX = -1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveX = 1f;
            }

            Vector2 moveDir = new Vector2(moveX, moveY).normalized;

            if (_player != null)
            {
                _player.Move(moveDir);
            }
        }


#elif UNITY_ANDROID        
       
        int i = 0;
        while(i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = TouchToWorldPoint(touch.position);


            if (touch.phase == TouchPhase.Began && _touchId == 99)
            {
                _touchStart = true;
                _touchPointA = touchPos;
                _touchPointB = touchPos;

                _touchId = touch.fingerId;
                _startingPoint = touchPos;

                _joystickOuterCircle.position = _touchPointA;
                _joystickInnerCircle.position = _touchPointA;

                _joystickInnerCircle.GetComponent<SpriteRenderer>().enabled = true;
                _joystickOuterCircle.GetComponent<SpriteRenderer>().enabled = true;
                _joystickDisable?.SetActive(false);


            }
            else if (touch.phase == TouchPhase.Moved && _touchId == touch.fingerId)
            {
                _touchPointB = touchPos;                
            }

            else if(touch.phase == TouchPhase.Ended && _touchId == touch.fingerId)
            {
                _touchId = 99;
                _touchStart = false;
                _joystickDisable?.SetActive(true);
            }
            i++;
        }

        if ((_touchId != 99 && Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(_touchId).fingerId)) || EventSystem.current.IsPointerOverGameObject())
        {
            _joystickInnerCircle.GetComponent<SpriteRenderer>().enabled = false;
            _joystickOuterCircle.GetComponent<SpriteRenderer>().enabled = false;
            _joystickDisable?.SetActive(true);
        }
#endif

    }

    private Vector2 TouchToWorldPoint(Vector2 touchPosition)
    {
        return _targetCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, _targetCamera.transform.position.z));
    }


    private void FixedUpdate()
    {
        if (GameTime.isPaused)
        {
            return;
        }


#if UNITY_EDITOR || UNITY_ANDROID || UNITY_WEBGL
        if (_touchStart)
        {
            Vector2 offset = _touchPointB - _touchPointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            if (_player != null) 
            {
                _player.Move(direction);
            }

            direction = Vector2.ClampMagnitude(offset, 3.3f);

            if (offset.magnitude > 3.0f)
            {
                _touchPointA += 5.0f * direction * GameTime.deltaTime;
                _joystickOuterCircle.position = _touchPointA;
            }

            _joystickInnerCircle.position = new Vector2(_touchPointA.x + direction.x, _touchPointA.y + direction.y);

        }
        else
        {
            if (_player != null)
            {
                _player.Move(Vector2.zero);
            }
            _joystickInnerCircle.GetComponent<SpriteRenderer>().enabled = false;
            _joystickOuterCircle.GetComponent<SpriteRenderer>().enabled = false;

            _joystickDisable?.SetActive(true);

        }
#endif

    }

}
