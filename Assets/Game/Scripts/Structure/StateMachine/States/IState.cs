using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Structure.StateMachine.States
{
    public interface IState
    {
        UniTask Enter(CancellationToken ct);
    }
    
    public interface IExitableState
    {
        UniTask Exit(CancellationToken ct);
    }

    public interface ILoadableState
    {
        UniTask Load(CancellationToken ct);
    }
    
    public interface IPayload
    {
        
    }

    public interface IPayloadedState<TPayload>: IPayload
    {
        UniTask Enter(TPayload payload, CancellationToken ct);
    }

    public interface IPayloadedState<TPayload, TPayload2>: IPayload
    {
        UniTask Enter(TPayload payload, TPayload2 payload2, CancellationToken ct);
    }

    public interface IPayloadedState<TPayload, TPayload2, TPayload3>: IPayload
    {
        UniTask Enter(TPayload payload, TPayload2 payload2, TPayload3 payload3, CancellationToken ct);
    }
}