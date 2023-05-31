using System;
using UnityEngine;

public class TurretFour : Turret
{
    public override int Level => 4;
    
    public override void Shoot()
    {
        for (int i = 0; i < 4; i++)
        {
            var angle = Quaternion.AngleAxis(i * 90 + 45, transform.up);
            var pos = transform.position + angle * _head.up * (-0.8f * transform.localScale.x);

            var obj = Instantiate(TurretManager.Instance.bulletPrefab,
                pos.Flatten(), Quaternion.identity);

            obj.GetComponent<Bullet>().SetDir(angle * _head.up * (-1 * transform.localScale.x * _bulletSpeed));   
        }
    }
}