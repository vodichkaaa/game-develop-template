using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.EventBus;
using Game.Scripts.Game.UI.Architecture;
using Game.Scripts.Services.Factories.UIFactory;
using VContainer;
using Object = UnityEngine.Object;

namespace Game.Scripts.Structure.StateMachine.States.Abstractions
{
    public class UIState<TModel, TView> : SimpleState, IDisposable
        where TModel : BaseModel, new()
        where TView : BaseView<TModel>
    {
        private IObjectResolver _container;
        private IUIFactory _uiFactory;
        protected IEventBus eventBus;

        protected TModel Model { get; private set; }
        protected TView View { get; private set; }

        [Inject]
        public void Construct(IUIFactory uiFactory, IObjectResolver container, IEventBus parEventBus)
        {
            _uiFactory = uiFactory;
            _container = container;
            eventBus = parEventBus;
        }

        public override async UniTask Enter(CancellationToken ct)
        {
            // Recreate the view if it was never created or was destroyed on a previous Exit
            // (Exit destroys the GameObject), so re-entering always has a live view.
            if (View == null || View.gameObject == null)
                await CreateView(ct);

            View.Show();
        }

        public override async UniTask Exit(CancellationToken ct)
        {
            View.Hide();
            Dispose();
            await UniTask.CompletedTask;
        }

        private async UniTask CreateView(CancellationToken ct)
        {
            (TModel viewModel, TView view) result =
                await _uiFactory.CreateViewAsync<TModel, TView>(_container, ct);
            View = result.view;
            Model = result.viewModel;
        }

        public void Dispose()
        {
            FullCleanup();
        }

        private void FullCleanup()
        {
            if (Model != null)
                Model.Dispose();

            if (View != null && View.gameObject != null)
                Object.Destroy(View.gameObject);
        }
    }
}