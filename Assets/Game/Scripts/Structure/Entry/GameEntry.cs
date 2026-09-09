using Game.Scripts.Services.Factories.StateFactory;
using Game.Scripts.Structure.StateMachine.GameStateMachine;
using Game.Scripts.Structure.StateMachine.States.GameStart;
using VContainer.Unity;

namespace Game.Scripts.Structure.Entry
{
    public class GameEntry : IStartable
    {
        private readonly IStateFactory _stateFactory;
        private readonly IStateMachine _stateMachine;

        public GameEntry(IStateMachine stateMachine, IStateFactory stateFactory)
        {
            _stateMachine = stateMachine;
            _stateFactory = stateFactory;
        }

        public void Start()
        {
            _stateFactory.CreateAllStates();
            _stateMachine.Enter<LoadDataState>();
        }
    }
}