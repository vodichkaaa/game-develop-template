using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Structure.StateMachine.States.Abstractions
{
    public abstract class SimpleState : IState, IExitableState
    {
        public abstract UniTask Enter(CancellationToken cts);
        public abstract UniTask Exit(CancellationToken cts);
    }
}