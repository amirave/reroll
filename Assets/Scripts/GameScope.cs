using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameScope : MonoBehaviour
{
    public static GameScope Instance { get; private set; }
    
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public Player player;
    
    [SerializeField] private GameObject _pauseMenuPrefab;
    [SerializeField] private GameObject _cursor;
    [SerializeField] public Vector2 playAreaSize;
    [SerializeField] public DifficultySettings difficultySettings;

    private float _startTime;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (LifetimeScope.Instance == null)
            SceneManager.LoadScene(MagicNumbers.MANAGERS_SCENE_NAME, LoadSceneMode.Additive);

        Instantiate(_pauseMenuPrefab, FindObjectOfType<Canvas>().transform);

        _startTime = Time.time;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            PauseGame();

        _cursor.transform.position = Utils.ScreenToWorldPos(Input.mousePosition);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        
        PauseMenuManager.Instance.PauseGame();
    }
    
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void SummonTurret(Vector3 pos, int level)
    {
        TurretManager.Instance.SummonTurret(pos, level);
    }

    public float GetTime()
    {
        return Time.time - _startTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(Vector3.zero, playAreaSize * 2);
    }
}