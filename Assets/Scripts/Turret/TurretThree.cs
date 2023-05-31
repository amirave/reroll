using UnityEngine;

public class TurretThree : Turret
{
    public override int Level => 3;
    
    public override void Shoot()
    {
        for (int i = -1; i <= 1; i++)
        {
            var angle = Quaternion.AngleAxis(i * 45, transform.up);
            var pos = transform.position + angle * _head.up * (-0.8f * transform.localScale.x);

            var obj = Instantiate(TurretManager.Instance.bulletPrefab,
                pos.Flatten(), Quaternion.identity);

            obj.GetComponent<Bullet>().SetDir(angle * _head.up * (-1 * transform.localScale.x * _bulletSpeed));
        }
    }
}