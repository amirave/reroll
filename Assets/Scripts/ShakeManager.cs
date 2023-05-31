using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public static ShakeManager Instance {get; private set;}

    public float recoveryRate;
    public float magnitude = 0.1f;

    private float _trauma;
    private Camera _mainCam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (GameScope.Instance.isPaused)
            return;
        
        _trauma -= recoveryRate * Time.deltaTime;
        _trauma = Mathf.Clamp01(_trauma);
        
        _mainCam.transform.localPosition = Utils.RandomDirection() * (_trauma * _trauma * magnitude) + _mainCam.transform.position.z * Vector3.forward;
    }

    public void AddShake(float intensity)
    {
        _trauma += intensity;
    }
}
