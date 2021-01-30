using UnityEngine;

public class EreaseSavePopupWindow : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySound2D(SoundLibrary.Sound.ClickButton01);
            Close();
        }
    }

    public void Close()
    {
        if (_animator != null)
        {
            _animator.SetBool("close", true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void EreaseSave()
    {
        GameDataController.EreaseSaveData();
        Close();
    }
}
