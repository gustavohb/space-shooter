using UnityEngine;

public class GameUICanvas : ExtendedCustomMonoBehavior
{
    [SerializeField] private GameObject _pauseScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseScreen();
        }
    }

    public void OpenPauseScreen()
    {
        if (_pauseScreen != null && !_pauseScreen.activeSelf)
        {
            _pauseScreen.SetActive(true);
        }
    }
}
