using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public static void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);

        if (!_subscribers.ContainsKey(type))
        {
            _subscribers[type] = new List<Delegate>();
        }

        _subscribers[type].Add(callback);
    }

    public static void Publish<T>(T eventData)
    {
        var type = typeof(T);

        if (!_subscribers.ContainsKey(type))
        {
            return;
        }

        foreach (var subscriber in _subscribers[type])
        {
            ((Action<T>)subscriber).Invoke(eventData);
        }
    }
}
