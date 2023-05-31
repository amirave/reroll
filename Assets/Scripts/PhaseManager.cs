using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }

    [SerializeField] private float _spawnHeight = 3;
    [SerializeField] private float _minStrength = 1;
    [SerializeField] private float _maxStrength = 1;
    
    private float _timeBetweenPhases = 8f;
    private int _obstaclePerPhase = 3;
    private float _timeBetweenObstacles = 1f;

    private float _lastPhaseEnd = 0;
    private bool _midPhase = false;
    private float _obstacleHeight;
    
    private GameScope gs => GameScope.Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _lastPhaseEnd = Time.time;
    }

    private void Update()
    {
        _obstaclePerPhase = (int) gs.difficultySettings.obstaclesPerPhase.GetCurrent(gs.GetTime());
        _timeBetweenObstacles = gs.difficultySettings.timeBetweenObstacles.GetCurrent(gs.GetTime());
        _timeBetweenPhases = gs.difficultySettings.timeBetweenPhases.GetCurrent(gs.GetTime());

        if (!_midPhase && Time.time - _lastPhaseEnd >= _timeBetweenPhases)
        {
            StartPhase();
        }
    }

    private async void StartPhase()
    {
        _midPhase = true;
        
        for (var i = 0; i < _obstaclePerPhase; i++)
        {
            var pos = Utils.RandomOnPerimeter(gs.playAreaSize * 0.9f).ToVector3(-1 * _spawnHeight);
            var dir = Random.Range(_minStrength, _maxStrength) * (gs.player.transform.position - pos).normalized;

            DiceManager.Instance.SummonDice(pos, dir);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_timeBetweenObstacles));
        }

        _midPhase = false;
        _lastPhaseEnd = Time.time;
    }
}
