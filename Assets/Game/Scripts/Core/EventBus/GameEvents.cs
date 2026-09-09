namespace Game.Scripts.Core.EventBus
{
    public class LevelChangedEvent : Event<int>
    {
        public LevelChangedEvent(int currentLevel) : base(currentLevel) { }
    }
    
    public class ScoreChangedEvent : Event<int>
    {
        public ScoreChangedEvent(int score) : base(score) { }
    }

    public class BalanceChangedEvent : Event<int>
    {
        public BalanceChangedEvent(int balance) : base(balance) { }
        
    }
}