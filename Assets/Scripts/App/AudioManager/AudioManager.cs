using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [Header("Audio Groups")] public AudioMixerGroup musicGroup;
    public AudioMixerGroup ambienceGroup;
    public AudioMixerGroup footstepsGroup;
    public AudioMixerGroup eventsGroup;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySfx(AudioClip clip, AudioMixerGroup group, float volume = 1f)
    {
        var sfxObj = new GameObject("SFX_" + clip.name);
        var source = sfxObj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = group;
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(sfxObj, clip.length + 0.1f);
    }
}