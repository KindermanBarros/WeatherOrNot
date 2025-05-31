using Events.MainMenu;
using WeatherOrNot.Utils;

namespace WeatherOrNot.UI.MainMenu
{
    public class MainMenuModel
    {
        public void StartGame()
        {
            EventBus.Notify(this, new PlayGameClickedEvent());
        }
    }
}