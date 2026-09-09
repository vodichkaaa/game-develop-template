using System;
using Game.Scripts.Services;

namespace Game.Scripts.Core.EventBus
{
    public interface IEventBus : IGlobalService
    {
        void Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent;
        void Unsubscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent;
        void Publish<TEvent>(TEvent eventData) where TEvent : IEvent;
        void Clear();
    }
}