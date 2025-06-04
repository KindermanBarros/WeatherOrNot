using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [Header("Audio Groups")] [SerializeField]
    public AudioMixerGroup MainAudioMixer;

    [SerializeField] public AudioMixerGroup MusicGroup;
    [SerializeField] public AudioMixerGroup AmbienceGroup;
    [SerializeField] public AudioMixerGroup FootstepsGroup;
    [SerializeField] public AudioMixerGroup EventsGroup;

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