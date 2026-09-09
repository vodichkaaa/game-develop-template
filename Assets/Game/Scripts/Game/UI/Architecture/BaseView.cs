using System;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.UI.Architecture
{
    public abstract class BaseView<TModel>: MonoBehaviour, IViewInitializer
        where TModel: BaseModel
    {
        private bool _isBound;

        public TModel Model { get; private set; }

        public BaseModel CreateModel(IObjectResolver container)
        {
            var vm = Activator.CreateInstance<TModel>();
            if (container != null)
                container.Inject(vm);
            Model = vm;
            Model.Init();
            SubscribeModel();
            return vm;
        }

        public void SetModel(TModel model)
        {
            Model = model;
            Model.Init();
            SubscribeModel();
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeModel();
        }

        private void SubscribeModel()
        {
            if (Model == null || _isBound)
                return;

            _isBound = true;
        }

        private void UnsubscribeModel()
        {
            if (Model == null || !_isBound)
                return;
            
            Model.Dispose();

            _isBound = false;
            Model = null;
        }

        public abstract void SubscribeView();
        public abstract void UnsubscribeView();

        public virtual void OnChildViewsInitialized() { }

        public virtual void Show()
        {
            if (this == null || gameObject == null) return;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (this == null || gameObject == null) return;
            gameObject.SetActive(false);
        }
    }
}