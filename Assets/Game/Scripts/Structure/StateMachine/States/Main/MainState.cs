using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.EventBus;
using Game.Scripts.Data.Enums;
using Game.Scripts.Game.Logic.Main;
using Game.Scripts.Game.UI.MainScreen;
using Game.Scripts.Services.LoadingCurtain;
using Game.Scripts.Services.SaveLoad;
using Game.Scripts.Services.Sound;
using Game.Scripts.Structure.StateMachine.GameStateMachine;
using Game.Scripts.Structure.StateMachine.States.Abstractions;
using Game.Scripts.Structure.StateMachine.States.Game;

namespace Game.Scripts.Structure.StateMachine.States.Main
{
    public class MainState : UIState<MainModel, MainView>
    {
        protected ISaveLoad _saveLoad;
        protected ISoundService _soundService;
        protected ILoadingCurtain _loadingCurtain;

        private readonly IStateMachine _stateMachine;
        private CancellationTokenSource _cancellationTokenSource;

        private Action<BalanceChangedEvent> _onBalanceChanged;

        public MainState(IStateMachine stateMachine, ILoadingCurtain loadingCurtain,
            ISaveLoad saveLoad, ISoundService soundService)
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
            View.OnPlayClick.AddListener(SwitchGameState);

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

            View.OnPlayClick.RemoveAllListeners();
            View.UnsubscribeView();

            _loadingCurtain.Show();
            await base.Exit(ct);
        }

        private void SubscribeToModel()
        {
            _onBalanceChanged = eventObj => View.UpdateBalance(eventObj.Value);
            eventBus.Subscribe(_onBalanceChanged);
        }

        private void UnsubscribeFromModel()
        {
            eventBus.Unsubscribe(_onBalanceChanged);
        }

        private void LoadInfo()
        {
            Model.SetBalanceWithoutNotify(_saveLoad.Progress.CurrentBalance);
        }

        private void SaveInfo()
        {
            _saveLoad.Progress.CurrentBalance = Model.Balance;
        }

        private void SwitchGameState()
        {
            _soundService.PlayEffectSound(SoundId.Click);
            _stateMachine.Enter<GameState>();
        }
    }
}