namespace Game.Scripts.Services.Registry
{
    public interface IRegistryService: IGlobalService
    {
        void Register<T>(T instance);
        T Get<T>();
    }
}