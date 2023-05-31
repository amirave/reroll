using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeTransition : MonoBehaviour
{
    [SerializeField] private Color _color;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnValidate()
    {
        var images = GetComponentsInChildren<InvertedMaskImage>();

        foreach (var im in images)
            im.color = _color;
    }
}
