using UnityEngine;
using System;

[Serializable]
public class ItemDrop
{
    public GameObject item;

    [Range(0, 1)]
    public float dropChance = 0.2f;

    public int dropMin = 1;

    public int dropMax = 1;

    public float dropSpread = 3f;
}
