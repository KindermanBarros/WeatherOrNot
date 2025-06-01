using UnityEngine;

namespace WeatherOrNot.App
{
    public class AudioManager : MonoBehaviour
    {
        [Header("---------  Audio Source    ---------")]
        [SerializeField] AudioSource SFXSource;
        [SerializeField] AudioSource musicSource;

        [Header("---------  Audio Clip      ---------")]
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
        }
    }
}
