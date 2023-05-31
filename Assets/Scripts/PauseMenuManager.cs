using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    private GameObject _menu;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        _menu = transform.GetChild(0).gameObject;

        _menu.SetActive(false);
    }

    public void PauseGame()
    {
        // anim
        _menu.SetActive(true);
        transform.SetAsLastSibling();
        AudioManager.Instance.PlayEffectRandom("sfx_menu_select");
        UpdateVolumes();
    }

    public void UnpauseGame()
    {
        _menu.SetActive(false);
        GameScope.Instance.UnpauseGame();
        AudioManager.Instance.PlayEffectRandom("sfx_menu_select");
    }

    public void SetVolume(bool isSfx)
    {
        var playerState = LifetimeScope.Instance.playerState;
        
        if (isSfx)
        {
            playerState.sfxVolume = Utils.Round(playerState.sfxVolume + 0.1f, 1);
            if (playerState.sfxVolume > 1) playerState.sfxVolume = 0;
            AudioManager.Instance.SetChannelActive(AudioChannel.Fx);
        }  
        else
        {
            playerState.musicVolume = Utils.Round(playerState.musicVolume + 0.1f, 1);
            if (playerState.musicVolume > 1) playerState.musicVolume = 0;
            AudioManager.Instance.SetChannelActive(AudioChannel.Music);
        }
        
        AudioManager.Instance.PlayEffectRandom("sfx_menu_select");
        
        UpdateVolumes();
    }

    public void Exit()
    {
        LifetimeScope.Instance.ToMainMenu();
    }

    public void OnHighlighted()
    {
        AudioManager.Instance.PlayEffectRandom("sfx_menu_move");
    }

    private void UpdateVolumes()
    {
        var playerState = LifetimeScope.Instance.playerState;

        _menu.transform.Find("sfx").GetChild(0).GetComponent<TMP_Text>().text = $"SFX volume : {playerState.sfxVolume * 100}%";
        _menu.transform.Find("music").GetChild(0).GetComponent<TMP_Text>().text = $"Music volume : {playerState.musicVolume * 100}%";
    }

    void Update()
    {
        
    }
}
