using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace WeatherOrNot.App
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer myMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("musicVolume")) LoadVolume();
            else
            {
                SetSFXVolume();
                SetMusicVolume();
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        public void SetSFXVolume()
        {
            float sfx = musicSlider.value;
            myMixer.SetFloat("sfx", Mathf.Log10(sfx) * 20);
            PlayerPrefs.SetFloat("sfxVolume", sfx);
        }


        private void LoadVolume()
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

}
