using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _dir;

    private Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position += _dir * Time.deltaTime;
    }

    public void SetDir(Vector3 v)
    {
        _dir = v.Flatten();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage();
            // TODO indicator
            Destroy(gameObject);
        }
    }
}
