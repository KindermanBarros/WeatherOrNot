using UnityEngine;
using UnityEngine.SceneManagement;

namespace WeatherOrNot.App
{
    public class MenuController : MonoBehaviour
    {
        [Header("Menu Controller")] [SerializeField]
        private GameObject settingsPanel;

        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private ParticleSystem menuParticles;
        [SerializeField] private ParticleSystem menuParticles1;
        public void GoToGame()
        {
            Debug.Log("Ir para o jogo");
            SceneManager.LoadScene(1);
        }

        public void OpenSettings()
        {
            settingsPanel.SetActive(true);
            menuPanel.SetActive(false);
        }

        public void OpenCredits()
        {
            creditsPanel.SetActive(true);
            menuPanel.SetActive(false);
        }

        public void CloseAllPanels()
        {
            settingsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            menuPanel.SetActive(true);
        }

        public void QuitGame()
        {
            Debug.Log("Sair do Jogo");
            Application.Quit();
        }
    }
}
