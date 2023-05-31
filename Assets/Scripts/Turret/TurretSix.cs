using System;
using UnityEngine;

public class TurretSix : Turret
{
    public override int Level => 6;
    
    public override void Shoot()
    {
        var obj = Instantiate(TurretManager.Instance.bulletPrefab,
            (transform.position + -1 * transform.localScale.x * _head.up).Flatten(), Quaternion.identity);

        obj.GetComponent<Bullet>().SetDir(_head.up * (-1 * transform.localScale.x * _bulletSpeed));
    }
}