using Game.Scripts.Data.Progress;

namespace Game.Scripts.Services.SaveLoad
{
    public interface ISaveLoad : IGlobalService
    {
        UserProgress Progress { get; set; }
        void Load();
    }
}