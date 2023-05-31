using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu()]
public class DifficultySettings : ScriptableObject
{
    public Difficulty obstaclesPerPhase;
    public Difficulty timeBetweenObstacles;
    public Difficulty timeBetweenPhases;
}

[Serializable]
public struct Difficulty
{
    public float min;
    public float max;
    public float timeUntilMax;

    public float GetCurrent(float time)
    {
        return Mathf.Lerp(min, max, time / timeUntilMax);
    }
}
