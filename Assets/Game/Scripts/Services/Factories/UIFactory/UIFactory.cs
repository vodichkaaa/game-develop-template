using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Game.UI.Architecture;
using Game.Scripts.Services.Assets;
using Game.Scripts.Services.SaveLoad;
using Game.Scripts.Services.Sound;
using Game.Scripts.Services.StaticData;
using Game.Scripts.Structure.StateMachine.GameStateMachine;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Services.Factories.UIFactory
{
    public class UIFactory: BaseFactory.BaseFactory, IUIFactory
    {
        private readonly IStaticData _staticData;
        private readonly IStateMachine _stateMachine;
        private readonly ISoundService _soundService;
        private readonly ISaveLoad _saveLoad;
        
        private const string RootCanvasKey = "RootCanvas";

        private Canvas _rootCanvas;
        
        public UIFactory(IStaticData staticData, IAssets assets, IObjectResolver objectResolver, 
            ISoundService soundService, ISaveLoad saveLoad, IStateMachine stateMachine)
            : base(assets, objectResolver)
        {
            _staticData = staticData;
            _soundService = soundService;
            _saveLoad = saveLoad;
            _stateMachine = stateMachine;
        }

        public void SetRootCanvas(Canvas canvas)
        {
            _rootCanvas = canvas;
        }

        public async UniTask<(TModel model, TView view)> CreateViewAsync<TModel, TView>(
            IObjectResolver scopeContainer,
            CancellationToken ct)
            where TModel : BaseModel, new()
            where TView : BaseView<TModel>
        {
            if (_rootCanvas == null)
            {
                Debug.LogError("UIFactory: RootCanvas is not set. Call SetRootCanvas before creating views.");
                return (default, null);
            }

            var view = await InstantiateAsRegistered<TView>(_rootCanvas.transform);
            
            if (view == null)
            {
                Debug.LogError($"UIFactory: Failed to instantiate view of type {typeof(TView).Name}");
                return (default, null);
            }
            
            Debug.Log($"<color=#07FF6D>UIFactory MainView:</color> Initializing view of type {view.GetType().Name}");

            BaseModel baseModel = view.CreateModel(scopeContainer);
            InitializeChildViews(view, scopeContainer);
            view.OnChildViewsInitialized();
            return ((TModel)baseModel, view);
        }
        
        public void InitializeChildViews(MonoBehaviour rootInstance, IObjectResolver scopeContainer)
        {
            IViewInitializer[] allViews = rootInstance.gameObject.GetComponentsInChildren<IViewInitializer>(true);

            foreach (IViewInitializer view in allViews)
            {
                if (ReferenceEquals(view, rootInstance))
                    continue;
                
                Debug.Log($"<color=#369DD7>UIFactory ChildView:</color> Initializing view of type {view.GetType().Name}");

                view.CreateModel(scopeContainer);
            }
        }
    }
}