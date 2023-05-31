
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _kickDist;
    [SerializeField] private float _kickForce = 3;
    [SerializeField] private float _lineLength = 0.5f;

    [SerializeField] private Image healthUI;
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private GameObject _linePrefab;
    
    private LineRenderer[] _lines;

    [HideInInspector] private int health = 3;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _lines = new LineRenderer[4];
        
        for (int i = 0; i < 4; i++)
            _lines[i] = Instantiate(_linePrefab, transform).GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (GameScope.Instance.isPaused)
            return;

        var hits = Physics.OverlapSphere(transform.position, _kickDist, LayerMask.GetMask("Dice"));

        for (int i = 0; i < 4; i++) 
        {
            if (i >= hits.Length)
            {
                _lines[i].positionCount = 0;
                continue;
            }
            
            var hit = hits[i].transform;
            var start = hit.position;
            var end = start + (Utils.ScreenToWorldPos(Input.mousePosition) - transform.position).normalized *
                _lineLength;

            var points = new Vector3[] { start, end };

            _lines[i].positionCount = 2;
            _lines[i].SetPositions(points);
            print(_lines[i].GetPosition(0) - start);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var looking = Utils.ScreenToWorldPos(Input.mousePosition) - transform.position;
            foreach (var hit in hits)
            {
                hit.transform.GetComponent<Dice>()?.CastDice(looking.normalized * _kickForce, null, true);
                AudioManager.Instance.PlayEffectRandom("kick");
                ShakeManager.Instance.AddShake(0.3f);
            }
            
            var bulletHits = Physics.OverlapSphere(transform.position, _kickDist * 2.5f, LayerMask.GetMask("Bullet"));
            
            if (hits.Length > 0)
                foreach (var bhit in bulletHits)
                    Destroy(bhit.gameObject, 0.01f);
        }
    }

    void FixedUpdate()
    {
        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        _rb.MovePosition(transform.position + _speed * Time.deltaTime * dir);
    }

    private Dice GetClosestDice()
    {
        Dice dice = null;
        var minSqrDist = _kickDist * _kickDist;
        
        foreach (var d in DiceManager.Instance.dice)
        {
            var dist = (d.transform.position - transform.position).sqrMagnitude;
            if (dist < minSqrDist)
            {
                dice = d;
                minSqrDist = dist;
            }
        }

        return dice;
    }

    public void TakeDamage()
    {
        health--;

        // TODO death screen
        if (health <= 0)
            LifetimeScope.Instance.ToMainMenu();
        else
            healthUI.sprite = healthSprites[health-1];
        
        AudioManager.Instance.PlayEffect("player_hit");
        ShakeManager.Instance.AddShake(0.6f);
    }
}
