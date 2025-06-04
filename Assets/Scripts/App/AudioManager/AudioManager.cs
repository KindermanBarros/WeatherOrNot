using System.Collections.Generic;
using UnityEngine;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;
using FMODUnity;
using FMOD.Studio;
// using Mono.Cecil;

namespace WeatherOrNot.App
{
    public class AudioManager : MonoBehaviour
    {
        private List<EventInstance> eventInstances;

        private EventInstance musicEventInstance;

        private EventInstance ambienceEventInstance;
        private WeatherTypes currentWeather = WeatherTypes.Clear;

        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                Debug.LogError("Found more than one Audio Manager in the scene");
            }
            instance = this;

            eventInstances = new List<EventInstance>();
            EventBus.Subscribe<ChangeWeatherEvent>(OnWeatherChange);
        }

        private void Start()
        {
            InitializeMusic(FMODEvents.instance.music);
            PlayAmbience(WeatherTypes.Clear);
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicEventReference);
            musicEventInstance.start();
        }

        private void OnWeatherChange(ChangeWeatherEvent args)
        {
            PlayAmbience(args.WeatherType);
        }

        private void PlayAmbience(WeatherTypes weather)
        {
            if (currentWeather == weather && ambienceEventInstance.isValid())
                return;

            if (ambienceEventInstance.isValid())
            {
                ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                ambienceEventInstance.release();
            }

            EventReference ambienceRef = FMODEvents.instance.clearAmbience;
            switch (weather)
            {
                case WeatherTypes.Rain:
                    ambienceRef = FMODEvents.instance.rainAmbience;
                    break;
                case WeatherTypes.Snow:
                    ambienceRef = FMODEvents.instance.snowAmbience;
                    break;
                case WeatherTypes.Thunderstorm:
                    ambienceRef = FMODEvents.instance.thunderstormAmbience;
                    break;
                case WeatherTypes.Windy:
                    ambienceRef = FMODEvents.instance.windyAmbience;
                    break;
                case WeatherTypes.Clear:
                default:
                    ambienceRef = FMODEvents.instance.clearAmbience;
                    break;
            }

            ambienceEventInstance = CreateInstance(ambienceRef);
            ambienceEventInstance.start();
            currentWeather = weather;
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(sound, worldPosition);
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(eventInstance);
            return eventInstance;
        }

        private void CleanUp()
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
            }
        }

        private void OnDestroy()
        {
            CleanUp();      
        }


        /*
            [Header("--------- Audio Source ---------")]
            [SerializeField] AudioSource SFXSource;
            [SerializeField] AudioSource musicSource;

            [Header("---------- Audio Clip ----------")]
            public AudioClip bgMusic;
            public AudioClip gumDeath;
            public AudioClip bubbleDeath;
            public AudioClip gumJump;
            public AudioClip bubbleJump;
            public AudioClip MenuSelection;

            private void Start()
            {
                musicSource.clip = bgMusic;
                musicSource.Play();
            }

            public void PlaySFX(AudioClip clip)
            {
                SFXSource.PlayOneShot(clip);
            }*/
    }
}
