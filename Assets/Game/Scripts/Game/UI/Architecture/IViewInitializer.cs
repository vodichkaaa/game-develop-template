using VContainer;

namespace Game.Scripts.Game.UI.Architecture
{
    public interface IViewInitializer
    {
        BaseModel CreateModel(IObjectResolver container);
    }
}