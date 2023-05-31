using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurretFive : Turret
{
    public override int Level => 5;
    
    public override void Shoot()
    {
        var pos = transform.position + -0.8f * transform.localScale.x * _head.up + Random.Range(-0.3f, 0.3f) * _head.right;

        var obj = Instantiate(TurretManager.Instance.bulletPrefab,
            pos.Flatten(), Quaternion.identity);

        obj.GetComponent<Bullet>().SetDir(_head.up * (-1 * transform.localScale.x * _bulletSpeed));
    }
}