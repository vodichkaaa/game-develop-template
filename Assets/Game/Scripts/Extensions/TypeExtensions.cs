using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Scripts.Services;
using Game.Scripts.Services.Factories.BaseFactory;
using Game.Scripts.Structure.StateMachine.States;
using UnityEngine;

namespace Game.Scripts.Extensions
{
    public static class TypeExtensions
    {
        private static IEnumerable<Type> _cachedStateTypes;
        private static IEnumerable<Type> _cachedLoadableStateTypes;
        private static IEnumerable<(Type, Type)> _cachedServiceTypes;
        
        public static IEnumerable<Type> GetAllStatesTypes()
        {
            if (_cachedStateTypes != null) return _cachedStateTypes;

            _cachedStateTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition &&
                               (typeof(IState).IsAssignableFrom(type) ||
                                typeof(IPayload).IsAssignableFrom(type) ||
                                typeof(IExitableState).IsAssignableFrom(type)))
                .ToList();

            Debug.Log($"[TypeExtensions] Found {_cachedStateTypes.Count()} state types");
            return _cachedStateTypes;
        }
        
        public static IEnumerable<(Type, Type)> GetAllGlobalServiceTypes()
        {
            if (_cachedServiceTypes != null)
                return _cachedServiceTypes;
            
            List<(Type, Type)> services = new List<(Type, Type)>(12);
            
            IEnumerable<Type> serviceTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IGlobalService).IsAssignableFrom(type) 
                            && type.IsClass 
                            && !type.IsAbstract);

            foreach (Type serviceType in serviceTypes)
            {
                Type instanceType = serviceType;
                
                Type interfaceType = serviceType.GetInterfaces()
                    .Where(type => type != typeof(IGlobalService) 
                                && type != typeof(IBaseFactory)
                                && !type.IsGenericTypeDefinition)
                    .OrderBy(type => type.GetInterfaces().Length) 
                    .FirstOrDefault();
                
                if (interfaceType == null)
                {
                    Debug.LogWarning($"[TypeExtensions] Service {serviceType.Name} has no valid interface!");
                    continue;
                }
                
                services.Add((instanceType, interfaceType));
                Debug.Log($"[TypeExtensions] Registered: {interfaceType.Name} -> {instanceType.Name}");
            }
            
            _cachedServiceTypes = services;
            return _cachedServiceTypes;
        }
        
        public static void ClearCache()
        {
            _cachedStateTypes = null;
            _cachedLoadableStateTypes = null;
            _cachedServiceTypes = null;
        }
    }
}