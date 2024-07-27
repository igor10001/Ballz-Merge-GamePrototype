using System;

public interface IEventAggregator
{
    void Subscribe<TEvent>(Action<TEvent> subscriber) where TEvent : EventArgs;
    void Unsubscribe<TEvent>(Action<TEvent> subscriber) where TEvent : EventArgs;
    void Publish<TEvent>(TEvent eventToPublish) where TEvent : EventArgs;
}