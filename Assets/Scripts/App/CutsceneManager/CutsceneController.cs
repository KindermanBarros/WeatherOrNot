using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

namespace WeatherOrNot.App
{
    public class CutsceneController : MonoBehaviour
    {
        private VideoPlayer videoPlayer;

        public string nextSceneName;

        void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        void OnVideoFinished(VideoPlayer vp)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
