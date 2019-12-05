namespace DeadmanRace
{
    public sealed class MainControllers : Controllers
    {
        #region ClassLifeCycles
        
        public MainControllers(GameContext context, Services services)
        {
            Add(new InitializationPlayerController(context, services));
            
            Add(new InputController(context, services));
        }

        #endregion
    }
}
