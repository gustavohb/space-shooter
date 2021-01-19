using UnityEngine;
using DG.Tweening;

public class PopupQuitWindowUI : MonoBehaviour
{

    [SerializeField] private Animator _animator;

    private void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    private void OnEnable()
    {
        OpenWindow();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
        _animator?.SetBool("open", true);
    }

    public void CloseWindow()
    {
        _animator?.SetBool("open", false);
    }

    public void Quit()
    {                
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
#else
            Application.Quit();
#endif
    }
}
