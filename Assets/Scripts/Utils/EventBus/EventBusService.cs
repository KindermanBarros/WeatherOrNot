using System;
using System.Collections.Generic;

namespace WeatherOrNot.Utils
{
    public class EventBusService
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();

            _subscribers[type].Add(callback);
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;

            _subscribers[type].Remove(callback);
            if (_subscribers[type].Count == 0)
                _subscribers.Remove(type);
        }

        public void Notify<T>(object sender, T evt)
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var callbacks)) return;

            foreach (var callback in callbacks)
                ((Action<T>)callback).Invoke(evt);
        }
    }
}