
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Turret : MonoBehaviour
{
    private Player _player => GameScope.Instance.player;
    public abstract int Level { get; }

    [SerializeField] private TurretRotationMode mode;
    [SerializeField] protected float _rotationSpeed;
    [SerializeField] protected float _firingRate;
    [SerializeField] protected float _bulletSpeed;
    
    [SerializeField] protected Transform _head;
    [SerializeField] protected Transform _base;

    private float _lastFired = -10;

    private void Awake()
    {
        _base.transform.Rotate(transform.up, Random.Range(0, 360f));
        _lastFired = Time.time;
    }

    void Update()
    {
        var angle = Vector2.SignedAngle(_head.up * -1, (_player.transform.position - transform.position));
        angle = Mathf.Clamp(angle, -1 * _rotationSpeed, _rotationSpeed);
        
        if (mode == TurretRotationMode.Follow)
        {
            _head.transform.Rotate(transform.up, angle * Time.deltaTime);
        }
        else
        {
            _head.Rotate(transform.up, _rotationSpeed * Time.deltaTime);
        }

        if (Time.time - _lastFired > _firingRate &&
            ((mode == TurretRotationMode.Follow && Mathf.Abs(angle) < _rotationSpeed) || mode == TurretRotationMode.Random))
        {
            Shoot();
            ShootSound();
            _lastFired = Time.time;
        }
    }

    public abstract void Shoot();

    public void ShootSound()
    {
        AudioManager.Instance.PlayEffectRandom("shoot");
    }
}

public class TurretFactory
{
    private Dictionary<int, Type> turretByLevel;

    public TurretFactory()
    {
        var turretTypes = Assembly.GetAssembly(typeof(Turret)).GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Turret)));

        turretByLevel = new Dictionary<int, Type>();

        var i = 1;
        foreach (var type in turretTypes)
        {
            turretByLevel.Add(i, type);
            i++;
        }
    }
}
