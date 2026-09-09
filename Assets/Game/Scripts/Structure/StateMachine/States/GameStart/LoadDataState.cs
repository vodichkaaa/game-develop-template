using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Services.SaveLoad;
using Game.Scripts.Services.SceneLoader;
using Game.Scripts.Services.Sound;
using Game.Scripts.Services.StaticData;
using Game.Scripts.Structure.StateMachine.GameStateMachine;
using Game.Scripts.Structure.StateMachine.States.Main;
using UnityEngine;

namespace Game.Scripts.Structure.StateMachine.States.GameStart
{
    public class LoadDataState : IState, IExitableState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly ISoundService _soundService;
        private readonly ISceneLoader _sceneLoader;
        private const string MenuScene = "Menu";

        public LoadDataState(IStateMachine stateMachine, IStaticData staticData, ISaveLoad saveLoad, 
            ISoundService soundService, ISceneLoader sceneLoader)
        {
            _staticData = staticData;
            _saveLoad = saveLoad;
            _soundService = soundService;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public UniTask Enter(CancellationToken cts)        
        {
            _saveLoad.Load();
            _soundService.Construct(_saveLoad, _staticData.SoundData);
            _sceneLoader.LoadScene(MenuScene, PrepareGame);
            return default;
        }
        
        private void PrepareGame()
                {
                    // Fall back to sane defaults if the optional StaticData/GameSettings asset is missing.
                    Application.targetFrameRate = _staticData.GameSettings != null
                        ? _staticData.GameSettings.TargetFrameRate
                        : 60;
                    QualitySettings.vSyncCount = _staticData.GameSettings != null
                        ? _staticData.GameSettings.VSyncCount
                        : 0;

                    _stateMachine.Enter<MainState>();
                }

        public UniTask Exit(CancellationToken cts)
        {
            return default;
        }
    }
}