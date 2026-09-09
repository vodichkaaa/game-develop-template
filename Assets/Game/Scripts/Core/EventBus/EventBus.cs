using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core.EventBus
{
    public class EventBus : IEventBus
    {
        private const string COLOR_SUBSCRIBE = "#00FF00";
        private const string COLOR_UNSUBSCRIBE = "#FFA500";
        private const string COLOR_PUBLISH = "#00BFFF";
        private const string COLOR_REMOVE = "#00000";
        private readonly Dictionary<Type, List<Delegate>> _eventSubscribers = new Dictionary<Type, List<Delegate>>();
        private readonly object _lock = new object();

        public void Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent
        {
            lock (_lock)
            {
                var eventType = typeof(TEvent);

                if (!_eventSubscribers.ContainsKey(eventType))
                {
                    _eventSubscribers[eventType] = new List<Delegate>();
                }

                if (!_eventSubscribers[eventType].Contains(callback))
                {
                    _eventSubscribers[eventType].Add(callback);

                    Debug.Log($"<color={COLOR_SUBSCRIBE}>[EventBus] ✓ Subscribed to {eventType.Name}. Total subscribers: {_eventSubscribers[eventType].Count}</color>");
                }
                else
                {

                    Debug.LogWarning($"[EventBus] ⚠ Already subscribed to {eventType.Name}</color>");
                }
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent
        {
            lock (_lock)
            {
                var eventType = typeof(TEvent);

                if (_eventSubscribers.ContainsKey(eventType))
                {
                    _eventSubscribers[eventType].Remove(callback);

                    Debug.Log($"<color={COLOR_UNSUBSCRIBE}>[EventBus] ✗ Unsubscribed from {eventType.Name}. Remaining subscribers: {_eventSubscribers[eventType].Count}</color>");

                    if (_eventSubscribers[eventType].Count == 0)
                    {
                        _eventSubscribers.Remove(eventType);
                        Debug.Log($"<color={COLOR_REMOVE}>[EventBus] ➤ No more subscribers for {eventType.Name}, removed from registry</color>");
                    }
                }
            }
        }

        public void Publish<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);

            lock (_lock)
            {
                if (_eventSubscribers.ContainsKey(eventType))
                {
                    var subscribers = new List<Delegate>(_eventSubscribers[eventType]);

                    Debug.Log($"<color={COLOR_PUBLISH}>[EventBus] ➤ Publishing {eventType.Name} to {subscribers.Count} subscribers</color>");

                    foreach (var subscriber in subscribers)
                    {
                        try
                        {
                            (subscriber as Action<TEvent>)?.Invoke(eventData);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"[EventBus] ✖ Error invoking subscriber for {eventType.Name}: {ex.Message}\n{ex.StackTrace}</color>");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"[EventBus] ⚠ No subscribers for {eventType.Name}</color>");
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                Debug.Log($"<color={COLOR_UNSUBSCRIBE}>[EventBus] ✗ Clearing all subscriptions. Total event types: {_eventSubscribers.Count}</color>");
                _eventSubscribers.Clear();
            }
        }

        public int GetSubscriberCount<TEvent>() where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            return _eventSubscribers.ContainsKey(eventType) ? _eventSubscribers[eventType].Count : 0;
        }
    }
}