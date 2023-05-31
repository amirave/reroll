using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioMusicTrack : MonoBehaviour
{
    public string id => _clipId;
    
    private AudioSource _audioSource;
    private float _volumeChangeRate;

    private bool _isFadeIn;
    private string _clipId;
    
    public void Initialize(AudioMixerGroup group)
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.spatialize = false;
        _audioSource.loop = true;
        _audioSource.volume = 0.0f;
        _audioSource.outputAudioMixerGroup = group;

        _volumeChangeRate = 1.0f / MagicNumbers.MUSIC_CROSSFADE_DURATION;
        _clipId = _audioSource.clip.name;
        
        _audioSource.Play();
        enabled = false;
    }

    public void FadeIn()
    {
        _isFadeIn = true;
        enabled = true;
    }

    public void FadeOut()
    {
        _isFadeIn = false;
        enabled = true;
    }

    void Update()
    {
        var isDone = false;
        
        if (_isFadeIn)
        {
            var volume = _audioSource.volume + _volumeChangeRate * Time.unscaledDeltaTime;
            if (volume >= 1.0f)
            {
                isDone = true;
                volume = 1.0f;
            }

            _audioSource.volume = volume;
        }
        else
        {
            var volume = _audioSource.volume - _volumeChangeRate * Time.unscaledDeltaTime;
            
            if (volume <= 0.0f)
            {
                isDone = true;
                volume = 0.0f;
            }

            _audioSource.volume = volume;
        }

        if (isDone == true)
            enabled = false;
    }
}