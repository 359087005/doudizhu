using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGAudio : AudioBase
{
    
    private void Awake()
    {
        Bind(AudioEvent.PLAY_BG_AUDIO,
            AudioEvent.STOP_BG_AUDIO,
            AudioEvent.SET_AUDIO_VOLUME);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_BG_AUDIO:
                PlayAudio((bool)message);
                break;
            case AudioEvent.SET_AUDIO_VOLUME:
                SetAudioVolume((float)message);
                break;
            default:
                break;
        }
    }

    private AudioSource audioSource;
    void Start()
    {
        audioSource = transform.Find("BGAudio").GetComponent<AudioSource>();
    }

    void PlayAudio(bool result)
    {
        audioSource.enabled = result;
    }

    void SetAudioVolume(float value)
    {
        audioSource.volume = value;
    }
}
