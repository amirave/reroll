using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tymski;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LifetimeScope : MonoBehaviour
{
    public static LifetimeScope Instance { get; private set; }
    
    [SerializeField] private SceneReference _managersScene;
    [SerializeField] private SceneReference _mainMenuScene; 
    [SerializeField] private SceneReference _gameScene;

    [SerializeField] private GameObject _canvas;
    
    public PlayerState playerState;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerState = new PlayerState
        {
            musicVolume = 0.5f,
            sfxVolume = 0.5f
        };

        DontDestroyOnLoad(gameObject);
        
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(MagicNumbers.MANAGERS_SCENE_NAME))
            SceneManager.LoadScene(_mainMenuScene);
    }

    public void ToMainMenu()
    {
        TransitionManager.Instance.ChangeScene(_mainMenuScene);
    }
}
