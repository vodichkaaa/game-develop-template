using Game.Scripts.Core.EventBus;
using VContainer;

namespace Game.Scripts.Game.UI.Architecture
{
    public abstract class BaseModel
    {
        [Inject]
        protected IEventBus eventBus;

        public virtual void Init()
        {
            
        }

        public virtual void Dispose()
        {

        }
    }
}