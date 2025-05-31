using System;

namespace WeatherOrNot.Utils
{
    public static class EventBus
    {
        private static readonly EventBusService _service = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            _service.Subscribe(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            _service.Unsubscribe(callback);
        }

        public static void Notify<T>(object sender, T evt)
        {
            _service.Notify(sender, evt);
        }
    }
}