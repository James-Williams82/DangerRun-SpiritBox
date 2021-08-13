using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Com.FastEffect.DataTypes;


public abstract class AudioManagerBase : MonoBehaviour
{
    public BoolValue muted;

    [SerializeField]
    private string mixerName = "Main";

    public FloatValue currentClipDuration;

    public List<CustomAudioContent> audioContainer = new List<CustomAudioContent>();

    private List<AudioSource> AudioSourcesPlaying = new List<AudioSource>();

    int location = 0;

    bool IsAudioContainerValid => !audioContainer[location];

    AudioMixer cachedMixer;

    float mixerVolume = 0;

    public void PlayAudioClip(string clipIndex)
    {
            string[] convertedClipIndex = clipIndex.Split(',');

            string firstIndexParam = convertedClipIndex[0];
            location = int.Parse(firstIndexParam);

            string secondIndexParam = convertedClipIndex[1];
            int clip = int.Parse(secondIndexParam);

            string thirdIndexParam = convertedClipIndex[2];
            bool setLoop = bool.Parse(thirdIndexParam);

            if (IsAudioContainerValid)
            {
                Debug.LogErrorFormat("Missing Audio Clip @ audioContainer " + " [ " + location + " ] ");

                return;
            }
            else
            {
                AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();

                audioSource.outputAudioMixerGroup = audioContainer[location].mixerGroup;

                audioSource.loop = setLoop;

                audioSource.clip = audioContainer[location].AudioClips[clip];
                
                if (!setLoop)
                {
                    currentClipDuration.Value = audioSource.clip.length;

                    AudioSourcesPlaying.Add(audioSource);
                    
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else
                {
                    audioSource.Play();
                }

                Sequence cleanupSequence = DOTween.Sequence();

                cleanupSequence.PrependInterval(audioSource.clip.length);

                cleanupSequence.OnComplete(() => RemoveUnusedAudioSources(audioSource, cleanupSequence));
            }
    }


    public void RemoveUnusedAudioSources(AudioSource audioSourceToRemove, Sequence sequenceToRemove)
    {
        if (audioSourceToRemove.loop)
        {
            return;
        }
        else
        {
            AudioSourcesPlaying.Remove(audioSourceToRemove);

            Destroy(audioSourceToRemove);

            sequenceToRemove.Kill();
        }
    }

    public void StopAudio()
    {
        foreach (AudioSource audioSource in AudioSourcesPlaying)
        {
            audioSource.Stop();
        }
    }


    public void MuteAudio(bool muteAudio, int audioSourceToMute = 0)
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            source.mute = muteAudio;
        }
    }

    public void FadeAudio(float targetVolume)
    {
        if (!muted.Value)
        {
            Debug.LogWarning("Mixer muted");

            return;
        }
        else
        {
            AudioMixer mixer = Resources.Load(mixerName) as AudioMixer;

            cachedMixer = mixer;

            if (!mixer)
            {
                Debug.LogWarning("Mixer not Found");

                return;
            }
            else
            {
                Debug.Log("Mixer Found");

                StartCoroutine(FadeMixerGroup.StartFade(mixer, "BGM Adjust Volume", 1.75f, targetVolume));
            }
        }
    }

    public void SetMixerVolume()
    {
        cachedMixer.SetFloat("VO", -6);
    }

    public void VolumeUp()
    {
        cachedMixer.SetFloat("VO", ButtonVolumeVolume() + 3.1f);

        Debug.Log(ButtonVolumeVolume());
    }

    public void VolumeDown()
    {
        cachedMixer.SetFloat("VO", ButtonVolumeVolume() - 3.1f);
    }


    public float ButtonVolumeVolume()
    {
        float value;

        bool result = cachedMixer.GetFloat("VO", out value);

        if (result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }

    
}
