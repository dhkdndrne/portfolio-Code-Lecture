using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    public static AudioCtrl instance;
     AudioSource[] theAudio = new AudioSource[3];
     AudioClip[] clip = new AudioClip[3];

    [SerializeField] AudioSource[] prefabAudio = new AudioSource[2100];
    [SerializeField] AudioClip[] prefabClip;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < 3; i++)
        {
            theAudio[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
            theAudio[i].Stop();
        }

        clip[0] = Resources.Load("Sounds/mainMusic", typeof(AudioClip)) as AudioClip;
        clip[1] = Resources.Load("Sounds/normal1", typeof(AudioClip)) as AudioClip;
        clip[2] = Resources.Load("Sounds/normal2", typeof(AudioClip)) as AudioClip;

        for (int i = 0; i < 3; i++)
        {
            //clip[0] 은 배경음악
            if (i == 0)
            {
                theAudio[i].clip = clip[i];
                theAudio[i].loop = true;
                theAudio[i].Play();
            }
            else
            {
                theAudio[i].clip = clip[i];
                theAudio[i].loop = false;
                theAudio[i].playOnAwake = false;
            }

        }
    }
    public void InputAudioSource(int _idx,GameObject _obj)
    {
        prefabAudio[_idx] = _obj.GetComponent<AudioSource>();
    }

    public void EffectVolumeControl(float _volume)
    {
        theAudio[1].volume = _volume;
        theAudio[2].volume = _volume;
        for (int i =0;i<prefabAudio.Length;i++)
        {
            prefabAudio[i].volume = _volume;
        }
    }
    public void BackGroundMusicVolumeCtrl(float _volume)
    {
        theAudio[0].volume = _volume;
    }
    public void PlayAudio(int _idx)
    {
        //theAudio[_idx].Play();
    }
}
