using UnityEngine;

public static class GameTime
{
    public static bool isPaused = false;
    public static float deltaTime { get { return isPaused ? 0 : Time.deltaTime; } }
}