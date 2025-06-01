using System;
using System.Collections.Generic;

namespace WeatherOrNot.Utils
{
    public class EventBusService
    {
        private readonly Dictionary<Type, List<Delegate>> m_subscribers = new();

        public void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!m_subscribers.ContainsKey(type))
                m_subscribers[type] = new List<Delegate>();

            m_subscribers[type].Add(callback);
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!m_subscribers.ContainsKey(type)) return;

            m_subscribers[type].Remove(callback);
            if (m_subscribers[type].Count == 0)
                m_subscribers.Remove(type);
        }

        public void Notify<T>(object sender, T evt)
        {
            var type = typeof(T);
            if (!m_subscribers.TryGetValue(type, out var callbacks)) return;

            foreach (var callback in callbacks)
                ((Action<T>)callback).Invoke(evt);
        }
    }
}