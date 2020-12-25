﻿using UnityEngine;

public class PauseScreen : ExtendedCustomMonoBehavior
{

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        GameTime.isPaused = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        GameTime.isPaused = false;
    }
}