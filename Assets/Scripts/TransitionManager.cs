using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private Transform _wipePrefab;

    private float _wipeWidth;
    private bool _isTransitioning;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        _wipeWidth = _wipePrefab.localScale.x;
        _wipePrefab.position = -1 * _wipeWidth * Vector3.right;
        _wipePrefab.gameObject.SetActive(false);
    }

    public async void ChangeScene(string scene)
    {
        await PlayTransition(() => SceneManager.LoadScene(scene));
    }

    public async UniTask PlayTransition(Action method)
    {
        await UniTask.WaitUntil(() => _isTransitioning == false);

        _isTransitioning = true;
        
        var sequence = DOTween.Sequence()
            .AppendCallback(() => _wipePrefab.gameObject.SetActive(true))
            .AppendCallback(() => _wipePrefab.position = new Vector3(-1 * _wipeWidth, 0, 0))
            .Append(_wipePrefab.DOMoveX(0, 1))
            .AppendCallback(new TweenCallback(method))
            .AppendInterval(1)
            .Append(_wipePrefab.DOMoveX(_wipeWidth, 1))
            .AppendCallback(() =>
            {
                _wipePrefab.gameObject.SetActive(false);
                _isTransitioning = false;
            });

        await sequence.Play().ToUniTask();
    }
}
