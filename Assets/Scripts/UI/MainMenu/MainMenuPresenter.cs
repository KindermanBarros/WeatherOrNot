using UnityEngine;
using WeatherOrNot.Utils;

namespace WeatherOrNot.UI.MainMenu
{
    public class MainMenuPresenter : BaseUIAnimatedView
    {
        [SerializeField] private MainMenuButtonPresenter m_startGameButton;

        private MainMenuModel m_mainMenuModel;

        protected override void Awake()
        {
            base.Awake();

            if (m_startGameButton == null)
            {
                Debug.LogError("[MainMenuPresenter] m_startGameButton is not assigned!");
                return;
            }

            m_mainMenuModel = new MainMenuModel();
            m_startGameButton.OnClick += OnStartGameButtonClicked;
        }

        private void OnDestroy()
        {
            if (m_startGameButton != null)
                m_startGameButton.OnClick -= OnStartGameButtonClicked;
        }

        private void OnStartGameButtonClicked()
        {
            m_mainMenuModel.StartGame();
        }
    }
}