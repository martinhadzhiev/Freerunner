
using System;
using UnityEngine;

[Serializable]
public class ObstacleSet
{
    public bool IsInUse
    {
        get => isInUse;
        set
        {
            isInUse = value;

            foreach (var obs in obstacles)
            {
                obs.SetActive(value);
            }
        }
    }
    public int level;
    public GameObject[] obstacles;
    private bool isInUse;
}