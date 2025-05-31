using UnityEngine;

namespace WeatherOrNot.Utils
{
    public class MainMenuButtonPresenter : AbstractButtonPresenter
    {
        [SerializeField] private MainMenuButtonType m_buttonType;

        protected override void OnReleased()
        {
            switch (m_buttonType)
            {
                case MainMenuButtonType.PlayButton:
                    HandlePlayClicked();
                    break;

                case MainMenuButtonType.ExitButton:
                    HandleExitClicked();
                    break;

                case MainMenuButtonType.SettingsButton:
                    HandleSettingsClicked();
                    break;

                case MainMenuButtonType.EasterEggButton:
                    HandleEasterEggClicked();
                    break;
            }
        }

        private void HandlePlayClicked()
        {
            Debug.Log("Play clicked! 🚀");
        }

        private void HandleExitClicked()
        {
            Application.Quit();
        }

        private void HandleSettingsClicked()
        {
            Debug.Log("Settings clicked! ⚙️");
        }

        private void HandleEasterEggClicked()
        {
            Debug.Log("Easter Egg clicked! 🥚✨");
        }
    }

    public enum MainMenuButtonType
    {
        PlayButton,
        ExitButton,
        SettingsButton,
        EasterEggButton
    }
}