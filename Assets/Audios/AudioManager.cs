using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public AudioMixer mixerPrincipal;

    [Header("Audio Groups")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup ambienceGroup;
    public AudioMixerGroup footstepsGroup;
    public AudioMixerGroup eventsGroup;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    public void PlaySFX(AudioClip clip, AudioMixerGroup group, float volume = 1f)
    {
        GameObject sfxObj = new GameObject("SFX_" + clip.name);
        AudioSource source = sfxObj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = group;
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(sfxObj, clip.length + 0.1f);
    }
}