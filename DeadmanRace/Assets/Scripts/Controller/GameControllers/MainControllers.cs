using UnityEngine;
using DeadmanRace.Components;


namespace DeadmanRace
{
    public sealed class MainControllers : Controllers
    {
        #region ClassLifeCycles
        
        public MainControllers(GameContext context, Services services)
        {
            var carEquipment = Object.FindObjectOfType<Equipment>();
            context.CarEquipment = carEquipment;

            Add(new InitializationPlayerController(context, services));
            
            Add(new InputController(context, services));

            Add(new CarInitializeController(context, services));
        }

        #endregion
    }
}
