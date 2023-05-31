
using System;
using UnityEngine;

[CreateAssetMenu]
public class DiceLandParameters : ScriptableObject
{
    public float displayOffset = 1;
    public float fadeInTime = 0.3f;
    public float fadeOutTime = 0.3f;

    public float timeToActivation = 2f;

    public string landSfx = "";
    public string activateSfx = "";
    
    public Color startColor;
    public Color endColor;
}
