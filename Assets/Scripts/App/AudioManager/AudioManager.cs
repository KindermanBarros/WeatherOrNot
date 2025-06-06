using System.Collections.Generic;
using UnityEngine;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;
using FMODUnity;
using FMOD.Studio;

namespace WeatherOrNot.App
{
    public class AudioManager : MonoBehaviour
    {
        private List<EventInstance> eventInstances;
        private EventInstance musicEventInstance;
        private EventInstance ambienceEventInstance;

        private const string WEATHER_PARAMETER_NAME = "Weather";

        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Found more than one Audio Manager in the scene. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }
            instance = this;

            DontDestroyOnLoad(gameObject);

            eventInstances = new List<EventInstance>();
            EventBus.Subscribe<ChangeWeatherEvent>(OnWeatherChange);
        }

        private void Start()
        {
            InitializeMusic(FMODEvents.instance.music);
            InitializeAmbience(FMODEvents.instance.ambience);
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicEventReference);
            musicEventInstance.start();
        }

        private void InitializeAmbience(EventReference ambienceEventReference)
        {
            ambienceEventInstance = CreateInstance(ambienceEventReference);
            ambienceEventInstance.start();
            SetWeatherParameter(WeatherTypes.Clear);
        }

        private void OnWeatherChange(ChangeWeatherEvent args)
        {
            SetWeatherParameter(args.WeatherType);
        }

        private void SetWeatherParameter(WeatherTypes weather)
        {
            float weatherValue = (float)weather;
            ambienceEventInstance.setParameterByName(WEATHER_PARAMETER_NAME, weatherValue);
        }

        public void PlayConfirmButtonSound()
        {
            PlayOneShot(FMODEvents.instance.confirmButton, Vector3.zero);
        }

        public void PlayCancelButtonSound()
        {
            PlayOneShot(FMODEvents.instance.cancelButton, Vector3.zero);
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
            EventBus.Unsubscribe<ChangeWeatherEvent>(OnWeatherChange);
            CleanUp();
        }
    }
}

/*using System.Collections.Generic;
using UnityEngine;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;
using FMODUnity;
using FMOD.Studio;

namespace WeatherOrNot.App
{
    public class AudioManager : MonoBehaviour
    {
        private List<EventInstance> eventInstances;

        private EventInstance musicEventInstance;

        private WeatherTypes currentWeather = WeatherTypes.Clear;
        private Dictionary<WeatherTypes, EventInstance> ambienceInstances;

        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Found more than one Audio Manager in the scene");
                Destroy(gameObject);
                return;
            }
            instance = this;

            eventInstances = new List<EventInstance>();
            ambienceInstances = new Dictionary<WeatherTypes, EventInstance>();

            ambienceInstances[WeatherTypes.Clear] = CreateInstance(FMODEvents.instance.clearAmbience);
            ambienceInstances[WeatherTypes.Rain] = CreateInstance(FMODEvents.instance.rainAmbience);
            ambienceInstances[WeatherTypes.Snow] = CreateInstance(FMODEvents.instance.snowAmbience);
            ambienceInstances[WeatherTypes.Thunderstorm] = CreateInstance(FMODEvents.instance.thunderstormAmbience);
            ambienceInstances[WeatherTypes.Windy] = CreateInstance(FMODEvents.instance.windyAmbience);

            EventBus.Subscribe<ChangeWeatherEvent>(OnWeatherChange);
        }

        private void Start()
        {
            InitializeMusic(FMODEvents.instance.forestMusic);
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
            if (currentWeather == weather)
                return;

            if (ambienceInstances[currentWeather].isValid())
            {
                ambienceInstances[currentWeather].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            if (ambienceInstances[weather].isValid())
            {
                ambienceInstances[weather].start();
            }

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
            foreach (var eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
            }
            if (ambienceInstances != null)
            {
                foreach (var instance in ambienceInstances.Values)
                {
                    instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    instance.release();
                }
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }

    }
}*/