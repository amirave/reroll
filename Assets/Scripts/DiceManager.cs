
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set; }
    
    [SerializeField] public GameObject dicePrefab;
    [SerializeField] public GameObject displayPrefab;
    [SerializeField] public DiceLandParameters diceLandParameters;
    
    [HideInInspector] public List<Dice> dice;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        dice = FindObjectsOfType<Dice>().ToList();
    }

    public void SummonDice(Vector3 pos, Vector3 dir)
    {
        var obj = Instantiate(dicePrefab, pos, Random.rotation);
        var d = obj.GetComponent<Dice>();
        d.CastDice(dir, DiceActivate);
        dice.Add(d);
    }

    public void DiceActivate(int face, Dice d)
    {
        TurretManager.Instance.SummonTurret(d.transform.position, face);
        dice.Remove(d);
        Destroy(d.gameObject); 
        // TODO play effect
    }
}
