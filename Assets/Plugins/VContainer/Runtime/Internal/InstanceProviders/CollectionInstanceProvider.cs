using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
    internal sealed class CollectionInstanceProvider : IInstanceProvider, IEnumerable<Registration>
    {

        private readonly List<Type> interfaceTypes;
        private readonly List<Registration> registrations = new List<Registration>();

        public CollectionInstanceProvider(Type elementType)
        {
            ElementType = elementType;
            ImplementationType = elementType.MakeArrayType();
            interfaceTypes = new List<Type>
            {
                RuntimeTypeCache.EnumerableTypeOf(elementType),
                RuntimeTypeCache.ReadOnlyListTypeOf(elementType)
            };
        }

        public Type ImplementationType { get; }
        public IReadOnlyList<Type> InterfaceTypes => interfaceTypes;
        public Lifetime Lifetime => Lifetime.Transient; // Collection reference is transient. So its members can have each lifetimes.

        public Type ElementType { get; }
        IEnumerator<Registration> IEnumerable<Registration>.GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver)
        {
            if (resolver is IScopedObjectResolver scope)
            {
                var entirelyRegistrations = CollectFromParentScopes(scope);
                return SpawnInstance(resolver, entirelyRegistrations);
            }
            return SpawnInstance(resolver, registrations);
        }
        public static bool Match(Type openGenericType)
        {
            return openGenericType == typeof(IEnumerable<>) ||
                   openGenericType == typeof(IReadOnlyList<>);
        }

        public List<Registration>.Enumerator GetEnumerator()
        {
            return registrations.GetEnumerator();
        }

        public override string ToString()
        {
            var contractTypes = InterfaceTypes != null ? string.Join(", ", InterfaceTypes) : "";
            return $"CollectionRegistration {ImplementationType} ContractTypes=[{contractTypes}] {Lifetime}";
        }

        public void Add(Registration registration)
        {
            foreach (var x in registrations)
            {
                if (x.Lifetime == Lifetime.Singleton && x.ImplementationType == registration.ImplementationType)
                {
                    throw new VContainerException(registration.ImplementationType, $"Conflict implementation type : {registration}");
                }
            }
            registrations.Add(registration);
        }

        internal object SpawnInstance(
            IObjectResolver resolver,
            IReadOnlyList<Registration> registrations)
        {
            var array = Array.CreateInstance(ElementType, registrations.Count);
            for (var i = 0; i < registrations.Count; i++)
            {
                array.SetValue(resolver.Resolve(registrations[i]), i);
            }
            return array;
        }

        internal List<Registration> CollectFromParentScopes(
            IScopedObjectResolver scope,
            bool localScopeOnly = false)
        {
            if (scope.Parent == null)
            {
                return registrations;
            }

            var finderType = InterfaceTypes[0];
            List<Registration> mergedRegistrations = null;

            scope = scope.Parent;
            while (scope != null)
            {
                if (scope.TryGetRegistration(finderType, out var registration) &&
                    registration.Provider is CollectionInstanceProvider parentCollection)
                {
                    if (mergedRegistrations == null)
                    {
                        mergedRegistrations = new List<Registration>(registrations);
                    }

                    if (localScopeOnly)
                    {
                        foreach (var x in parentCollection.registrations)
                        {
                            if (x.Lifetime != Lifetime.Singleton)
                            {
                                mergedRegistrations.Add(x);
                            }
                        }
                    }
                    else
                    {
                        mergedRegistrations.AddRange(parentCollection.registrations);
                    }
                }
                scope = scope.Parent;
            }
            return mergedRegistrations ?? registrations;
        }
    }
}