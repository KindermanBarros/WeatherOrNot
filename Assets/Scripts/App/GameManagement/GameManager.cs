using WeatherOrNot.Utils;

namespace WeatherOrNot.App.GameManagement
{
    public class GameManager : BaseUIAnimatedUIView
    {
        private static GameManager m_instance { get; set; }

        public EventBusService EventBus { get; private set; }

        protected override void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            m_instance = this;
            DontDestroyOnLoad(gameObject);

            EventBus = new EventBusService();
        }
    }
}