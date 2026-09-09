namespace Game.Scripts.Core.EventBus
{
    public interface IEvent { }

    public abstract class Event<T> : IEvent
    {
        public T Value { get; }

        protected Event(T value)
        {
            Value = value;
        }
    }

    public abstract class Event : IEvent
    {
    }
}