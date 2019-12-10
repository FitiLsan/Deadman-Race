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

        public void Initialize()
        {
            var carData = Resources.Load<CarTemplate>("CarData/TestTemplate");

            if (carData == null) return;

            var carModel = new CarModel(carData);

            _context.PlayerCar = carModel;

            var equipment = Object.FindObjectOfType<Equipment>();
            equipment.AttachObject(carModel.CarTransform);
        }
    }
}
//var horizontal = Input.GetAxis("Horizontal");
//var vertical = Input.GetAxis("Vertical");
//var handbreak = Input.GetAxis("Jump");

//_drivableObj.Drive(horizontal, vertical, vertical, handbreak);