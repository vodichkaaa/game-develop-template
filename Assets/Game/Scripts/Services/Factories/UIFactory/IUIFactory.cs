using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Game.UI.Architecture;
using Game.Scripts.Services.Factories.BaseFactory;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Services.Factories.UIFactory
{
    public interface IUIFactory : IBaseFactory, IGlobalService
    {
        void SetRootCanvas(Canvas canvas);
        
        UniTask<(TModel model, TView view)> CreateViewAsync<TModel, TView>(IObjectResolver scopeContainer, CancellationToken ct)
            where TModel : BaseModel, new()
            where TView : BaseView<TModel>;
    }
}