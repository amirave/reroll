using System;
using UnityEngine;

public class TurretTwo : Turret
{
    public override int Level => 2;
    
    public override void Shoot()
    {
        for (int i = 0; i < 2; i++)
        {
            var pos = transform.position + -0.8f * transform.localScale.x * (i - 0.5f) * 0.3f * _head.right;

            var obj = Instantiate(TurretManager.Instance.bulletPrefab,
                pos.Flatten(), Quaternion.identity);

            obj.GetComponent<Bullet>().SetDir(_head.up * (-1 * transform.localScale.x * _bulletSpeed));
        }
    }
}