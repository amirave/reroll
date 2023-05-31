
using Unity.Mathematics;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }

    [SerializeField] private GameObject[] turretPrefabs;
    [SerializeField] public GameObject bulletPrefab;

    [SerializeField] private float _knockbackMult = 3f;

    private TurretFactory _turretFactory;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        _turretFactory = new TurretFactory();
    }

    public void SummonTurret(Vector3 pos, int level)
    {
        
        Instantiate(turretPrefabs[level - 1], pos.Flatten(), Quaternion.Euler(-270, 0, 180));
        var hits = Physics.OverlapSphere(pos, 0.7f, LayerMask.GetMask("Dice", "Bullet", "Player"));

        foreach (var hit in hits)
        {
            var dir = hit.transform.position - pos;
            if (dir.magnitude < 0.01f)
                continue;
            
            var dist = dir.magnitude;
                
            var dice = hit.GetComponent<Dice>();
                
            if (dice != null) 
                dice.CastDice((1 / dist) * _knockbackMult * dir.normalized,
                    DiceManager.Instance.DiceActivate);
            else
                hit.GetComponent<Rigidbody>().AddForce((1/dist) * _knockbackMult * dir.normalized, ForceMode.Impulse);
        }
        
        AudioManager.Instance.PlayEffectRandom("turret_spawn");
    }
}
