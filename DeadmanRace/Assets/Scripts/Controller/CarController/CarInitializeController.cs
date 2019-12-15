using UnityEngine;
using DeadmanRace.Components;
using DeadmanRace.Items;
using DeadmanRace.Models;


namespace DeadmanRace
{
    public class CarInitializeController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public CarInitializeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            var carData = Resources.Load<CarTemplate>("CarData/TestTemplate");

            if (carData == null) return;

            var carModel = new CarModel(carData);

            _context.PlayerCar = carModel;
            _context.CarEquipment.AttachObject(carModel.CarTransform);
        }

        #endregion
    }
}