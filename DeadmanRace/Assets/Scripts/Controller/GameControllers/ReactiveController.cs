namespace DeadmanRace
{
    public abstract class ReactiveController
    {
        protected abstract void GetTrigger();

        protected abstract bool Filter();

        protected abstract void Execute();
    }
}
