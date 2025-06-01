using System;

namespace WeatherOrNot.Utils
{
    public static class EventBus
    {
        private static readonly EventBusService m_service = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            m_service.Subscribe(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            m_service.Unsubscribe(callback);
        }

        public static void Notify<T>(object sender, T evt)
        {
            m_service.Notify(sender, evt);
        }
    }
}