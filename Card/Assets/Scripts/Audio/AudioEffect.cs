using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioEffect : AudioBase 
{
 private void Awake()
    {
        Bind(AudioEvent.PLAY_EFFECT_AUDIO);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_EFFECT_AUDIO:
                PlayEffectAudio(message.ToString());
                break;
            default:
                break;
        }
    }
    private AudioSource audioSource;
   	void Start () {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void PlayEffectAudio(string audioName)
    {
       AudioClip ac = Resources.Load<AudioClip>("Sound/" + audioName);
        audioSource.clip = ac;
        audioSource.Play();
    }

}
