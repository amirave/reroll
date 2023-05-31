using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScope : MonoBehaviour
{
    public static MainMenuScope Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        if (LifetimeScope.Instance == null)
            SceneManager.LoadScene(MagicNumbers.MANAGERS_SCENE_NAME, LoadSceneMode.Additive);
    }
}
