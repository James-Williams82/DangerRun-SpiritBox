using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CustomAudioManager : AudioManagerBase
{
   #region OLD
    /*
 
    public AudioMixer _audioMixer;

    private static CustomAudioManager _instance;

    public static CustomAudioManager Instance { get { return _instance; } }

    public List<CustomAudioContent> AudioContentObject = new List<CustomAudioContent>();

    private List<AudioSource> AudioSourceList = new List<AudioSource>();

    private AudioSource _aSource;

    public void AudioManagerInit()
    {
        Debug.LogError("Init");

        foreach (CustomAudioContent item in AudioContentObject)
        {
            _aSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            _aSource.playOnAwake = false;

            _aSource.outputAudioMixerGroup = item.mixerGroup;

            AudioSourceList.Add(_aSource);
        }
    }

    public void PlayBGMString(string i)
    {


        string[] splittedParams = i.Split(',');

        //get the first param
        string FirstParam = splittedParams[0];

        //Convert it back to int
        int firstParam = int.Parse(FirstParam);

        //get the first param
        string SecondParam = splittedParams[1];

        //Convert it back to int
        int secondParam = int.Parse(SecondParam);

        PlayBGM(firstParam, secondParam);
    }

    public void PlaySFXString(string i)
    {
        string[] splittedParams = i.Split(',');

        //get the first param
        string FirstParam = splittedParams[0];

        //Convert it back to int
        int firstParam = int.Parse(FirstParam);

        //get the first param
        string SecondParam = splittedParams[1];

        //Convert it back to int
        int secondParam = int.Parse(SecondParam);

        Play(firstParam, secondParam);
    }


    public void Play(int AudioObjectIndex, int AudioIndexClip)
    {
        if (AudioContentObject.Count != 0)
        {
            if (!AudioSourceList[AudioObjectIndex].isPlaying)
            {
                AudioSourceList[AudioObjectIndex].clip = AudioContentObject[AudioObjectIndex].AudioClips[AudioIndexClip];
                AudioSourceList[AudioObjectIndex].PlayOneShot(AudioContentObject[AudioObjectIndex].AudioClips[AudioIndexClip]);
            }
        }
    }

    public void PlayBGM(int AudioObjectIndex, int AudioIndexClip, bool loop = true)
    {
        if (AudioContentObject.Count != 0)
        {
            Debug.LogErrorFormat("Playing");

            if (!AudioSourceList[AudioObjectIndex].isPlaying)
            {


                AudioSourceList[AudioObjectIndex].volume = 0.5f;
                AudioSourceList[AudioObjectIndex].loop = true;
                AudioSourceList[AudioObjectIndex].clip = AudioContentObject[AudioObjectIndex].AudioClips[AudioIndexClip];
                AudioSourceList[AudioObjectIndex].Play();
            }
        }
    }

    public void PlayAmbience(int AudioObjectIndex, int AudioIndexClip, bool loop)
    {
        if (AudioContentObject.Count != 0)
        {
            if (!AudioSourceList[AudioObjectIndex].isPlaying)
            {
                AudioSourceList[AudioObjectIndex].volume = 0.5f;
                AudioSourceList[AudioObjectIndex].loop = loop;
                AudioSourceList[AudioObjectIndex].clip = AudioContentObject[AudioObjectIndex].AudioClips[AudioIndexClip];
                AudioSourceList[AudioObjectIndex].Play();
            }
        }
    }

    public void StopAudio(int AudioObjectIndex)
    {
        if (AudioContentObject.Count != 0)
        {
            if (AudioSourceList[AudioObjectIndex].isPlaying)
            {

                AudioSourceList[AudioObjectIndex].Stop();
            }
        }
    }
    
    */
#endregion


}

