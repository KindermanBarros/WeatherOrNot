using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace WeatherOrNot.App
{
    public class MenuController : MonoBehaviour
    {
        [FormerlySerializedAs("settingsPanel")] [Header("Menu Controller")] [SerializeField]
        private GameObject m_settingsPanel;

        [FormerlySerializedAs("creditsPanel")] [SerializeField]
        private GameObject m_creditsPanel;

        [FormerlySerializedAs("menuPanel")] [SerializeField]
        private GameObject m_menuPanel;

        [FormerlySerializedAs("menuParticles")] [SerializeField]
        private ParticleSystem m_menuParticles;

        [FormerlySerializedAs("menuParticles1")] [SerializeField]
        private ParticleSystem m_menuParticles1;

        public void GoToGame()
        {
            SceneManager.LoadScene(1);
        }

        public void OpenSettings()
        {
            m_settingsPanel.SetActive(true);
            m_menuPanel.SetActive(false);
        }

        public void OpenCredits()
        {
            m_creditsPanel.SetActive(true);
            m_menuPanel.SetActive(false);
        }

        public void CloseAllPanels()
        {
            m_settingsPanel.SetActive(false);
            m_creditsPanel.SetActive(false);
            m_menuPanel.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}