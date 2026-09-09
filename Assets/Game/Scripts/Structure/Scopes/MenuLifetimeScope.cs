using Game.Scripts.Services.Factories.UIFactory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Structure.Scopes
{
    public class MenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private Canvas _sceneRootCanvas;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_sceneRootCanvas).AsSelf();
        
            builder.RegisterBuildCallback(resolver =>
            {
                var factory = resolver.Resolve<IUIFactory>();
                factory.SetRootCanvas(_sceneRootCanvas);
            });
        }
    }
}