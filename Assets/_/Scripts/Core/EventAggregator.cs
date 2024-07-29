using System;
using System.Collections.Generic;


public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

    public void Subscribe<TEvent>(Action<TEvent> subscriber) where TEvent : EventArgs
    {
        if (!_subscribers.ContainsKey(typeof(TEvent)))
        {
            _subscribers[typeof(TEvent)] = new List<object>();
        }
        _subscribers[typeof(TEvent)].Add(subscriber);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> subscriber) where TEvent : EventArgs
    {
        if (_subscribers.ContainsKey(typeof(TEvent)))
        {
            _subscribers[typeof(TEvent)].Remove(subscriber);
        }
    }

    public void Publish<TEvent>(TEvent eventToPublish) where TEvent : EventArgs
    {
        if (_subscribers.ContainsKey(eventToPublish.GetType()))
        {
            foreach (var subscriber in _subscribers[eventToPublish.GetType()])
            {
                ((Action<TEvent>)subscriber).Invoke(eventToPublish);
            }
        }
    }
}