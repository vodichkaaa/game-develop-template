using System;
using System.Collections.Generic;

namespace Game.Scripts.Services.Registry
{
    public class RegistryService: IRegistryService
    {
        private readonly Dictionary<Type, object> _entities = new();

        public void Register<T>(T instance)
        {
            _entities[typeof(T)] = instance;
        }

        public T Get<T>()
        {
            if (_entities.TryGetValue(typeof(T), out var entity))
                return (T)entity;
        
            return default;
        }

        public void Remove<T>() => _entities.Remove(typeof(T));
        public void Clear() => _entities.Clear();
    }
}