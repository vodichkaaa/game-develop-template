using Game.Scripts.Extensions;
using Game.Scripts.Structure.StateMachine.GameStateMachine;
using Game.Scripts.Structure.StateMachine.States;
using VContainer;

namespace Game.Scripts.Services.Factories.StateFactory
{
    public class StateFactory : IStateFactory
    {
        private readonly IObjectResolver _objectResolver;
        private readonly IStateMachine _stateMachine;

        public StateFactory(IObjectResolver objectResolver, IStateMachine stateMachine)
        {
            _objectResolver = objectResolver;
            _stateMachine = stateMachine;
        }

        public void CreateAllStates()
        {
            foreach (var stateType in TypeExtensions.GetAllStatesTypes())
            {
                object stateInstance = _objectResolver.Resolve(stateType);

                if (typeof(IExitableState).IsAssignableFrom(stateType))
                    _stateMachine.AddState(stateType, stateInstance as IExitableState);
                else if (stateInstance is IState)
                    _stateMachine.AddState(stateType, stateInstance);
            }
        }
    }
}