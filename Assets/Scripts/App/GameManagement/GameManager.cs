using UnityEngine;

namespace WeatherOrNot.App.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance { get; set; }

        protected void Awake()
        {
            if (m_instance)
            {
                Destroy(gameObject);
                return;
            }

            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}