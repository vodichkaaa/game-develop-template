using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Game.Scripts.Structure.StateMachine.States;
using UnityEngine;

namespace Game.Scripts.Structure.StateMachine.GameStateMachine
{
    public class StateMachine : IStateMachine
    {
        private readonly IDictionary<Type, object> _states = new Dictionary<Type, object>(10);
        private object _activeState;
        
        public object ActiveState => _activeState;

        public void Enter<TState>() where TState : class, IState
        {
            ChangeState<TState>().Enter(CancellationToken.None);
        }

        public void Enter<TState, TPayload>(TPayload payload) 
            where TState : class, IPayloadedState<TPayload> =>
            ChangeState<TState>().Enter(payload, CancellationToken.None);

        public void Enter<TState, TPayload, TPayload2>(TPayload payload, TPayload2 payload2)
            where TState : class, IPayloadedState<TPayload, TPayload2>
        {
            ChangeState<TState>().Enter(payload, payload2, CancellationToken.None);
        }

        public void Enter<TState, TPayload, TPayload2, TPayload3>(TPayload payload, TPayload2 payload2, TPayload3 payload3) where TState : class, IPayloadedState<TPayload, TPayload2, TPayload3>
        {
            ChangeState<TState>().Enter(payload, payload2, payload3, CancellationToken.None);
        }

        public bool IsActive<TState>() where TState : class, IState
        {
            TState state = GetState<TState>();
            return _activeState == state;
        }
        
        // Overload for states that ARE NOT exitable
        public void AddState(Type type, object instance) => _states.Add(type, instance);

        public void AddState<TState>(TState instance) where TState : class, IState =>
            _states.Add(typeof(TState), instance);

        public void AddState<TState, TPayload>(TState instance) where TState : class, IPayloadedState<TPayload> =>
            _states.Add(typeof(TState), instance);

        public void AddState(Type type, IExitableState instance) => _states.Add(type, instance);

        private TState ChangeState<TState>() where TState : class
        {
            TState state = GetState<TState>();

            // Guard against re-entering the currently active state, which would
            // double-exit it and spam the transition logs.
            if (ReferenceEquals(_activeState, state))
                return state;

            if (_activeState is IExitableState exitable)
            {
                Debug.Log($"Exiting: {_activeState.GetType().Name}");
                exitable.Exit(CancellationToken.None);
            }

            _activeState = state;
            
            Debug.Log($"Entering: {typeof(TState).Name}");
            return state;
        }

        public TState GetState<TState>() where TState : class =>
            _states[typeof(TState)] as TState;
        
        public IEnumerable<T> GetAllStates<T>() where T : class
        {
            return _states.Values.OfType<T>();
        }
        
        ~StateMachine()
        {
            if (_activeState is IExitableState exitable)
                exitable.Exit(CancellationToken.None);
        }
    }
}