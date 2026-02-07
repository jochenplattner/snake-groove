using System;
using System.Collections.Generic;

using UnityEngine;

namespace SnakeGroove.Infrastructure
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        /// <summary>
        /// Проверка инициализации EventBus и очистка подписчиков.
        /// </summary>
        public static void Init()
        {
            _subscribers.Clear();
            Log("EventBus is alive and clear.");
        }

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Delegate>();
            }

            _subscribers[type].Add(callback);
            Log($"Subscribe<{type.Name}> (count={_subscribers[type].Count})");
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;

            _subscribers[type].Remove(callback);
            Log($"Unsubscribe<{type.Name}> (count={_subscribers[type].Count})");
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);

            if (!_subscribers.ContainsKey(type))
            {
                Debug.Log($"Publish<{type.Name}> (no listeners)");
                return;
            }

            Log($"Publish<{type.Name}> (listeners={_subscribers[type].Count})");

            foreach (var subscriber in _subscribers[type])
            {
                ((Action<T>)subscriber).Invoke(eventData);
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void Log(string msg)
        {
            Debug.Log($"[EventBus] {msg}");
        }
    }
}
