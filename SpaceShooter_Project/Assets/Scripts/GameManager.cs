﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        BulletForParticles.Init();
        GameTime.isPaused = false;
    }
}
