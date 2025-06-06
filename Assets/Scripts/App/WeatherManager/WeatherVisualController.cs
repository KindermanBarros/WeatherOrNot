using UnityEngine;
using UnityEngine.Tilemaps;
using WeatherOrNot.Events.Weather;
using WeatherOrNot.Utils;

namespace WeatherOrNot.App.WeatherManager
{
    public class WeatherVisualController : MonoBehaviour
    {
        [SerializeField] private Tilemap m_tilemap;
        [SerializeField] private Tilemap m_ceilingTilemap;

        [SerializeField] private ParticleSystem m_rain;

        [Header("Danger Tilemap")] [SerializeField]
        private TilemapRenderer m_tilemapDangersRenderer;

        [Header("Colors by Weather")] [SerializeField]
        private Color m_clearColor = Color.white;

        [SerializeField] private Color m_rainColor = new Color(0.4f, 0.6f, 0.9f);
        [SerializeField] private Color m_snowColor = new Color(0.8f, 0.9f, 1f);
        [SerializeField] private Color m_thunderColor = new Color(0.2f, 0.2f, 0.3f);
        [SerializeField] private Color m_windyColor = new Color(0.7f, 0.85f, 0.9f);

        private void Awake()
        {
            EventBus.Subscribe<UpdateWeatherEvent>(OnWeatherUpdated);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<UpdateWeatherEvent>(OnWeatherUpdated);
        }

        private void OnWeatherUpdated(UpdateWeatherEvent e)
        {
            SetWeatherVisual(e.WeatherType);
        }

        private void SetWeatherVisual(WeatherTypes weather)
        {
            if (m_tilemap == null)
            {
                return;
            }

            switch (weather)
            {
                case WeatherTypes.Clear:
                    m_tilemap.color = m_clearColor;
                    m_ceilingTilemap.color = m_clearColor;
                    m_tilemapDangersRenderer.gameObject.SetActive(true);
                    m_rain.gameObject.SetActive(false);
                    break;
                case WeatherTypes.Rain:
                    m_tilemapDangersRenderer.gameObject.SetActive(true);
                    m_tilemap.color = m_rainColor;
                    m_ceilingTilemap.color = m_rainColor;
                    m_rain.gameObject.SetActive(true);
                    break;
                case WeatherTypes.Snow:
                    m_tilemap.color = m_snowColor;
                    m_ceilingTilemap.color = m_snowColor;
                    m_tilemapDangersRenderer.gameObject.SetActive(false);
                    m_rain.gameObject.SetActive(false);
                    break;
                case WeatherTypes.Thunderstorm:
                    m_tilemapDangersRenderer.gameObject.SetActive(true);
                    m_rain.gameObject.SetActive(false);
                    m_tilemap.color = m_thunderColor;
                    m_ceilingTilemap.color = m_thunderColor;
                    break;
                case WeatherTypes.Windy:
                    m_tilemapDangersRenderer.gameObject.SetActive(true);
                    m_rain.gameObject.SetActive(false);
                    m_tilemap.color = m_windyColor;
                    m_ceilingTilemap.color = m_windyColor;
                    break;
                default:
                    break;
            }
        }
    }
}