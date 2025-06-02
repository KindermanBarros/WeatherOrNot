using UnityEngine;
using UnityEngine.Tilemaps;
using WeatherOrNot.Utils;
using WeatherOrNot.Events.Weather;


namespace WeatherOrNot.App.WeatherManager
{
    public class WeatherVisualController : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        [Header("Tilemap dos perigos")] [SerializeField]
        private TilemapRenderer tilemapDangersRenderer;

        [Header("Cores por clima")] [SerializeField]
        private Color clearColor = Color.white;

        [SerializeField] private Color rainColor = new Color(0.4f, 0.6f, 0.9f);
        [SerializeField] private Color snowColor = new Color(0.8f, 0.9f, 1f);
        [SerializeField] private Color thunderColor = new Color(0.2f, 0.2f, 0.3f);
        [SerializeField] private Color windyColor = new Color(0.7f, 0.85f, 0.9f);

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

        public void SetWeatherVisual(WeatherTypes weather)
        {
            if (tilemap == null)
            {
                Debug.LogError("Tilemap não atribuído.");
                return;
            }

            switch (weather)
            {
                case WeatherTypes.Clear:
                    tilemap.color = clearColor;
                    tilemapDangersRenderer.gameObject.SetActive(true);
                    break;
                case WeatherTypes.Rain:
                    tilemapDangersRenderer.gameObject.SetActive(true);
                    tilemap.color = rainColor;
                    break;
                case WeatherTypes.Snow:
                    tilemap.color = snowColor;
                    tilemapDangersRenderer.gameObject.SetActive(false);
                    break;
                case WeatherTypes.Thunderstorm:
                    tilemapDangersRenderer.gameObject.SetActive(true);
                    tilemap.color = thunderColor;
                    break;
                case WeatherTypes.Windy:
                    tilemapDangersRenderer.gameObject.SetActive(true);
                    tilemap.color = windyColor;
                    break;
            }
        }
    }
}