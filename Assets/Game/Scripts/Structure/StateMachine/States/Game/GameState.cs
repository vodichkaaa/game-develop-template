using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.EventBus;
using Game.Scripts.Data.Enums;
using Game.Scripts.Game.Logic.Game;
using Game.Scripts.Game.UI.GameScreen;
using Game.Scripts.Services.LoadingCurtain;
using Game.Scripts.Services.SaveLoad;
using Game.Scripts.Services.Sound;
using Game.Scripts.Structure.StateMachine.GameStateMachine;
using Game.Scripts.Structure.StateMachine.States.Abstractions;
using Game.Scripts.Structure.StateMachine.States.Main;

namespace Game.Scripts.Structure.StateMachine.States.Game
{
    public class GameState : UIState<GameModel, GameView>
    {
        protected ISaveLoad _saveLoad;
        protected ISoundService _soundService;
        protected ILoadingCurtain _loadingCurtain;

        private readonly IStateMachine _stateMachine;
        private CancellationTokenSource _cancellationTokenSource;

        private Action<ScoreChangedEvent> _onScoreChanged;
        private Action<LevelChangedEvent> _onLevelChanged;
        private Action<BalanceChangedEvent> _onBalanceChanged;

        public GameState(IStateMachine stateMachine,
            ILoadingCurtain loadingCurtain, ISaveLoad saveLoad,
            ISoundService soundService)
        {
            _soundService = soundService;
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _saveLoad = saveLoad;
        }

        public override async UniTask Enter(CancellationToken ct)
        {
            await base.Enter(ct);
            _cancellationTokenSource = new CancellationTokenSource();

            View.SubscribeView();
                        View.OnBackClick.AddListener(SwitchBackState);

            SubscribeToModel();
            LoadInfo();

            _loadingCurtain.Hide();
        }

        public override async UniTask Exit(CancellationToken ct)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            SaveInfo();
            UnsubscribeFromModel();

            View.OnBackClick.RemoveAllListeners();
            View.UnsubscribeView();

            _loadingCurtain.Show();
            await base.Exit(ct);
        }

        private void SubscribeToModel()
        {
            _onScoreChanged = eventObj => View.UpdateScore(eventObj.Value);
            _onLevelChanged = eventObj => View.UpdateLevel(eventObj.Value);
            _onBalanceChanged = eventObj => View.UpdateBalance(eventObj.Value);

            eventBus.Subscribe(_onScoreChanged);
            eventBus.Subscribe(_onLevelChanged);
            eventBus.Subscribe(_onBalanceChanged);
        }

        private void UnsubscribeFromModel()
        {
            eventBus.Unsubscribe(_onScoreChanged);
            eventBus.Unsubscribe(_onLevelChanged);
            eventBus.Unsubscribe(_onBalanceChanged);
        }

        private void LoadInfo()
        {
            Model.LoadValues(0, _saveLoad.Progress.CurrentLevel, _saveLoad.Progress.CurrentBalance);
        }

        private void SaveInfo()
        {
            _saveLoad.Progress.CurrentBalance = Model.Balance;
            _saveLoad.Progress.CurrentLevel = Model.CurrentLevel;
        }

        private void SwitchBackState()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            _stateMachine.Enter<MainState>();
        }
    }
}