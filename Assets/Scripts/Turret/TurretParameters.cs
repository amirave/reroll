
using System;
using UnityEngine;

[Serializable]
public struct TurretParameters
{
    public GameObject prefab;
    public TurretRotationMode mode;
    public float _rotationSpeed;
    public float _firingRate;
}

public enum TurretRotationMode
{
    Random,
    Follow
}
